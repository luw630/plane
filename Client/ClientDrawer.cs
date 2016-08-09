using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using Client;
using System.Windows.Media;
using System.Windows;
using static Shared;


namespace PT
{
    partial class Scene
    {
        public Polygon v_border;
        // todo: 表格线子容器

        public void DrawInit()
        {
            // 设置场景显示容器 size
            var r = FSM.defaultInstance.v_root;
            r.Width = map.cfg.size.width;
            r.Height = map.cfg.size.height;

            // 画些边框    todo:  表格线作为背景
            var polygon = v_border = new Polygon { StrokeThickness = 1, Fill = Brushes.LightGray };
            polygon.Points.Add(new Point(0, 0));
            polygon.Points.Add(new Point(0, map.cfg.size.height));
            polygon.Points.Add(new Point(map.cfg.size.width, map.cfg.size.height));
            polygon.Points.Add(new Point(map.cfg.size.width, 0));
            polygon.SetPositon(0, 0);
            r.Children.Add(polygon);

            foreach (var o in planes) o.DrawInit();
            foreach (var area in map.areas)
            {
                foreach (var gas in area.gass) gas.DrawInit();
            }
        }

        public void DrawUpdate()
        {
            if (v_border != null)
            {
                // 定位到客户端玩家自己的飞机的坐标, 空间坐标转换, 令当前 plane 为显示的正中心
                var p = FSM.defaultInstance.self?.plane;
                if (p != null)
                {
                    FSM.defaultInstance.v_root.SetPositon(400 - p.xy.x, 300 - p.xy.y);
                }

                foreach (var o in planes) o.DrawUpdate();
                // gas 当前不需要 update
            }
            else DrawInit();
        }

        public void DrawDestroy()
        {
            if (v_border != null)
            {
                // 清掉 边框, 表格线
                FSM.defaultInstance.v_root.Children.Remove(v_border);
                v_border = null;
                // todo

                foreach (Player a in players) a.DrawDestroy();

                foreach (var area in map.areas)
                {
                    foreach (var gas in area.gass) gas.DrawDestroy();
                }
            }
        }
    }

    partial class Player
    {
        public void DrawDestroy()
        {
            if (plane != null)
            {
                plane.DrawDestroy();
            }
        }

        public void DrawEnhance() { }
    }


    partial class Gas
    {
        public Ellipse v_body;

        public void DrawInit()
        {
            // 绘制, 添加到 canvas
            var e = v_body = new Ellipse();
            e.Width = e.Height = cfg.radius * 2;
            e.StrokeThickness = 2;
            e.Stroke = Brushes.Green;
            e.SetPositon(xy.x - cfg.radius, xy.y - cfg.radius);
            FSM.defaultInstance.v_root.Children.Add(e);
        }
        public void DrawDestroy()
        {
            // 从 canvas 移除
            if (v_body != null)
            {
                FSM.defaultInstance.v_root.Children.Remove(v_body);
                v_body = null;
            }
        }
    }


    // todo: 继承关系下的 DrawXxxxxxx 的 base.Xxxxxx 调用需要调整下面的代码


    partial class Plane
    {
        public Canvas v_bodyroot;
        public Polygon v_body;
        public TextBlock v_id;
        public Rectangle v_hp;
        public Ellipse v_bornShield;
        public Ellipse v_target;

        public virtual void DrawInit()
        {
            // 绘制, 添加到 canvas ( 有可能包含激光绘制 )
            var fsm = FSM.defaultInstance;
            var c = fsm.v_root.Children;
            var s = fsm.scene;

            // 容器
            {
                v_bodyroot = new Canvas { Width = 0, Height = 0 };
                c.Add(v_bodyroot);
                v_bodyroot.SetPositon(xy.x, xy.y);
            }

            // 三角
            {
                v_body = new Polygon { StrokeThickness = 1, Fill = fsm.selfId == player.id ? Brushes.DimGray : Brushes.DarkGray };
                v_body.Points.Add(new Point(cfg.strikeRadius, 0));
                v_body.Points.Add(new Point(-cfg.strikeRadius, -cfg.strikeRadius));
                v_body.Points.Add(new Point(-cfg.strikeRadius, cfg.strikeRadius));
                v_body.RenderTransform = new RotateTransform(angle * 180.0);
                v_bodyroot.Children.Add(v_body);
            }

            // ID
            {
                v_id = new TextBlock();
                v_id.Text = player.camp.ToString() + " " + id.ToString();
                v_id.RenderTransform = new ScaleTransform(1, -1);
                v_bodyroot.Children.Add(v_id);
            }

            // 血条
            {
                v_hp = new Rectangle();
                v_hp.Width = hp;
                v_hp.Height = 5;
                v_hp.Fill = Brushes.Green;
                v_hp.StrokeThickness = 1;
                v_hp.Stroke = Brushes.Black;
                v_hp.SetPositon(-v_hp.Width / 2, -5);
                v_bodyroot.Children.Add(v_hp);
            }

            // 初生无敌盾
            if (cfg.bornShieldLifespanTicks > s.ticks)
            {
                var e = v_bornShield = new Ellipse();
                e.Width = e.Height = cfg.strikeRadius * 2;
                e.StrokeThickness = 2;
                e.Stroke = Brushes.Blue;
                e.SetPositon(xy.x - cfg.strikeRadius, xy.y - cfg.strikeRadius);
                c.Add(e);
            }

            // 如果是本机, 在焦点目标上显示个红圈圈
            if (fsm.self.id == playerId && target != null)
            {
                var e = v_target = new Ellipse();
                e.Width = e.Height = cfg.strikeRadius * 1.6;
                //e.Fill = Brushes.Red;
                e.StrokeThickness = 3;
                e.Stroke = Brushes.Red;
                e.SetPositon(target.xy.x - cfg.strikeRadius * 0.8, target.xy.y - cfg.strikeRadius * 0.8);
                c.Add(e);
            }
        }
        public virtual void DrawUpdate()
        {
            if (v_bodyroot != null)
            {
                var fsm = FSM.defaultInstance;
                var r = fsm.v_root;
                var c = r.Children;
                var s = fsm.scene;

                // 更新坐标
                v_bodyroot.SetPositon(xy.x, xy.y);

                // 更新朝向
                ((RotateTransform)v_body.RenderTransform).Angle = angle * 180.0;

                // 更新血长
                v_hp.Width = hp;

                if (cfg.bornShieldLifespanTicks > FSM.defaultInstance.scene.ticks)
                {
                    if (v_bornShield != null)
                    {
                        v_bornShield.SetPositon(xy.x - cfg.strikeRadius, xy.y - cfg.strikeRadius);
                    }
                }
                else
                {
                    c.Remove(v_bornShield);
                    v_bornShield = null;
                }

                // 如果是本机, 焦点红圈圈随目标移动
                if (fsm.self.id == playerId)
                {
                    if (target != null)
                    {
                        if (v_target == null)
                        {
                            var e = v_target = new Ellipse();
                            e.Width = e.Height = cfg.strikeRadius * 1.6;
                            //e.Fill = Brushes.Red;
                            e.StrokeThickness = 3;
                            e.Stroke = Brushes.Red;
                            e.SetPositon(target.xy.x - cfg.strikeRadius * 0.8, target.xy.y - cfg.strikeRadius * 0.8);
                            c.Add(e);
                        }
                        else
                        {
                            v_target.SetPositon(target.xy.x - cfg.strikeRadius * 0.8, target.xy.y - cfg.strikeRadius * 0.8);
                        }
                    }
                    else
                    {
                        if (v_target != null)
                        {
                            c.Remove(v_target);
                            v_target = null;
                        }
                    }
                }
            }
            else DrawInit();
        }
        public virtual void DrawDestroy()
        {
            if (v_body != null)
            {
                // 从 canvas 移除  ( 有可能包含激光的显示移除 )
                var c = FSM.defaultInstance.v_root.Children;
                c.Remove(v_bodyroot);
                v_bodyroot = null;
                v_body = null;
                v_id = null;
                v_hp = null;

                if (v_bornShield != null)
                {
                    c.Remove(v_bornShield);
                    v_bornShield = null;
                }

                if (v_target != null)
                {
                    c.Remove(v_target);
                    v_target = null;
                }
            }
        }
    }





    partial class Plane_Laser
    {
        public Line v_laser;

        public override void DrawInit()
        {
            base.DrawInit();

            var fsm = FSM.defaultInstance;
            var c = fsm.v_root.Children;
            var s = fsm.scene;

            // 激光
            if (fireBeforeTicks == s.ticks)
            {
                v_laser = new Line();
                v_laser.Stroke = Brushes.Red;
                v_laser.X1 = fireXY1.x;
                v_laser.X2 = fireXY2.x;
                v_laser.Y1 = fireXY1.y;
                v_laser.Y2 = fireXY2.y;
                c.Add(v_laser);
                Log("firing p1 = " + fireXY1.x + "," + fireXY1.y + " p2 = " + fireXY2.x + "," + fireXY2.y);

                ((RotateTransform)v_body.RenderTransform).Angle = fireAngle * 180.0;
            }
            else if (fireCDTicks > s.ticks)
            {
                // 开火过程角度 = 当前角度 + 当前角度与打击目标的角度差值 * 已经历的?摇时长 / ?摇总时长 
                var a = angle;
                var fa = fireAngle;
                var inc = xxlib.XY.AngleMinDiff(a, fa);
                if (fireBeforeTicks > s.ticks)
                {
                    inc = inc * (cfg.plane_Laser.startDelayTicks - (fireBeforeTicks - s.ticks)) / cfg.plane_Laser.startDelayTicks;
                    xxlib.XY.ChangeAngle(ref a, fa, inc);
                    ((RotateTransform)v_body.RenderTransform).Angle = a * 180.0;
                }
                else if (fireAfterTicks > s.ticks)
                {
                    inc = inc * (fireAfterTicks - s.ticks) / cfg.plane_Laser.endDelayTicks;
                    xxlib.XY.ChangeAngle(ref fa, a, inc);
                    ((RotateTransform)v_body.RenderTransform).Angle = fa * 180.0;
                }
            }

        }
        public override void DrawUpdate()
        {
            base.DrawUpdate();

            if (v_bodyroot != null)
            {
                var fsm = FSM.defaultInstance;
                var r = fsm.v_root;
                var c = r.Children;
                var s = fsm.scene;

                // 重绘激光
                if (v_laser != null)
                {
                    c.Remove(v_laser);
                    v_laser = null;
                }
                if (fireBeforeTicks == s.ticks)
                {
                    v_laser = new Line();
                    v_laser.Stroke = Brushes.Red;
                    v_laser.X1 = fireXY1.x;
                    v_laser.X2 = fireXY2.x;
                    v_laser.Y1 = fireXY1.y;
                    v_laser.Y2 = fireXY2.y;
                    c.Add(v_laser);
                    Log("firing p1 = " + fireXY1.x + "," + fireXY1.y + " p2 = " + fireXY2.x + "," + fireXY2.y);

                    ((RotateTransform)v_body.RenderTransform).Angle = fireAngle * 180.0;
                }
                else if (fireCDTicks > s.ticks)
                {
                    // 开火过程角度 = 当前角度 + 当前角度与打击目标的角度差值 * 已经历的?摇时长 / ?摇总时长 
                    var a = angle;
                    var fa = fireAngle;
                    var inc = xxlib.XY.AngleMinDiff(a, fa);
                    if (fireBeforeTicks > s.ticks)
                    {
                        inc = inc * (cfg.plane_Laser.startDelayTicks - (fireBeforeTicks - s.ticks)) / cfg.plane_Laser.startDelayTicks;
                        xxlib.XY.ChangeAngle(ref a, fa, inc);
                        ((RotateTransform)v_body.RenderTransform).Angle = a * 180.0;
                    }
                    else if (fireAfterTicks > s.ticks)
                    {
                        inc = inc * (fireAfterTicks - s.ticks) / cfg.plane_Laser.endDelayTicks;
                        xxlib.XY.ChangeAngle(ref fa, a, inc);
                        ((RotateTransform)v_body.RenderTransform).Angle = fa * 180.0;
                    }
                }

            }
            else DrawInit();
        }
        public override void DrawDestroy()
        {
            if (v_body != null)
            {
                // 激光的显示移除
                var c = FSM.defaultInstance.v_root.Children;

                if (v_laser != null)
                {
                    c.Remove(v_laser);
                    v_laser = null;
                }
            }
            base.DrawDestroy();
        }
    }



    partial class Plane0
    {
        public override void DrawInit()
        {
            base.DrawInit();
        }
        public override void DrawUpdate()
        {
            base.DrawUpdate();
        }
        public override void DrawDestroy()
        {
            base.DrawDestroy();
        }
    }

    partial class Plane1
    {
        public override void DrawInit()
        {
            base.DrawInit();
        }
        public override void DrawUpdate()
        {
            base.DrawUpdate();
        }
        public override void DrawDestroy()
        {
            base.DrawDestroy();
        }
        public void DrawRecover(int recover) { }
    }

    partial class Plane2
    {
        public override void DrawInit()
        {
            base.DrawInit();
        }
        public override void DrawUpdate()
        {
            base.DrawUpdate();
        }
        public override void DrawDestroy()
        {
            base.DrawDestroy();
        }
    }

}

public static class DrawUtils
{
    public static void SetPositon(this UIElement o, double x, double y)
    {
        Canvas.SetLeft(o, x);
        Canvas.SetTop(o, y);
    }

}