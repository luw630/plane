using System;
using System.Collections.Generic;
using xxlib;
using System.Diagnostics;
#if SERVER
using System.Collections.Concurrent;
using Server;
#endif

namespace PT
{
    /// <summary>
    /// 玩家代理, 客户端上下文. 当网络连接成功时, 创建本类. 收到的数据将位于 msgs
    /// </summary>
    partial class Player : IState
    {
        public Player() { }

#if SERVER
        public Player(Peer peer)
        {
            this.peer = peer;
            peer.player = this;
            isGuest = true;
        }
#endif

        public void Init(Scene scene)
        {
            this.scene = scene;

            enhanceLevels = new List<int>(3);                     // 初始强化表
            for (int i = 0; i < 3; i++) enhanceLevels.Add(0);

            State_WaitJoin = new State_WaitJoin_t(this);
            State_WaitBorn = new State_WaitBorn_t(this);
            State_Alive = new State_Alive_t(this);

            prevState = null;
            currState = State_WaitJoin;
            nextState = null;
            currState.Enter(scene.ticks);
        }

        public void Restore(Scene scene)
        {
            this.scene = scene;
            index_in_scene = scene.players.FindIndex(a => a.id == id);
            // plane 不用填。在还原 plane 的时候顺道填上

            State_WaitJoin = new State_WaitJoin_t(this);
            State_WaitBorn = new State_WaitBorn_t(this);
            State_Alive = new State_Alive_t(this);

            if (bornCDTicks > scene.ticks) currState = State_WaitBorn;
            else currState = State_Alive;
        }


#if SERVER
        /// <summary>
        /// 在收到玩家发送的 Join 之前, 是 guest 身份, 位于 guests 容器
        /// 便于 Update 后检查是否需要从容器中清除
        /// </summary>
        public bool isGuest = true;
#endif

        /// <summary>
        /// 指向场景实例
        /// </summary>
        public Scene scene;

        /// <summary>
        /// 附加一个位于 List 中的 index, 以便快速删除( 和最后一个交换 )
        /// </summary>
        public int index_in_scene = -1;

        /// <summary>
        /// 附加一个属于玩家的飞机容器( 没重生时为 null )
        /// </summary>
        public Plane plane = null;




        /// <summary>
        /// 创建指定型号 Plane 的一组函数
        /// </summary>
        static Func<Plane>[] creators = new Func<Plane>[]
        {
            ()=>{ return new Plane0(); },
            ()=>{ return new Plane1(); },
            ()=>{ return new Plane2(); },
            ()=>{ return new Plane3(); },
        };

        /// <summary>
        /// 创建 planeCfgId 型号 Plane 实例, 与 player 关联并放入 scene( 后面还需要 Init )
        /// </summary>
        public Plane CreatePlane(float a = 0)
        {
            Debug.Assert(plane == null);
            plane = creators[planeCfgId]();
            plane.Init(scene, this, a);

            // 在 player 中初始化 enhance 容器. 根据当前 plane 来调整数组长度并保留老数据
            if (enhanceLevels.Count < plane.cfg.enhanceIds.Count)
            {
                for (int i = 0; i < plane.cfg.enhanceIds.Count - enhanceLevels.Count; i++)
                {
                    enhanceLevels.Add(0);
                }
            }
            return plane;
        }


        /// <summary>
        /// 转为 waitborn 状态并开始等待 cd( cd 时间以当前飞机 cfg 为准 )
        /// </summary>
        public void Rebirth(int ticks)
        {
            Debug.Assert(plane != null);
            bornCDTicks = ticks + plane.cfg.bornIntervalTicks;
            scene.RemovePlane(plane);
            plane = null;
            currState.Leave();
            prevState = currState;
            currState = State_WaitBorn;
        }



        /// <summary>
        /// 产生通知, 断线, 并直接自杀并清掉场景中属于该玩家的所有 飞机, 子弹, 下一状态清空, 退出自杀
        /// </summary>
        public bool Quit()
        {
#if SERVER
            scene.events.quits.Add(id);
            Disconnect();
#endif
            nextState = null;
            return false;
        }





        /// <summary>
        /// 用以提供给 plane 调用以 记录增强日志, 以便重生时读回恢复 
        /// </summary>
        public void Enhance(int idx)
        {
            if (idx >= plane.cfg.enhanceEnergyCosts.Count) return;
            var price = plane.cfg.enhanceEnergyCosts[idx];
            if (energy < price) return;
            energy -= price;
            enhanceLevels[idx]++;
#if SERVER
            scene.events.enhances.Add(new Pkg.SC.Events.PlayerAction { playerId = id, index = idx });
#else
            DrawEnhance();
#endif
            if (plane != null) plane.Enhance();
        }

        /// <summary>
        /// 升级飞机( 如果当前 energy 够的话, 杀掉旧飞机, 创建新飞机. 还要计算当前飞机的增强花了多少  energy , 可抵扣. 清掉 player 上的 当前强化等级 )
        /// </summary>
        public void Evolution(int idx)
        {
            // 升级只能发生在飞机未阵亡之时( 不然状态不好设得 )
            if (plane == null) return;

            // 计算并消耗能量
            var price = GetEvolutionCost(plane.cfg.evolutionEnergyCost);
            if (price > energy) return;
            energy -= price;

            planeCfgId = plane.cfg.evolutionIds[idx];
            // 换飞机, 清强化等级
            var xybak = plane.xy;
            float anglebak = plane.angle;
            
            scene.RemovePlane(plane);
            plane = null;
            for (int i = 0; i < enhanceLevels.Count; i++)
            {
                enhanceLevels[i] = 0;
            }            
            CreatePlane(anglebak);
            plane.xy = xybak;            
            Shared.Log(plane.xy.ToString());
#if SERVER
            scene.events.evolutions.Add(new Pkg.SC.Events.PlayerAction { playerId = id, index = idx });
#else
            DrawEvolution();
#endif
        }

        /// <summary>
        /// 算所有已强化部位的已消耗能量值
        /// </summary>
        public int GetTotalEnhanceCost()
        {
            var costs = plane.cfg.enhanceEnergyCosts;
            int sum = 0;
            for (int j = 0; j < plane.cfg.enhances.Count; j++)
            {
                var lv = enhanceLevels[j];
                for (int i = 0; i < lv; ++i)
                {
                    sum += costs[i];
                }
            }
            return sum;
        }

        /// <summary>
        /// 算进阶价格
        /// </summary>
        public int GetEvolutionCost(int price)
        {
            var deduction = GetTotalEnhanceCost();
            if (deduction >= price) return 0;
            return price - deduction;
        }





        /// <summary>
        /// 设置下一个状态并退出的快捷函数, 一般前面加 return
        /// </summary>
        public bool ChangeState(IState state)
        {
            nextState = () => state;
            return false;
        }


        #region StateBase Code

        public IState prevState;
        public IState currState;
        public Func<IState> nextState;

        public bool Update(int ticks)
        {
            if (currState == null)
                return true;
            if (!currState.Update(ticks))
            {
                currState.Leave();
                if (nextState == null)
                    return false;
                prevState = currState;
                currState = nextState();
                if (currState == null)
                    return false;
                nextState = null;
                currState.Enter(ticks);
            }
            return true;
        }

        public void Enter(int ticks) { }
        public void Leave() { }

        #endregion



#if SERVER
        #region network

        public void Disconnect()
        {
            if (peer != null)
            {
                peer.player = null;
                peer.Dispose();
                peer = null;
            }
        }

        /// <summary>
        /// 指向 client peer ( 可空：即已断开 )，经由 Entry CreatePeer 时填充
        /// </summary>
        public Peer peer;

        /// <summary>
        /// client peer 的 OnOperationRequest 收到的的数据( 因为是在接收数据的线程中操作，故 concurrent )
        /// </summary>
        public ConcurrentQueue<IBBWriter> msgs = new ConcurrentQueue<IBBWriter>();


        // 针对同一 peer 的数据会确保不会多线同时投递, 故这些可以复用
        public ByteBuffer bb_read = new ByteBuffer();                               // for recv


        /// <summary>
        /// Peer 投递过来的 接收到的数据( 多线程 但是同一 Peer 始终只被一个线程处理 )
        /// msg[0] -> bytebuffer -> do{ decode -> object => msgs } while offset < dataLen
        /// </summary>
        public void PushMsgByPeer(byte[] buf)
        {
            bb_read.Assign(buf, buf.Length);
            while (bb_read.offset < bb_read.dataLen)
            {
                var o = Pkg.Pkg2Obj.Convert(bb_read);
                if (o != null) msgs.Enqueue(o);
                else
                {
                    msgs.Enqueue(Pkg.Sim.Disconnect.DefaultInstance);
                    break;
                }
            }
        }

        /// <summary>
        /// 使用 peer 发送消息, 返回 是否成功的交给底层发送
        /// 为简化编码, 返回值可以不用立即处理. 发送失败将导致 Peer.OnDisconnect 从而产生 断线或连接失败 模拟包
        /// </summary>
        public bool Send(byte[] buf)
        {
            if (peer == null) return false;
            return peer.Send(buf);
        }

        public bool Send(ByteBuffer bb)
        {
            return Send(bb.DumpData());
        }

        public bool Send(params IBBWriter[] os)
        {
            var bb = Looper.bb;
            bb.Clear();
            foreach (var o in os)
            {
                bb.Write(o.PackageId);
                bb.Write(o);
            }
            return Send(Looper.bb);
        }

        #endregion
#else
        // 模拟收包用的容器
        public Queue<IBBWriter> msgs = new Queue<IBBWriter>();
#endif

    }
}
