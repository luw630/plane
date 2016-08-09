using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxlib;
using Pkg;
using SC = Pkg.SC;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Test1
{
    public class Program
    {
        [DllImport("winmm")]
        static extern void timeBeginPeriod(int t);
        [DllImport("winmm")]
        static extern void timeEndPeriod(int t);


        // todo: 算角度的查表因为范围有限( 避免占掉太多内存 ), 故大于这个范围的检测, 直接套公式.
        // 根据角度查 xy 增量则不用. 0 ~ 1 的值转为
        // todo: 根据坐标算角度, 根据角度算增量, 算距离, 算距离的平方   的 float 版先实现.


        //public const short MAX_X = 1000, MAX_Y = 1000;
        //public short[,] xyas = new short[MAX_X * 2, MAX_Y * 2];     // MAX_X * MAX_Y 最大范围内每个格子对应的角度值. 中间为原点
        //public XY[] axys = new XY[65536];                           // 每个角度值对应的 xy 增量
        //public void FillXyas()
        //{
        //    var PI2 = Math.PI * 2;
        //    for (int x = 0; x < MAX_X * 2; ++x)
        //    {
        //        for (int y = 0; y < MAX_Y * 2; ++y)
        //        {
        //            var X = x - MAX_X;
        //            var Y = y - MAX_Y;
        //            if (X == 0 && Y == 0)
        //            {
        //                xyas[x, y] = 0;
        //            }
        //            else
        //            {
        //                xyas[x, y] = (short)(-Math.Atan2(Y, X) / PI2 * 32768);
        //            }
        //        }
        //    }

        //    for (int i = 0; i < 65536; ++i)  // 填充 360 度的 xy 增量表
        //    {
        //        var a = Math.PI * 2 / 65536 * i;
        //        axys[i].x = (short)(Math.Sin(a) * 65536);
        //        axys[i].y = (short)(Math.Cos(a) * 65536);
        //    }
        //}


        ///// <summary>
        ///// 返回 a 点到 b 点的前进角度( +-180度 对应 -32767 ~ 32768 )
        ///// </summary>
        //short GetAByXY(XY a, XY b)
        //{
        //    return xyas[b.x - a.x + MAX_X, b.y - a.y + MAX_Y];
        //}
        //short GetAByXY(short x, short y)
        //{
        //    return xyas[x, y];
        //}



        static void Main(string[] args)
        {
            timeBeginPeriod(1);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 999; ++i) Thread.Sleep(1);
            Console.WriteLine(sw.ElapsedMilliseconds);
            timeEndPeriod(1);
            Console.ReadLine();
        }
    }
}
