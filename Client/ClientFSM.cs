using System.Collections.Generic;
using xxlib;
using static Shared;
using PT;
using CS = Pkg.CS;
using SC = Pkg.SC;
using Sim = Pkg.Sim;
using System.Diagnostics;
using System.Windows.Controls;
using System;

namespace Client
{
    /// <summary>
    /// 管理网络连接状态, 负责通信, 重连, 创建 scene 并更新显示, 上传用户输入指令
    /// </summary>
    public class FSM : StateBase
    {
        /// <summary>
        /// 负责键盘输入
        /// </summary>
        public Inputer inputer;

        /// <summary>
        /// 负责网络输入消息队列, 网络发送
        /// </summary>
        public Peer peer;

        /// <summary>
        /// 指向当前客户端的逻辑代理
        /// </summary>
        public Player self;

        /// <summary>
        /// join 后存 当前客户端的逻辑代理 id
        /// </summary>
        public int selfId = -1;

        /// <summary>
        /// 场景在连接成功后收到 FullSync 包才创建, 收到数据包后才更新
        /// </summary>
        public Scene scene;


        // 找个地方放显示相关对象
        MainWindow v_mw;            // window handle
        public Canvas v_root;       // root canvas



        public FSM(MainWindow mw)
        {
            peer = new Peer();

            v_mw = mw;
            v_root = mw.root;
            inputer = new Inputer(this);

            State_Connect = new State_Connect_t(this);
            State_Connected = new State_Connected_t(this);

            defaultInstance = this;
        }

        public static FSM defaultInstance;

        public override void Enter(int ticks)
        {
            currState = State_Connect;
            State_Connect.Enter(ticks);
        }
        public override bool Update(int ticks)
        {
            peer.Update();
            return base.Update(ticks);
        }

        #region State_Connect

        State_Connect_t State_Connect;
        public class State_Connect_t : IState
        {
            FSM fsm;
            int timeout;
            bool connecting;
            int errCount;

            public State_Connect_t(FSM fsm)
            {
                this.fsm = fsm;
            }

            public void Enter(int ticks)
            {
                Log("State_Connect Enter");
                timeout = ticks + 3;               // 延迟 0.1 秒开始连
                connecting = false;
                errCount = 0;

                fsm.scene = null;
                fsm.selfId = -1;
            }

            public void Leave() { }

            // 逻辑：每次发起连接之后，等连接事件通知。如果没连上，不断重连重试。
            // 连上则退出该状态，切换到 State_Connected
            public bool Update(int ticks)
            {
                if (ticks > timeout)
                {
                    var ms = fsm.peer.msgs;
                    if (!connecting)
                    {
                        ms.Clear();
                        if (fsm.peer.Connect(Cfgs.serverIP))
                        {
                            Log("State_Connect Connecting " + Cfgs.serverIP);
                            connecting = true;
                        }
                        else
                        {
                            ++errCount;
                            if (errCount > 100)     // 出错多少次直接退了
                            {
                                fsm.nextState = null;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // 读所有。不认识的包直接跳过
                        while (ms.Count > 0)
                        {
                            var o = ms.Dequeue();
                            if (o.PackageId == Pkg.Sim.Disconnect.packageId)
                            {
                                Log("State_Connect Disconnected");
                                connecting = false;
                                timeout = ticks + 30;       // 延迟 2 秒继续连
                                break;
                            }
                            else if (o.PackageId == Pkg.Sim.Connect.packageId)
                            {
                                Log("State_Connect Connect Success");
                                fsm.nextState = () => fsm.State_Connected;              // 切换到 connected 状态
                                return false;
                            }
                            else
                            {
                                // 日志？
                            }
                        }
                        // 还没有读到事件包, 下个 Update 继续
                    }
                }
                return true;
            }
        }

        #endregion

        #region State_Connedted

        State_Connected_t State_Connected;
        public class State_Connected_t : StateBase
        {
            FSM fsm;
            public State_Connected_t(FSM fsm)
            {
                this.fsm = fsm;
                State_Join = new State_Join_t(fsm);
                State_Alive = new State_Alive_t(fsm);
            }

            public override void Enter(int ticks)
            {
                Log("State_Connedted Enter");
                currState = State_Join;
                currState.Enter(ticks);
            }

            #region State_Join

            State_Join_t State_Join;
            public class State_Join_t : IState
            {
                FSM fsm;
                bool sent = false;
                bool gotJoin = false;
                bool gotFullSync = false;
                int timeout = 0;

                public State_Join_t(FSM fsm)
                {
                    this.fsm = fsm;
                }

                public void Enter(int ticks)
                {
                    Log("State_Join Enter");
                    sent = gotJoin = gotFullSync = false;
                    timeout = ticks + 1000;
                }

                public void Leave() { }

                public bool Update(int ticks)
                {
                    // 如果没有发送过 进入游戏 就发, 否则就等回应( SC.Join, SC.FullSync ). 如果断线, 则重连
                    if (!sent)
                    {
                        fsm.peer.Send(CS.Join.DefaultPackageData);
                        sent = true;
                    }
                    else
                    {
                        if (ticks > timeout)                // 即便发送过. 也需要超时检测. 
                        {
                            fsm.peer.Disconnect();
                            timeout = ticks + 1000;
                        }

                        var ms = fsm.peer.msgs;
                        while (ms.Count > 0)
                        {
                            var o = ms.Dequeue();
                            Log(o.ToString());
                            if (!gotJoin && o.PackageId == SC.Join.packageId)
                            {
                                var pkg = o as Pkg.SC.Join;

                                // 记录下自己的 id
                                fsm.selfId = pkg.selfId;

                                gotJoin = true;
                            }
                            else if (gotJoin && !gotFullSync && o.PackageId == SC.FullSync.packageId)
                            {
                                var pkg = o as SC.FullSync;
                                pkg.scene.Init(ticks);
                                var s = fsm.scene = pkg.scene;
                                s.Restore();

                                // 指向自己的实例
                                fsm.self = s.players.Find(a => a.id == fsm.selfId);

                                // 切换到 alive 状态
                                fsm.nextState = () => fsm.State_Connected.State_Alive;
                                return false;
                            }
                            else if (o.PackageId == Pkg.Sim.Disconnect.packageId)
                            {
                                // 切换到 connect 状态
                                fsm.nextState = () => fsm.State_Connect;
                                return false;
                            }
                            else
                            {
                                // 日志？
                            }
                        }
                    }
                    return true;
                }
            }
            #endregion

            #region State_Alive

            State_Alive_t State_Alive;
            public class State_Alive_t : IState
            {
                FSM fsm;
                public State_Alive_t(FSM fsm)
                {
                    this.fsm = fsm;
                }
                public void Enter(int ticks)
                {
                    Log("State_Alive Enter");
                }
                public void Leave() { }

                public bool Update(int ticks)
                {
                    fsm.inputer.Update();

                    var ms = fsm.peer.msgs;
                    if (ms.Count == 0) return true;

                    var scene = fsm.scene;
                    while (ms.Count > 0)
                    {
                        var o = ms.Dequeue();
                        if (o.PackageId == Pkg.Sim.Disconnect.packageId)
                        {
                            //s.DrawDestroy();
                            fsm.nextState = null;
                            return false;
                        }
                        else if (o.PackageId != SC.EventSync.packageId)
                        {
                            Log("invalid PackageId = " + o.PackageId.ToString());
                            return true;
                        }

                        var pkg = o as SC.EventSync;

                        // 快进到收到的差异包的 ticks -1 帧( 最后一帧需要先填充事件消息再执行 )
                        while (scene.ticks < pkg.ticks - 1)
                        {
                            scene.ticks++;
                            scene.Update(scene.ticks);
                        }
                        scene.ticks = pkg.ticks;

                        // 下列均走模拟消息路线

                        foreach (var q in pkg.quits)
                        {
                            scene.players.Find(a => a.id == q).msgs.Enqueue(CS.Quit.DefaultInstance);
                        }

                        for (int i = 0; i < pkg.joins; ++i)
                        {
                            var plr = new Player();
                            plr.Init(scene);

                            plr.msgs.Enqueue(new CS.Join());        // 模拟 join
                            plr.Update(scene.ticks);                    // 令 join 立即完成

                            scene.AddPlayer(plr);
                        }

                        foreach (var r in pkg.rotates)
                        {
                            scene.players.Find(a => a.id == r.playerId).msgs.Enqueue(new Sim.Rotate { angle = r.angle, xyIncBase = r.xyIncBase });
                        }
                        foreach (var r in pkg.casts)
                        {
                            scene.players.Find(a => a.id == r.playerId).msgs.Enqueue(new CS.Cast { index = r.index });
                        }
                        foreach (var r in pkg.stopCasts)
                        {
                            scene.players.Find(a => a.id == r.playerId).msgs.Enqueue(new CS.StopCast { index = r.index });
                        }
                        foreach (var r in pkg.moves)
                        {
                            scene.players.Find(a => a.id == r).msgs.Enqueue(CS.Move.DefaultInstance);
                        }
                        foreach (var r in pkg.stopMoves)
                        {
                            scene.players.Find(a => a.id == r).msgs.Enqueue(CS.StopMove.DefaultInstance);
                        }
                        foreach (var r in pkg.aims)
                        {
                            scene.players.Find(a => a.id == r.playerId).msgs.Enqueue(new CS.Aim { planeId = r.planeId });
                        }
                        foreach (var r in pkg.enhances)
                        {
                            scene.players.Find(a => a.id == r.playerId).msgs.Enqueue(new CS.Enhance { index = r.index });
                        }
                        foreach (var r in pkg.evolutions)
                        {
                            scene.players.Find(a => a.id == r.playerId).msgs.Enqueue(new CS.Evolution { index = r.index });
                        }

                        // 令 scene 响应所有模拟消息 并跑逻辑
                        scene.Update(scene.ticks);

                        // 校验 FullSync 的值
                        if (pkg.debugInfo.dataLen > 0)
                        {
//                             var serverScene = new Scene();
//                             pkg.debugInfo.Read(ref serverScene);
// 
//                             var diff = scene.FindDiff(serverScene);
//                             if (diff != null)
//                             {
//                                 throw new Exception(diff);
//                             }
                        }
                    }

                    // 绘制
                    scene.DrawUpdate();

                    return true;
                }
            }
            #endregion
        }

        #endregion
    }
}
