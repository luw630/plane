using System;
using System.Collections.Generic;
using xxlib;
using SC = Pkg.SC;
using static Shared;
#if SERVER
using Server;
#endif

namespace PT
{
    /// <summary>
    /// 场景( 含 配置, 地图, 所有玩家 / 游戏对象 的分类存储体 )
    /// </summary>
    partial class Scene
    {
        public Cfgs cfgs;
        Fortrees fortrees1 = new Fortrees();
        Fortrees fortrees2 = new Fortrees();

        public void Init(int ticks)
        {
            cfgs = CfgLoader.Load();
            this.ticks = ticks;
            map.Init(this, 0);   // todo: 这里直接使用模拟的配置数据 0

            fortrees1.Init(fortrees2, ticks);
            fortrees2.Init(fortrees1, ticks, true);
        }
        public void Restore()
        {
            cfgs = CfgLoader.Load();
            map.Restore(this);
            foreach (Player p in players) p.Restore(this);
            foreach (var p in planes) p.Restore(this);
        }


#if SERVER
        /// <summary>
        /// 临时存一下当前帧加入的玩家
        /// </summary>
        public HashSet<Player> joins = new HashSet<Player>();

        /// <summary>
        /// 指向帧事件容器默认包( 没其他地方用. 为了方便 )
        /// </summary>
        public SC.EventSync events = SC.EventSync.DefaultInstance;
#endif


        /// <summary>
        /// 添加到 scene.players
        /// </summary>
        public void AddPlayer(Player a)
        {
            a.index_in_scene = players.Count;
            players.Add(a);
        }

        /// <summary>
        /// 从 scene.players 移除
        /// </summary>
        public void RemovePlayer(Player a)
        {
            // 根据当前策划内容, 玩家下线时需要清理其相关游戏对象.
            if (a.plane != null)
            {
                RemovePlane(a.plane);
                a.plane = null;
            }
#if SERVER
#else
            a.DrawDestroy();
#endif
            if (players.FastRemoveAt(a.index_in_scene))
            {
                players[a.index_in_scene].index_in_scene = a.index_in_scene;     // 修正换到当前位置的元素存的索引
            }
        }


        /// <summary>
        /// 向 scene.planes 中添加 p
        /// </summary>
        public void AddPlane(Plane p)
        {
            p.index_in_scene = planes.Count;
            planes.Add(p);
        }
        /// <summary>
        /// 从 scene.planes 中移除 p
        /// </summary>
        public void RemovePlane(Plane p)
        {
#if SERVER
#else
            p.DrawDestroy();
#endif
            foreach (Plane plane in planes) plane.ClearFocus(p.id);
            if (planes.FastRemoveAt(p.index_in_scene))
            {
                planes[p.index_in_scene].index_in_scene = p.index_in_scene;
            }
        }

        public void AddFortrees(Fortress f)
        {

        }

        public bool Update(int ticks)
        {
            this.ticks = ticks;

            // 1. 处理 players/agents 的消息输入( 主要是设置标记或保存一些行为目标, 并不产生 会影响到其他个体或公平性的的 行为 )
            for (int i = players.Count - 1; i >= 0; --i)    // 倒循环 for fast remove
            {
                var a = players[i];
                if (!a.Update(ticks))
                {
                    RemovePlayer(a);
                }
            }

   

            // 2. 处理移动逻辑
            // 3. 处理增益行为逻辑( 加血啥的， 受上限影响 )
            // 4. 处理减益行为结果逻辑( 这期间被打死的只是打死亡标记, 不影响其开火. 也就是说可能出现同归于尽的情况. )
            foreach (var p in planes) p.Update0(ticks);
            foreach (var p in planes) p.Update1(ticks);
            foreach (var p in planes) p.Update2(ticks);

            // 5. 找出上一步打死的飞机做清除操作
            for (int i = planes.Count - 1; i >= 0; --i)
            {
                var p = planes[i];
                if (p.hp > 0) continue;
                p.player.Rebirth(ticks);                                    // 干掉 0 血飞机 并令 player 切到 wait born 状态
            }

            // gas 相关逻辑
            map.Update(ticks);

#if SERVER
            // 广播给老玩家 差异事件数据( 如果有事件的话 )                 // 先注释掉 以确保每 frame 都有数据下发 简化前端 demo
            //if (joins.Count > 0
            //|| events.quits.Count > 0
            //|| events.rotates.Count > 0
            //|| events.casts.Count > 0
            //|| events.stopCasts.Count > 0
            //|| events.moves.Count > 0
            //|| events.stopMoves.Count > 0
            //|| events.aims.Count > 0
            //|| events.enhances.Count > 0
            //|| events.evolutions.Count > 0
            //)
            {
                events.joins = joins.Count;
                events.ticks = ticks;

                // 在 events 中附加调试数据
                events.debugInfo.Clear();
                events.debugInfo.Write(this);

                var buf = ByteBuffer.MakePackageData(Looper.bb, events);

                for (int i = 0; i < players.Count; ++i)
                {
                    var a = players[i];
                    if (joins.Contains(a)) continue;                        // 不发给刚 join 的这批玩家( 他们需要接收完整同步包 )
                    a.Send(buf);
                }
            }

            // 广播给刚 join 的这批玩家 完整同步数据
            if (joins.Count > 0)
            {
                var pkg = SC.FullSync.DefaultInstance;
                pkg.scene = this;

                var buf = ByteBuffer.MakePackageData(Looper.bb, pkg);

                //Log("Server full sync sending... joins.Count = " + joins.Count + ", ticks = " + ticks + ", bufLen = " + buf.Length);
                //var xx = new SC.FullSync();
                //var bb = new ByteBuffer(buf, buf.Length);
                //bb.offset = 2;
                //bb.Read(ref xx);
                //Log("bb.offset = " + bb.offset);

                foreach (var j in joins)
                {
                    j.Send(buf);
                }

            }

            // 清空事件广播容器备用
            joins.Clear();
            events.joins = 0;
            events.quits.Clear();
            events.rotates.Clear();
            events.casts.Clear();
            events.stopCasts.Clear();
            events.moves.Clear();
            events.stopMoves.Clear();
            events.aims.Clear();
            events.enhances.Clear();
            events.evolutions.Clear();
#endif
            return true;
        }

        public class FindTargetResult
        {
            public Plane tar;
            public XY p1;
            public XY p2;
        }

        /// <summary>
        /// 求线段与圆的交点离发射点的最短距离的敌机
        /// </summary>
        public FindTargetResult FindTarget(Plane p, float range)
        {
            Plane target = null;
            var minDis = float.PositiveInfinity;

            var xy1 = p.xy + p.xyIncBase * p.cfg.shootOffsets[0].x;       // 得到发射点坐标
            var xy2 = xy1 + p.xyIncBase * range;                    // 另一端坐标

            foreach (Plane p2 in planes)
            {
                if (p == p2 || p.player == p2.player || p.player.camp == p2.player.camp || p2.hp <= 0) continue;     // 本机或同一阵营或已死亡的跳过

                var dis2 = XY.DistancePow2(xy1, p2.xy);

                // 如果没有相交则 continue. 
                if (dis2 > p.cfg.targetDetectDistance * p.cfg.targetDetectDistance + p2.cfg.strikeRadius * p2.cfg.strikeRadius) continue;

                // 如果发射点位置被 p2 圆包容, 则直接选中 p2 并终止 foreach
                if (dis2 < p2.cfg.strikeRadius * p2.cfg.strikeRadius)
                {
                    target = p2;
                    minDis = 0;
                    break;
                }

                // 算线段与圆的交点距离
                var crossDis = XY.LineCrossCircleDis(xy1, xy2, p2.xy, p2.cfg.strikeRadius);
                if (float.IsNaN(crossDis)) continue;

                // 有相交则更新 minDis, target. 出去之后根据 minDis 来修正 p.fireXY2
                // 找出最小距离者
                if (minDis > crossDis)
                {
                    minDis = crossDis;
                    target = p2;
                }
            }
            if (target != null)
            {
                xy2 = xy1 + p.xyIncBase * minDis;                       // 修正另一端坐标
            }
            return new FindTargetResult { tar = target, p1 = xy1, p2 = xy2 };
        }

    }
}
