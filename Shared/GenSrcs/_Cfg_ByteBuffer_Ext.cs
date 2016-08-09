using System;
using System.Collections.Generic;
using System.Text;
using xxlib;

namespace Cfg
{
    public static partial class ByteBuffer_Ext
    {
        public static void WriteEnum(this ByteBuffer bb, Cfg.PropertyTypes v)
        {
            bb.Write((byte)v);
        }
        public static void WriteEnum(this ByteBuffer bb, Cfg.PropertyTypes[] vs)
        {
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Write((byte)vs[i]);
            }
        }
        public static void WriteEnum(this ByteBuffer bb, List<Cfg.PropertyTypes> vs)
        {
            bb.WriteLength(vs.Count);
            foreach (var v in vs)
            {
                bb.Write((byte)v);
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref Cfg.PropertyTypes v)
        {
            byte tmp = 0;
            bb.Read(ref tmp);
            v = (Cfg.PropertyTypes)tmp;
        }
        public static void ReadEnum(this ByteBuffer bb, ref Cfg.PropertyTypes[] vs)
        {
            byte tmp = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Read(ref tmp);
                vs[i] = (Cfg.PropertyTypes)tmp;
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<Cfg.PropertyTypes> vs)
        {
            ReadEnum(bb, ref vs, 0, 0);
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<Cfg.PropertyTypes> vs, int minLen, int maxLen)
        {
            int len = bb.ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                byte tmp = 0;
                bb.Read(ref tmp);
                vs.Add((Cfg.PropertyTypes)tmp);
            }
        }

        public static void WriteEnum(this ByteBuffer bb, Cfg.AtkType v)
        {
            bb.Write((byte)v);
        }
        public static void WriteEnum(this ByteBuffer bb, Cfg.AtkType[] vs)
        {
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Write((byte)vs[i]);
            }
        }
        public static void WriteEnum(this ByteBuffer bb, List<Cfg.AtkType> vs)
        {
            bb.WriteLength(vs.Count);
            foreach (var v in vs)
            {
                bb.Write((byte)v);
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref Cfg.AtkType v)
        {
            byte tmp = 0;
            bb.Read(ref tmp);
            v = (Cfg.AtkType)tmp;
        }
        public static void ReadEnum(this ByteBuffer bb, ref Cfg.AtkType[] vs)
        {
            byte tmp = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Read(ref tmp);
                vs[i] = (Cfg.AtkType)tmp;
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<Cfg.AtkType> vs)
        {
            ReadEnum(bb, ref vs, 0, 0);
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<Cfg.AtkType> vs, int minLen, int maxLen)
        {
            int len = bb.ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                byte tmp = 0;
                bb.Read(ref tmp);
                vs.Add((Cfg.AtkType)tmp);
            }
        }

    }
}
