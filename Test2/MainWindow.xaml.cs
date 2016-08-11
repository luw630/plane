using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Shared;
using xxlib;
using System.Windows.Threading;
using System.Diagnostics;

namespace Test2
{
    public static class ListXYExt
    {
        public static XY CircleAt(this List<XY> list, int idx)
        {
            if (idx < 0) return list[list.Count + idx];
            return list[idx % list.Count];
        }
    }

    public struct Size
    {
        public int width, height;
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    public class CardinalSplineTo
    {
        List<XY> points = new List<XY>();
        float tension;
        float duration;
        float deltaT;
        UIElement target;
        XY origin;
        float radius;
        XY accumulatedDiff;
        int timeScale = 100;

        public CardinalSplineTo(float duration, List<XY> points, float tension)
        {
            Debug.Assert(points.Count > 1);
            this.duration = duration;
            this.points.AddRange(points);
            this.tension = tension;
            deltaT = (float)1 / (points.Count - 1);
        }

        public void StartWithTarget(UIElement target, XY origin, float radius)
        {
            this.target = target;
            this.origin = origin;
            this.radius = radius;
            accumulatedDiff.SetZero();
        }

        public void Update(int ticks)
        {
             
            float time = (float)(ticks * Cfgs.logicFrameMS) / 1000.0f/ (float)timeScale;
            int p;
            float lt;
            // eg.
            // p..p..p..p..p..p..p
            // 1..2..3..4..5..6..7
            // want p to be 1, 2, 3, 4, 5, 6
            //                 if (time == 1)
            //                 {
            //                     p = points.Count - 1;
            //                     lt = 1;
            //                 }
            //                 else
            {
                p = (int)(time / deltaT);
                lt = (time - deltaT * (float)p) / deltaT;
            }

            // Interpolate
            var pp0 = points.CircleAt(p - 1);
            var pp1 = points.CircleAt(p + 0);
            var pp2 = points.CircleAt(p + 1);
            var pp3 = points.CircleAt(p + 2);

            var newPos = ccCardinalSplineAt(pp0, pp1, pp2, pp3, tension, lt);
            Log("newPos = " + newPos);

            UpdatePosition(newPos);




        }

        public void UpdatePosition(XY newPos)
        {
            newPos += origin;
            newPos.x -= radius;
            newPos.y -= radius;
            target.SetPositon(newPos);
        }

        // CatmullRom Spline formula:
        public static XY ccCardinalSplineAt(XY p0, XY p1, XY p2, XY p3, float tension, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            /*
             * Formula: s(-ttt + 2tt - t)P1 + s(-ttt + tt)P2 + (2ttt - 3tt + 1)P2 + s(ttt - 2tt + t)P3 + (-2ttt + 3tt)P3 + s(ttt - tt)P4
             */
            float s = (1 - tension) / 2;

            float b1 = s * ((-t3 + (2 * t2)) - t);                      // s(-t3 + 2 t2 - t)P1
            float b2 = s * (-t3 + t2) + (2 * t3 - 3 * t2 + 1);          // s(-t3 + t2)P2 + (2 t3 - 3 t2 + 1)P2
            float b3 = s * (t3 - 2 * t2 + t) + (-2 * t3 + 3 * t2);      // s(t3 - 2 t2 + t)P3 + (-2 t3 + 3 t2)P3
            float b4 = s * (t3 - t2);                                   // s(t3 - t2)P4

            float x = (p0.x * b1 + p1.x * b2 + p2.x * b3 + p3.x * b4);
            float y = (p0.y * b1 + p1.y * b2 + p2.y * b3 + p3.y * b4);

            return new XY(x, y);
        }
    }

    public class Fortrees
    {
        public Ellipse circle;
        public CardinalSplineTo action;

        enum FortreesStatus
        {
            MOVE,
            STOP,
            ONFIRE,
        }

        public void Init(Canvas root)
        {
            // 边距
            var dist = 100;

            // 园直径
            var r = 20.0f;

            // 每圈耗时秒数
            var sec = 3.0f;    //60.0f * 3;

            // 张力
            var z = 0.3f;

            //// 倒角长度
            //var a = 250;

            // 假设这个就是地图大小
            var s = new Size { width = (int)root.Width, height = (int)root.Height };

            // 原点坐标
            var op = new XY(dist, dist);

            // 轨迹矩形大小
            var wh = new Size { width = s.width - dist * 2, height = s.height - dist * 2 };

            var array = new List<XY>();
            //for (int i = 0; i < n; i++)
            //{
            array.Add(new XY(0, 0));
            array.Add(new XY(wh.width, 0));
            array.Add(new XY(wh.width, wh.height));
            array.Add(new XY(0, wh.height));
            //}
            action = new CardinalSplineTo(sec, array, z);


            // area
            var rect = new Rectangle();
            rect.Width = wh.width;
            rect.Height = wh.height;
            //rect.Fill = Brushes.Green;
            rect.StrokeThickness = 2;
            rect.Stroke = Brushes.Red;
            rect.SetPositon(op);
            root.Children.Add(rect);

            // circle
            circle = new Ellipse();
            circle.Width = circle.Height = r;
            //e.Fill = Brushes.Red;
            circle.StrokeThickness = 2;
            circle.Stroke = Brushes.Blue;
            root.Children.Add(circle);
            circle.SetPositon(op + new XY(-r / 2, -r / 2));

            action.StartWithTarget(circle, op, r / 2);

        }

        public void Update(int  ticks)
        {
            action.Update(ticks);
            Log("ticks = " + ticks);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
	 	Scene scene = new Scene();
        Fortrees fortrees = new Fortrees();

//         Ellipse circle;
//         CardinalSplineTo action;

        int ticks = 0;
        DispatcherTimer t;

        /// <summary>
        /// 用来限制输入发包的上次执行时间点
        /// </summary>
        public static long lastMS = GetCurrentMS();
        /// <summary>
        /// 上次执行时间到这次的经历时间的蓄水池
        /// </summary>
        public static long accumulatMS = 0;


        private void T_Tick(object sender, EventArgs e)
        {
            var nowMS = GetCurrentMS();
            var durationMS = nowMS - lastMS;
            lastMS = nowMS;
            if (durationMS > Cfgs.logicFrameMS) durationMS = Cfgs.logicFrameMS;
            accumulatMS += durationMS;
            bool executed = false;
            while (accumulatMS >= Cfgs.logicFrameMS)
            {
                executed = true;
                accumulatMS -= Cfgs.logicFrameMS;
                fortrees.Update(ticks);
				scene.Update(ticks);
                ++ticks;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            fortrees.Init(root);

            t = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            t.Tick += T_Tick;
            t.Interval = new TimeSpan(0, 0, 0, 0, 1);
            t.Start();
			scene.Init(root, ticks);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}

