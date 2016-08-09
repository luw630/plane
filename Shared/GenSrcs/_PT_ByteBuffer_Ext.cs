using System;
using System.Collections.Generic;
using System.Text;
using xxlib;

namespace PT
{
    public static partial class ByteBuffer_Ext
    {
        public static void WriteEnum(this ByteBuffer bb, PT.FortressStates v)
        {
            bb.Write((byte)v);
        }
        public static void WriteEnum(this ByteBuffer bb, PT.FortressStates[] vs)
        {
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Write((byte)vs[i]);
            }
        }
        public static void WriteEnum(this ByteBuffer bb, List<PT.FortressStates> vs)
        {
            bb.WriteLength(vs.Count);
            foreach (var v in vs)
            {
                bb.Write((byte)v);
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref PT.FortressStates v)
        {
            byte tmp = 0;
            bb.Read(ref tmp);
            v = (PT.FortressStates)tmp;
        }
        public static void ReadEnum(this ByteBuffer bb, ref PT.FortressStates[] vs)
        {
            byte tmp = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Read(ref tmp);
                vs[i] = (PT.FortressStates)tmp;
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<PT.FortressStates> vs)
        {
            ReadEnum(bb, ref vs, 0, 0);
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<PT.FortressStates> vs, int minLen, int maxLen)
        {
            int len = bb.ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                byte tmp = 0;
                bb.Read(ref tmp);
                vs.Add((PT.FortressStates)tmp);
            }
        }

        public static void WriteEnum(this ByteBuffer bb, PT.Camps v)
        {
            bb.Write((byte)v);
        }
        public static void WriteEnum(this ByteBuffer bb, PT.Camps[] vs)
        {
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Write((byte)vs[i]);
            }
        }
        public static void WriteEnum(this ByteBuffer bb, List<PT.Camps> vs)
        {
            bb.WriteLength(vs.Count);
            foreach (var v in vs)
            {
                bb.Write((byte)v);
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref PT.Camps v)
        {
            byte tmp = 0;
            bb.Read(ref tmp);
            v = (PT.Camps)tmp;
        }
        public static void ReadEnum(this ByteBuffer bb, ref PT.Camps[] vs)
        {
            byte tmp = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Read(ref tmp);
                vs[i] = (PT.Camps)tmp;
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<PT.Camps> vs)
        {
            ReadEnum(bb, ref vs, 0, 0);
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<PT.Camps> vs, int minLen, int maxLen)
        {
            int len = bb.ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                byte tmp = 0;
                bb.Read(ref tmp);
                vs.Add((PT.Camps)tmp);
            }
        }

    }
}
