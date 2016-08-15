using System.Collections.Generic;
using xxlib;
using System.Diagnostics;

public class CardinalSplineTo
{
    List<XY> points = new List<XY>();
    float tension;
    float deltaT;
    XY origin;
    XY accumulatedDiff;

    public CardinalSplineTo(List<XY> points, float tension)
    {
        Debug.Assert(points.Count > 1);
        this.points.AddRange(points);
        this.tension = tension;
        deltaT = (float)1 / (points.Count - 1);
    }

    public void StartWithTarget(XY origin)
    {
        this.origin = origin;
        accumulatedDiff.SetZero();
    }

    public XY Calc(float time)
    {
        int p;
        float lt;

        // eg.
        // p..p..p..p..p..p..p
        // 1..2..3..4..5..6..7
        // want p to be 1, 2, 3, 4, 5, 6
        p = (int)(time / deltaT);
        lt = (time - deltaT * (float)p) / deltaT;

        // Interpolate
        var pp0 = points.CircleAt(p - 1);
        var pp1 = points.CircleAt(p + 0);
        var pp2 = points.CircleAt(p + 1);
        var pp3 = points.CircleAt(p + 2);

        return origin + CardinalSplineAt(pp0, pp1, pp2, pp3, tension, lt);
    }

    // CatmullRom Spline formula:
    public static XY CardinalSplineAt(XY p0, XY p1, XY p2, XY p3, float tension, float t)
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

