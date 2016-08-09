using System;
using xxlib;

public class Program
{
    public const float EPS = 0.00001f;

    /**
* @brief 求线段与圆的交点
* @return 如果有交点返回true,否则返回false
* @note 与圆可能存在两个交点，如果存在两个交点在ptInter1和ptInter2都为有效值，如果有一个交点，则ptInter2的值为
*       无效值，此处为65536.0
*/
    public static bool LineInterCircle(
    XY ptStart, // 线段起点
    XY ptEnd, // 线段终点
    XY ptCenter, // 圆心坐标
    float Radius,
    ref XY ptInter1,
    ref XY ptInter2)
    {
        ptInter1.x = ptInter2.x = float.NaN;
        ptInter2.y = ptInter2.y = float.NaN;

        float fDis = (float)Math.Sqrt((ptEnd.x - ptStart.x) * (ptEnd.x - ptStart.x) + (ptEnd.y - ptStart.y) * (ptEnd.y - ptStart.y));

        XY d;
        d.x = (ptEnd.x - ptStart.x) / fDis;
        d.y = (ptEnd.y - ptStart.y) / fDis;

        XY E;
        E.x = ptCenter.x - ptStart.x;
        E.y = ptCenter.y - ptStart.y;

        float a = E.x * d.x + E.y * d.y;
        float a2 = a * a;

        float e2 = E.x * E.x + E.y * E.y;

        float r2 = Radius * Radius;

        if ((r2 - e2 + a2) < 0)
        {
            return false;
        }
        else
        {
            float f = (float)Math.Sqrt(r2 - e2 + a2);

            float t = a - f;

            if (((t - 0.0) > -EPS) && (t - fDis) < EPS)
            {
                ptInter1.x = ptStart.x + t * d.x;
                ptInter1.y = ptStart.y + t * d.y;
            }

            t = a + f;

            if (((t - 0.0) > -EPS) && (t - fDis) < EPS)
            {
                ptInter2.x = ptStart.x + t * d.x;
                ptInter2.y = ptStart.y + t * d.y;
            }

            return true;
        }
    }




    static void Main(string[] args)
    {
        XY ptStart = new XY { x = 3.0f, y = 2.0f };
        XY ptEnd = new XY { x = 10.0f, y = 6.0f };

        XY ptCenter = new XY { x = 10.0f, y = 4.0f };
        float fR = 3.0f;

        XY pt1 = new XY(), pt2 = new XY();

        if (!LineInterCircle(ptStart, ptEnd, ptCenter, fR, ref pt1, ref pt2))
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
