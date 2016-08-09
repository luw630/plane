using System;
using xxlib;
using static Shared;
using CS = Pkg.CS;

namespace Client
{
    /// <summary>
    /// 负责键盘输入, 显示输出
    /// </summary>
    public class Inputer
    {
        public Inputer(FSM fsm)
        {
            this.fsm = fsm;
            this.peer = fsm.peer;
        }

        public FSM fsm;
        public Peer peer;


        public enum Keys
        {
            Up, Down, Left, Right, Enter, Space, Escape, Go, MouseLeft, MouseRight, MouseMiddle, Move
        }
        bool[] keyDowns = new bool[12];
        bool[] lastKeyDowns = new bool[12];  // 备份上一次的值以便做对比
        public bool IsKeyDown(Keys k)
        {
            return keyDowns[(int)k];
        }
        public bool IsKeyStateChanged(Keys k)
        {
            return keyDowns[(int)k] != lastKeyDowns[(int)k];
        }
        public void SetKeyState(Keys k, bool v)
        {
            keyDowns[(int)k] = v;
        }
        public void StoreStates()
        {
            Array.Copy(keyDowns, lastKeyDowns, keyDowns.Length);
            lastMousePos.x = mousePos.x;
            lastMousePos.y = mousePos.y;
        }

        public XY mousePos;
        XY lastMousePos;
        public bool IsMouseMoved()
        {
            return !mousePos.EqualsTo(lastMousePos);
        }


        /// <summary>
        /// 用来限制输入发包的上次执行时间点
        /// </summary>
        long lastMS = GetCurrentMS();
        /// <summary>
        /// 上次执行时间到这次的经历时间的蓄水池
        /// </summary>
        long accumulatMS = 0;


        public void Update()
        {
            if (peer == null) return;

            var nowMS = GetCurrentMS();
            var durationMS = nowMS - lastMS;
            lastMS = nowMS;
            if (durationMS > Cfgs.logicFrameMS) durationMS = Cfgs.logicFrameMS;
            accumulatMS += durationMS;
            while (accumulatMS >= Cfgs.logicFrameMS)
            {
                accumulatMS -= Cfgs.logicFrameMS;

                // 处理用户输入消息: 读取鼠标坐标算朝向, 读取开火状态. 如果坐标离当前飞机很近 则认为停止移动. 

                var s = fsm.scene;
                var p = fsm.self.plane;
                if (p == null) return;

                // 判断是否有操作产生( 鼠标没有移动过的话就不算角度不发包了 )
                if (IsMouseMoved())
                {
                    // 算角度
                    CS.Rotate.DefaultInstance.angle = (float)XY.Angle(p.xy, mousePos);
                    peer.Send(CS.Rotate.DefaultInstance);

                    var d2 = XY.DistancePow2(p.xy, mousePos);
                    if (d2 > p.cfg.strikeRadius * p.cfg.strikeRadius)
                    {
                        // 设为移动状态
                        SetKeyState(Keys.Move, true);
                        if (IsKeyStateChanged(Keys.Move))        // 如果状态改变
                        {
                            peer.Send(CS.Move.DefaultPackageData);
                        }
                    }
                    else
                    {
                        // 设为停止移动状态
                        SetKeyState(Keys.Move, false);
                        if (IsKeyStateChanged(Keys.Move))        // 如果状态改变
                        {
                            peer.Send(CS.StopMove.DefaultPackageData);
                        }
                    }
                }

                if (IsKeyStateChanged(Keys.MouseLeft))
                {
                    if (IsKeyDown(Keys.MouseLeft))
                    {
                        var pkg = CS.Cast.DefaultInstance;
                        pkg.index = 0;
                        peer.Send(pkg);
                    }
                    else
                    {
                        var pkg = CS.StopCast.DefaultInstance;
                        pkg.index = 0;
                        peer.Send(pkg);
                    }
                }

                if (IsKeyStateChanged(Keys.MouseRight))
                {
                    if (IsKeyDown(Keys.MouseRight))
                    {
                        var pkg = CS.Cast.DefaultInstance;
                        pkg.index = 1;
                        peer.Send(pkg);
                    }
                    else
                    {
                        var pkg = CS.StopCast.DefaultInstance;
                        pkg.index = 1;
                        peer.Send(pkg);
                    }
                }

                if (IsKeyStateChanged(Keys.MouseMiddle) && IsKeyDown(Keys.MouseMiddle))
                {
                    var t = s.FindTarget(p, p.cfg.targetDetectDistance);
                    CS.Aim.DefaultInstance.planeId = t.tar == null ? -1 : t.tar.id;
                    peer.Send(CS.Aim.DefaultInstance);
                }

                if (IsKeyDown(Keys.Escape))
                {
                    peer.Send(CS.Quit.DefaultPackageData);
                }

                StoreStates();
            }
        }
    }
}
