using System;
using System.Collections.Generic;
using System.Text;

namespace xxlib
{
    public struct XY : IBBReader, IBBWriter
    {
        public XY(float x, float y) { this.x = x; this.y = y; }

        public void SetZero()
        {
            x = y = 0;
        }

        public float x, y;
        public static XY operator *(XY a, float b)
        {
            a.x *= b;
            a.y *= b;
            return a;
        }
        public static XY operator +(XY a, XY b)
        {
            a.x += b.x;
            a.y += b.y;
            return a;
        }
        public bool EqualsTo(XY o)
        {
            return x == o.x && y == o.y;
        }

        public void Add(XY other)
        {
            x += other.x;
            y += other.y;
        }
        public void Multiply(float ratio)
        {
            x *= ratio;
            y *= ratio;
        }
        public void Limit(float width, float height)
        {
            if (x < 0) x = 0;
            else if (x >= width - float.Epsilon) x = width - float.Epsilon;
            if (y < 0) y = 0;
            else if (y >= height - float.Epsilon) y = height - float.Epsilon;
        }
        public void Limit(float width, float height, float x, float y)
        {
            width += x;
            if (this.x < x) this.x = x;
            else if (this.x > width) this.x = width;
            height += y;
            if (this.y < y) this.y = y;
            else if (this.y > height) this.y = height;
        }
        public bool Contains(float width, float height)
        {
            return x >= 0 && x <= width && y >= 0 && y <= height;
        }

        public override string ToString()
        {
            return x + ", " + y;
        }

        // todo: 下列函数的各种查表优化

        /// <summary>
        /// 原点 到 a点 的角度( -1 ~ 1 的 float )
        /// </summary>
        public static double Angle(XY a)
        {
            return Math.Atan2(a.y, a.x) / Math.PI;
        }

        /// <summary>
        /// a点 到 b点 的角度( -1 ~ 1 的 float )
        /// </summary>
        public static double Angle(XY a, XY b)
        {
            b.x -= a.x;
            b.y -= a.y;
            return Angle(b);
        }

        /// <summary>
        /// 原点 到 a点 的距离的平方
        /// </summary>
        public static double DistancePow2(XY a)
        {
            return a.x * a.x + a.y * a.y;
        }

        /// <summary>
        /// a点 到 b点 的距离的平方
        /// </summary>
        public static double DistancePow2(XY a, XY b)
        {
            a.x -= b.x;
            a.y -= b.y;
            return DistancePow2(a);
        }

        /// <summary>
        /// 原点 到 a点 的距离
        /// </summary>
        public static double Distance(XY a)
        {
            return Math.Sqrt(DistancePow2(a));
        }

        /// <summary>
        /// a点 到 b点 的距离
        /// </summary>
        public static double Distance(XY a, XY b)
        {
            a.x -= b.x;
            a.y -= b.y;
            return Distance(a);
        }

        /// <summary>
        /// 从 原点 以 angle 角度前进 distance 后的坐标( 通常于角度改变时, 用来算每帧的 xy 增量 )
        /// </summary>
        public static XY Forward(double angle, double distance = 1)
        {
            angle *= Math.PI;
            XY rtv;
            rtv.x = (float)(Math.Cos(angle) * distance);
            rtv.y = (float)(Math.Sin(angle) * distance);
            return rtv;
        }



        // todo: 感觉下面这个函数还有计算 bug

        // http://blog.csdn.net/rabbit729/article/details/4285119
        /// <summary>
        /// 求 线段 pa pb 与 半径r的圆(圆心pc) 的离 pa 最近的边缘重叠点 p1 的距离
        /// 未相交返回 NaN
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

            if ((r2 - e2 + a2) < 0) return float.NaN;

            var f = (float)Math.Sqrt(r2 - e2 + a2);

            return Math.Min(a - f, a + f);
        }


        /// <summary>
        /// 求角度的最小差值 以及如何接近( true: 顺时针 )
        /// </summary>
        public static float AngleMinDiff(float a, float b)
        {
            // a 在 1,2 象限, b 在 3,4 象限. 
            if (a >= 0 && b <= 0)
            {
                // 如果其差值小于 1. 则应该跨越 1 -> -1 , 逆时针转( +2 将 b 转为正数 )
                if (2 + b - a < 1)
                {
                    return 2 + b - a;
                }
                else
                {
                    return a - b;
                }
            }
            // a 在 3,4 象限, b 在 1,2 象限( 同理 )
            else if (a <= 0 && b >= 0)
            {
                if (2 + a - b < 1)
                {
                    return 2 + a - b;
                }
                else
                {
                    return a - b;
                }
            }
            // 同为正或负
            else
            {
                if (a < b)
                {
                    return b - a;
                }
                else
                {
                    return a - b;
                }
            }
        }

        // todo: 补一个按目标角度改 Angle 的函数
        public static void ChangeAngle(ref float a, float b, float inc)
        {
            // a 在 1,2 象限, b 在 3,4 象限. 
            if (a >= 0 && b <= 0)
            {
                // 如果其差值小于 1. 则应该跨越 1 -> -1 , 逆时针转( +2 将 b 转为正数 )
                if (2 + b - a < 1)
                {
                    a += inc;
                    if (a > 1)
                    {
                        a -= 2;
                        if (a > b) a = b;
                    }
                }
                else
                {
                    a -= inc;
                    if (a < b)
                    {
                        a = b;
                    }
                }
            }
            // a 在 3,4 象限, b 在 1,2 象限. 如果其差值小于 1. 则应该跨越 -1 -> 1 , 顺时针转
            else if (a <= 0 && b >= 0)
            {
                if (2 + a - b < 1)
                {
                    a -= inc;
                    if (a < -1)
                    {
                        a += 2;
                        if (a < b) a = b;
                    }
                }
                else
                {
                    a += inc;
                    if (a > b) a = b;
                }
            }
            // 同为正或负
            else
            {
                if (a < b)
                {
                    a += inc;
                    if (a > b) a = b;
                }
                else
                {
                    a -= inc;
                    if (a < b) a = b;
                }
            }

        }


        #region 序列化相关

        public short PackageId { get { throw new NotImplementedException(); } }

        public void ReadFrom(ByteBuffer bb)
        {
            bb.Read(ref x);
            if (float.IsNaN(x) || float.IsInfinity(x)) throw new Exception("the angle can't be NaN.");
            bb.Read(ref y);
            if (float.IsNaN(y) || float.IsInfinity(y)) throw new Exception("the angle can't be NaN.");
        }

        public void WriteTo(ByteBuffer bb)
        {
            bb.Write(x);
            bb.Write(y);
        }

        public string FindDiff(XY o, string rootName = "")
        {
            string rtv = null;
            if (x != o.x) return rootName + " x is diff! this is " + x + ", other is " + o.x;
            if (y != o.y) return rootName + " y is diff! this is " + y + ", other is " + o.y;
            return rtv;
        }

        #endregion
    }
}
