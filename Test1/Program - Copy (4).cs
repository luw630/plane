using System;
using xxlib;

public class Program
{
    /// <summary>
    /// 求 线段 pa pb 与 半径r的圆(圆心pc) 的边缘重叠点 p1, p2
    /// </summary>
    public static bool LineInterCircle(XY pa, XY pb, XY pc, float r, ref XY p1, ref XY p2)
    {
        var fDis = (float)Math.Sqrt((pb.x - pa.x) * (pb.x - pa.x) + (pb.y - pa.y) * (pb.y - pa.y));

        XY d;
        d.x = (pb.x - pa.x) / fDis;
        d.y = (pb.y - pa.y) / fDis;

        XY e;
        e.x = pc.x - pa.x;
        e.y = pc.y - pa.y;

        var a = e.x * d.x + e.y * d.y;
        var a2 = a * a;

        var e2 = e.x * e.x + e.y * e.y;

        var r2 = r * r;

        if ((r2 - e2 + a2) < 0) return false;

        var f = (float)Math.Sqrt(r2 - e2 + a2);

        var t = a - f;

        if (((t - 0.0) > -float.Epsilon) && (t - fDis) < float.Epsilon)
        {
            p1.x = pa.x + t * d.x;
            p1.y = pa.y + t * d.y;
        }

        t = a + f;

        if (((t - 0.0) > -float.Epsilon) && (t - fDis) < float.Epsilon)
        {
            p2.x = pa.x + t * d.x;
            p2.y = pa.y + t * d.y;
        }
        return true;
    }



    /// <summary>
    /// 求 线段 pa pb 与 半径r的圆(圆心pc) 是否相交
    /// </summary>
    public static bool LineCrossCircle(XY pa, XY pb, XY pc, float r)
    {
        var fDis = (float)Math.Sqrt((pb.x - pa.x) * (pb.x - pa.x) + (pb.y - pa.y) * (pb.y - pa.y));

        XY d;
        d.x = (pb.x - pa.x) / fDis;
        d.y = (pb.y - pa.y) / fDis;

        XY e;
        e.x = pc.x - pa.x;
        e.y = pc.y - pa.y;

        var a = e.x * d.x + e.y * d.y;
        var a2 = a * a;

        var e2 = e.x * e.x + e.y * e.y;

        var r2 = r * r;

        return r2 - e2 + a2 >= 0;
    }



    /// <summary>
    /// 求 线段 pa pb 与 半径r的圆(圆心pc) 的离 pa 最近的边缘重叠点 p1 的距离
    /// </summary>
    public static float LineCrossCircleDis(XY pa, XY pb, XY pc, float r)
    {
        var fDis = (float)Math.Sqrt((pb.x - pa.x) * (pb.x - pa.x) + (pb.y - pa.y) * (pb.y - pa.y));

        XY d;
        d.x = (pb.x - pa.x) / fDis;
        d.y = (pb.y - pa.y) / fDis;

        XY e;
        e.x = pc.x - pa.x;
        e.y = pc.y - pa.y;

        var a = e.x * d.x + e.y * d.y;
        var a2 = a * a;

        var e2 = e.x * e.x + e.y * e.y;

        var r2 = r * r;

        if ((r2 - e2 + a2) < 0) return 0;

        var f = (float)Math.Sqrt(r2 - e2 + a2);

        return Math.Min(a - f, a+f);
    }




    static void Main(string[] args)
    {
        XY ptStart = new XY { x = 3.0f, y = 2.0f };
        XY ptEnd = new XY { x = 10.0f, y = 6.0f };
        XY ptCenter = new XY { x = 10.0f, y = 4.0f };
        float fR = 3.0f;

        XY pt1, pt2;
        pt1.x = pt1.y = pt2.x = pt2.y = float.NaN;

        if (LineInterCircle(ptStart, ptEnd, ptCenter, fR, ref pt1, ref pt2))
        {
            Console.WriteLine(" 不相交!");
        }
        else
        {
            Console.WriteLine(" 交点1" + pt1.x + " " + pt1.y);
            Console.WriteLine(" 交点2" + pt2.x + " " + pt2.y);
        }

    }
}
