using System;
using System.Collections.Generic;
using System.Text;

namespace xxlib
{
    /// <summary>
    /// interface 在本代码文件的尾部
    /// 为简化，不支持“空”，“指针”的概念（ Write 时传入 null 是不被支持的, 要自己判断 )
    /// 基本上这个类和 xxlib.ByteBuffer 的设计，实现原理，使用方式几乎一样
    /// 区别在于， c# 版 Read 需要 靠 try catch 来捕获错误( 长度超 Limit 也是 OverflowException )
    /// 读写分作两种：定长( memcpy ), 变长( 7bit ), 视需求选用( 模板里面可以用相应的 Attribute 设置具体的 field 使用变长存储 )。
    /// 可变长类型：int, long, double
    /// 支持关键字级别基础数据类型及其数组( 算定长，不会记录长度 ),
    /// 支持 DateTime, string( 会记录长度 ), ByteBuffer( 会记录长度 ), List( 会记录长度 )
    /// 支持 bool 数组或 List 的位压缩存储
    /// 支持 List 最多嵌套 2 层
    /// 支持 非数组容器还原时限长
    /// </summary>
    public partial class ByteBuffer
    {
        public byte[] buf;
        public int offset;
        public int dataLen;

        #region constructors

        public ByteBuffer(int capacity)
        {
            Assign(new byte[Round2n((uint)capacity)], 0);
        }

        public ByteBuffer() : this(1024)
        {
        }

        public ByteBuffer(byte[] buf_, int dataLen_ = 0)
        {
            Assign(buf_, dataLen_);
        }

        public ByteBuffer(IBBWriter o) : this()
        {
            Write(o);
        }

        public static implicit operator ByteBuffer(byte[] buf)
        {
            return new ByteBuffer(buf, buf.Length);
        }


        #endregion

        #region static make funcs

        public static byte[] MakePackageData(IBBWriter o)
        {
            var bb = new ByteBuffer();
            bb.Write(o.PackageId);
            bb.Write(o);
            return bb.DumpData();
        }
        public static byte[] MakePackageData(ByteBuffer bb, IBBWriter o)
        {
            bb.Clear();
            bb.Write(o.PackageId);
            bb.Write(o);
            return bb.DumpData();
        }

        public static ByteBuffer Make(byte o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }

        public static ByteBuffer Make(sbyte o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(ushort o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(short o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(uint o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(int o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(ulong o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(long o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(float o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(double o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(bool o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(string o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(DateTime o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(ByteBuffer o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer Make(ByteBuffer v, int offset, int len)
        {
            var rtv = new ByteBuffer();
            rtv.Write(v, offset, len);
            return rtv;
        }

        public static ByteBuffer Make<T>(T v) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            v.WriteTo(rtv);
            return rtv;
        }

        // 数组都算作定长, 不写长度值
        public static ByteBuffer Make<T>(T[] vs) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }


        public static ByteBuffer Make(sbyte[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(short[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(ushort[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(int[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(uint[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(long[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(ulong[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(float[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(double[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(string[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(DateTime[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(ByteBuffer[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }

        public static ByteBuffer Make(byte[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(byte[] v, int offset, int len)
        {
            var rtv = new ByteBuffer();
            rtv.Write(v, offset, len);
            return rtv;
        }
        public static ByteBuffer Make(bool[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make<T>(List<T> vs) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<byte> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<sbyte> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<short> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<ushort> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<int> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<uint> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<long> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<ulong> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<float> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<double> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<string> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<DateTime> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<ByteBuffer> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<bool> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make<T>(List<List<T>> vs) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<byte>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<sbyte>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<short>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<ushort>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<int>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<uint>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<long>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<ulong>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<float>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<double>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<bool>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<string>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<DateTime>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer Make(List<List<ByteBuffer>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }





        public static ByteBuffer VarMake(uint v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarMake(int v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarMake(ulong v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarMake(long v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarMake(double v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }



        #endregion

        #region misc funcs

        public void Assign(byte[] buf_, int dataLen_)
        {
            offset = 0;
            buf = buf_;
            dataLen = dataLen_;
        }

        public void Assign(byte[] buf_)
        {
            Assign(buf_, 0);
        }

        public void Clear()
        {
            dataLen = offset = 0;
        }

        public bool Empty()
        {
            return dataLen == 0;
        }

        public bool Full()
        {
            return dataLen == offset;
        }

        public byte[] Collapse()
        {
            Array.Resize(ref buf, dataLen);
            return buf;
        }

        public byte[] DumpData()
        {
            var rtv = new byte[dataLen];
            Array.Copy(buf, rtv, dataLen);
            return rtv;
        }

        public void Reserve(int capacity)
        {
            if (capacity <= buf.Length)
                return;
            ReserveCore(capacity);
        }

        public byte At(int idx)
        {
            return buf[idx];
        }

        #endregion

        #region dump / tostring / findDiff func

        public void Dump(ref StringBuilder s)
        {
            Dump(ref s, buf, dataLen);
        }

        public void Dump()
        {
            var s = new StringBuilder();
            Dump(ref s);
            Console.Write(s);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            Dump(ref s);
            return s.ToString();
        }

        public string FindDiff(ByteBuffer o, string rootName = "")
        {
            string rtv = null;
            if (dataLen != o.dataLen) return rootName + " dataLen is diff! this is " + dataLen + ", other is " + o.dataLen;
            for (int i = 0; i < dataLen; i++)
            {
                if (buf[i] != o.buf[i]) return rootName + " buf is diff! idx is " + i;
            }
            return rtv;
        }

        #endregion

        #region write package funcs & read package id funcs

        public void WritePackage(IBBWriter o)
        {
            Write(o.PackageId);
            Write(o);
        }

        public short ReadPackageId()
        {
            short rtv = 0;
            Read(ref rtv);
            return rtv;
        }

        #endregion

        #region write funcs

        public void Write(byte v)
        {
            if (dataLen + 1 > buf.Length)
                ReserveCore(dataLen + 1);
            buf[dataLen] = v;
            dataLen++;
        }
        public void Write(sbyte v)
        {
            Write((byte)v);
        }
        public void Write(ushort v)
        {
            if (dataLen + 2 > buf.Length)
                ReserveCore(dataLen + 2);
            buf[dataLen] = (byte)(v & 0xff);
            buf[dataLen + 1] = (byte)((v & 0xff00) >> 8);
            dataLen += 2;
        }
        public void Write(short v)
        {
            Write((ushort)v);
        }
        public void Write(uint v)
        {
            if (dataLen + 4 > buf.Length)
                ReserveCore(dataLen + 4);
            buf[dataLen] = (byte)(v & 0xff);
            buf[dataLen + 1] = (byte)((v & 0xff00) >> 8);
            buf[dataLen + 2] = (byte)((v & 0xff0000) >> 16);
            buf[dataLen + 3] = (byte)((v & 0xff000000) >> 24);
            dataLen += 4;
        }
        public void Write(int v)
        {
            Write((uint)v);
        }
        public void Write(ulong v)
        {
            if (dataLen + 8 > buf.Length)
                ReserveCore(dataLen + 8);
            buf[dataLen + 0] = (byte)v;
            buf[dataLen + 1] = (byte)(v >> 8);
            buf[dataLen + 2] = (byte)(v >> 16);
            buf[dataLen + 3] = (byte)(v >> 24);
            buf[dataLen + 4] = (byte)(v >> 32);
            buf[dataLen + 5] = (byte)(v >> 40);
            buf[dataLen + 6] = (byte)(v >> 48);
            buf[dataLen + 7] = (byte)(v >> 56);
            dataLen += 8;
        }
        public void Write(long v)
        {
            Write((ulong)v);
        }

        public void Write(float v)
        {
            var tmpbuf = BitConverter.GetBytes(v);
            if (dataLen + 4 > buf.Length)
                ReserveCore(dataLen + 4);
            Array.Copy(tmpbuf, 0, buf, dataLen, 4);
            dataLen += 4;
        }
        public void Write(double v)
        {
            var tmpbuf = BitConverter.GetBytes(v);
            if (dataLen + 8 > buf.Length)
                ReserveCore(dataLen + 8);
            Array.Copy(tmpbuf, 0, buf, dataLen, 8);
            dataLen += 8;
        }
        public void Write(bool v)
        {
            Write(v ? (byte)1 : (byte)0);
        }
        public void Write(string v)
        {
            if (v == null)
            {
                v = "";
            }
            var sbuf = Encoding.UTF8.GetBytes(v);
            int sbufLen = sbuf.Length;
            WriteLength(sbufLen);
            if (v == null)
            {
                return;
            }
            if (dataLen + sbufLen > buf.Length)
                ReserveCore(dataLen + sbufLen);
            Array.Copy(sbuf, 0, buf, dataLen, sbufLen);
            dataLen += sbufLen;
        }
        public void Write(DateTime v)
        {
            if (dataLen + 8 > buf.Length)
                ReserveCore(dataLen + 8);
            short year = (short)v.Year;
            buf[dataLen + 0] = (byte)year;
            buf[dataLen + 1] = (byte)(year >> 8);
            buf[dataLen + 2] = (byte)v.Month;
            buf[dataLen + 3] = (byte)v.Day;
            buf[dataLen + 4] = (byte)v.Hour;
            buf[dataLen + 5] = (byte)v.Minute;
            buf[dataLen + 6] = (byte)v.Second;
            buf[dataLen + 7] = (byte)0;
            dataLen += 8;
        }
        public void Write(ByteBuffer v)
        {
            WriteLength(v.dataLen);
            Write(v.buf, 0, v.dataLen);
        }
        public void Write(ByteBuffer v, int offset, int len)
        {
            WriteLength(len);
            Write(v.buf, offset, len);
        }

        public void Write<T>(T v) where T : IBBWriter
        {
            v.WriteTo(this);
        }


        // 数组都算作定长, 不写长度值
        public void Write<T>(T[] vs) where T : IBBWriter
        {
            foreach (var v in vs)
            {
                Write(v);
            }
        }

        public void Write(sbyte[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(short[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(ushort[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(int[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(uint[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(long[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(ulong[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(float[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(double[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(string[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(DateTime[] vs) { foreach (var v in vs) { Write(v); } }
        public void Write(ByteBuffer[] vs) { foreach (var v in vs) { Write(v); } }

        public void Write(byte[] v)
        {
            var vlen = v.Length;
            if (dataLen + vlen > buf.Length)
                ReserveCore(dataLen + vlen);
            Reserve(dataLen + vlen);
            Array.Copy(v, 0, buf, dataLen, vlen);
            dataLen += vlen;
        }
        public void Write(byte[] v, int offset, int len)
        {
            if (dataLen + len > buf.Length)
                ReserveCore(dataLen + len);
            Array.Copy(v, offset, buf, dataLen, len);
            dataLen += len;
        }
        public void Write(bool[] v)
        {
            var bits = v.Length;
            var bytes = (bits + 7) >> 3;
            if (dataLen + bytes > buf.Length)
                ReserveCore(dataLen + bytes);
            var mod = bits % 8;
            if (mod > 0)
                --bytes;
            byte n = 0;
            for (var i = 0; i < bytes; ++i)
            {
                var p = i << 3;
                if (v[p + 0]) n |= 1;
                if (v[p + 1]) n |= 2;
                if (v[p + 2]) n |= 4;
                if (v[p + 3]) n |= 8;
                if (v[p + 4]) n |= 16;
                if (v[p + 5]) n |= 32;
                if (v[p + 6]) n |= 64;
                if (v[p + 7]) n |= 128;
                buf[dataLen++] = n;
            }
            if (mod > 0)
            {
                n = 0;
                var offset = bytes << 3;
                for (var i = 0; i < mod; ++i)
                {
                    if (v[offset + i]) n |= (byte)(1 << i);
                }
                buf[dataLen++] = n;
            }
        }





        public void Write<T>(List<T> vs) where T : IBBWriter
        {
            WriteLength(vs.Count);
            foreach (var v in vs)
            {
                Write(v);
            }
        }

        public void Write(List<byte> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<sbyte> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<short> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<ushort> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<int> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<uint> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<long> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<ulong> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<float> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<double> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<string> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<DateTime> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<ByteBuffer> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }



        public void Write(List<bool> vs)
        {
            WriteLength(vs.Count);
            var bits = vs.Count;
            var bytes = (bits + 7) >> 3;
            if (dataLen + bytes > buf.Length)
                ReserveCore(dataLen + bytes);
            var mod = bits % 8;
            if (mod > 0)
                --bytes;
            byte n = 0;
            for (var i = 0; i < bytes; ++i)
            {
                var p = i << 3;
                if (vs[p + 0])
                    n |= 1;
                if (vs[p + 1])
                    n |= 2;
                if (vs[p + 2])
                    n |= 4;
                if (vs[p + 3])
                    n |= 8;
                if (vs[p + 4])
                    n |= 16;
                if (vs[p + 5])
                    n |= 32;
                if (vs[p + 6])
                    n |= 64;
                if (vs[p + 7])
                    n |= 128;
                buf[dataLen++] = n;
            }
            if (mod > 0)
            {
                n = 0;
                var offset = bytes << 3;
                for (var i = 0; i < mod; ++i)
                {
                    if (vs[offset + i])
                        n |= (byte)(1 << i);
                }
                buf[dataLen++] = n;
            }
        }




        public void Write<T>(List<List<T>> vs) where T : IBBWriter
        {
            WriteLength(vs.Count);
            foreach (var v in vs)
            {
                Write(v);
            }
        }

        public void Write(List<List<byte>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<sbyte>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<short>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<ushort>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<int>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<uint>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<long>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<ulong>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<float>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<double>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<bool>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<string>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<DateTime>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }
        public void Write(List<List<ByteBuffer>> vs) { WriteLength(vs.Count); foreach (var v in vs) { Write(v); } }

        #endregion

        #region read funcs

        public void Read(ref byte v)
        {
            v = buf[offset++];
        }
        public void Read(ref sbyte v)
        {
            v = (sbyte)buf[offset++];
        }
        public void Read(ref ushort v)
        {
            v = (ushort)((0xff & buf[offset])
                | (0xff00 & (buf[offset + 1] << 8)));
            offset += 2;
        }
        public void Read(ref short v)
        {
            v = (short)((0xff & buf[offset])
                | (0xff00 & (buf[offset + 1] << 8)));
            offset += 2;
        }
        public void Read(ref uint v)
        {
            v = (uint)(buf[offset]
                | (buf[offset + 1] << 8)
                | (buf[offset + 2] << 16)
                | (buf[offset + 3] << 24));
            offset += 4;
        }
        public void Read(ref int v)
        {
            v = buf[offset]
                | (buf[offset + 1] << 8)
                | (buf[offset + 2] << 16)
                | (buf[offset + 3] << 24);
            offset += 4;
        }
        public void Read(ref ulong v)
        {
            v = ((ulong)buf[offset])
                | (((ulong)buf[offset + 1] << 8))
                | (((ulong)buf[offset + 2] << 16))
                | (((ulong)buf[offset + 3] << 24))
                | (((ulong)buf[offset + 4] << 32))
                | (((ulong)buf[offset + 5] << 40))
                | (((ulong)buf[offset + 6] << 48))
                | (((ulong)buf[offset + 7] << 56));
            offset += 8;
        }
        public void Read(ref long v)
        {
            ulong tmp = 0;
            Read(ref tmp);
            v = (long)tmp;
        }

        public void Read(ref float v)
        {
            v = BitConverter.ToSingle(buf, offset);
            offset += 4;
        }
        public void Read(ref double v)
        {
            v = BitConverter.ToDouble(buf, offset);
            offset += 8;
        }
        public void Read(ref bool v)
        {
            v = buf[offset++] == 1;
        }
        public void Read(ref string v)
        {
            Read(ref v, 0, 0);
        }
        public void Read(ref string v, int minLen, int maxLen)
        {
            int len = ReadLength(minLen, maxLen);
            v = Encoding.UTF8.GetString(buf, offset, len);
            offset += len;
        }
        public void Read(ref ByteBuffer v)
        {
            v.dataLen = ReadLength(0, 0);
            Array.Resize(ref v.buf, v.dataLen);
            Read(ref v.buf);
        }
        public void Read(ref ByteBuffer v, int minLen, int maxLen)
        {
            v.dataLen = ReadLength(minLen, maxLen);
            Array.Resize(ref v.buf, v.dataLen);
            Read(ref v.buf);
        }
        public void Read(ref DateTime v)
        {
            short years = 0;
            Read(ref years);
            v = new DateTime(years,
                (int)buf[offset],
                (int)buf[offset + 1],
                (int)buf[offset + 2],
                (int)buf[offset + 3],
                (int)buf[offset + 4]);
            offset += 6;    // Read short already += 2
        }

        public void Read<T>(ref T v) where T : IBBReader
        {
            v.ReadFrom(this);
        }

        // 数组都算作定长, 不读长度值
        public void Read<T>(ref T[] vs) where T : IBBReader
        {
            for (int i = 0; i < vs.Length; ++i)
            {
                Read(ref vs[i]);
            }
        }

        public void Read(ref sbyte[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref short[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref ushort[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref int[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref uint[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref long[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref ulong[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref float[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref string[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref DateTime[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }
        public void Read(ref ByteBuffer[] vs) { for (int i = 0; i < vs.Length; ++i) { Read(ref vs[i]); } }


        public void Read(ref byte[] v)
        {
            Array.Copy(buf, offset, v, 0, v.Length);
            offset += v.Length;
        }
        public void Read(ref bool[] vs)
        {
            var bytes = (vs.Length + 7) >> 3;
            var p = offset;
            offset += bytes;
            var mod = vs.Length % 8;
            if (mod > 0)
                --bytes;
            int idx = 0;
            for (var i = 0; i < bytes; ++i)
            {
                var n = buf[p + i];
                vs[idx + 0] = ((n & 1) > 0);
                vs[idx + 1] = ((n & 2) > 0);
                vs[idx + 2] = ((n & 4) > 0);
                vs[idx + 3] = ((n & 8) > 0);
                vs[idx + 4] = ((n & 16) > 0);
                vs[idx + 5] = ((n & 32) > 0);
                vs[idx + 6] = ((n & 64) > 0);
                vs[idx + 7] = ((n & 128) > 0);
                idx += 8;
            }
            if (mod > 0)
            {
                var n = buf[p + bytes];
                for (var i = 0; i < mod; ++i)
                {
                    vs[idx++] = ((n & (1 << i)) > 0);
                }
            }
        }


        public void Read<T>(ref List<T> vs) where T : IBBReader, new()
        {
            Read(ref vs, 0, 0);
        }
        public void Read<T>(ref List<T> vs, int minLen, int maxLen) where T : IBBReader, new()
        {
            int len = ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                vs.Add(Read<T>());
            }
        }
        public T Read<T>() where T : IBBReader, new()
        {
            var rtv = new T();
            rtv.ReadFrom(this);
            return rtv;
        }

        public void Read(ref List<byte> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<sbyte> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<short> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<ushort> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<int> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<uint> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<long> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<ulong> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<float> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<double> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<string> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<DateTime> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<ByteBuffer> vs) { Read(ref vs, 0, 0); }

        public void Read(ref List<byte> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { byte s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<sbyte> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { sbyte s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<short> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { short s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<ushort> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { ushort s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<int> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { int s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<uint> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { uint s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<long> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { long s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<ulong> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { ulong s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<float> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { float s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<double> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { double s = 0; Read(ref s); vs.Add(s); } }
        public void Read(ref List<string> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { string s = ""; Read(ref s); vs.Add(s); } }
        public void Read(ref List<DateTime> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { DateTime s = new DateTime(); Read(ref s); vs.Add(s); } }
        public void Read(ref List<ByteBuffer> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { ByteBuffer s = new ByteBuffer(); Read(ref s); vs.Add(s); } }

        public void Read(ref List<bool> vs)
        {
            Read(ref vs, 0, 0);
        }
        public void Read(ref List<bool> vs, int minLen, int maxLen)
        {
            int len = ReadLength(minLen, maxLen);
            vs.Clear();
            if (len == 0)
                return;

            var bytes = (len + 7) >> 3;
            var p = offset;
            offset += bytes;
            var mod = len % 8;
            if (mod > 0)
                --bytes;
            for (var i = 0; i < bytes; ++i)
            {
                var n = buf[p + i];
                vs.Add((n & 1) > 0);
                vs.Add((n & 2) > 0);
                vs.Add((n & 4) > 0);
                vs.Add((n & 8) > 0);
                vs.Add((n & 16) > 0);
                vs.Add((n & 32) > 0);
                vs.Add((n & 64) > 0);
                vs.Add((n & 128) > 0);
            }
            if (mod > 0)
            {
                var n = buf[p + bytes];
                for (var i = 0; i < mod; ++i)
                {
                    vs.Add((n & (1 << i)) > 0);
                }
            }
        }






        public void Read<T>(ref List<List<T>> vs) where T : IBBReader, new()
        {
            Read(ref vs, 0, 0);
        }

        public void Read<T>(ref List<List<T>> vs, int minLen, int maxLen) where T : IBBReader, new()
        {
            int len = ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                var o = new List<T>();
                Read(ref o);
                vs.Add(o);
            }
        }


        public void Read(ref List<List<byte>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<sbyte>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<short>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<ushort>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<int>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<uint>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<long>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<ulong>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<float>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<double>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<bool>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<string>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<DateTime>> vs) { Read(ref vs, 0, 0); }
        public void Read(ref List<List<ByteBuffer>> vs) { Read(ref vs, 0, 0); }

        public void Read(ref List<List<byte>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<byte>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<sbyte>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<sbyte>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<short>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<short>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<ushort>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<ushort>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<int>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<int>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<uint>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<uint>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<long>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<long>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<ulong>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<ulong>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<float>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<float>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<double>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<double>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<bool>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<bool>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<string>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<string>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<DateTime>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<DateTime>(); Read(ref o, 0, 0); vs.Add(o); } }
        public void Read(ref List<List<ByteBuffer>> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { var o = new List<ByteBuffer>(); Read(ref o, 0, 0); vs.Add(o); } }


        #endregion

        #region var write funcs

        public void VarWrite(uint v)
        {
            if (dataLen + 5 > buf.Length)
                ReserveCore(dataLen + 5);
            Bit7Write32(buf, ref dataLen, v);
        }
        public void VarWrite(int v)
        {
            if (dataLen + 5 > buf.Length)
                ReserveCore(dataLen + 5);
            Bit7Write32(buf, ref dataLen, ZigZagEncode32(v));
        }
        public void VarWrite(ulong v)
        {
            if (dataLen + 9 > buf.Length)
                ReserveCore(dataLen + 9);
            Bit7Write64(buf, ref dataLen, v);
        }
        public void VarWrite(long v)
        {
            if (dataLen + 9 > buf.Length)
                ReserveCore(dataLen + 9);
            Bit7Write64(buf, ref dataLen, ZigZagEncode64(v));
        }
        public void VarWrite(double v)
        {
            if (dataLen + 9 > buf.Length)
                ReserveCore(dataLen + 9);
            if (v == 0)
            {
                buf[dataLen++] = 0;
            }
            else
            {
                var intv = (int)v;
                if (v == (double)intv)
                {
                    buf[dataLen++] = 1;
                    VarWrite(intv);
                }
                else
                {
                    buf[dataLen++] = 2;
                    Write(v);
                }
                // todo: inf? nan?
            }
        }



        public void VarWrite(List<int> vs) { WriteLength(vs.Count); foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(List<uint> vs) { WriteLength(vs.Count); foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(List<long> vs) { WriteLength(vs.Count); foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(List<ulong> vs) { WriteLength(vs.Count); foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(List<double> vs) { WriteLength(vs.Count); foreach (var v in vs) { VarWrite(v); } }




        public void VarWrite(int[] vs) { foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(uint[] vs) { foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(long[] vs) { foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(ulong[] vs) { foreach (var v in vs) { VarWrite(v); } }
        public void VarWrite(double[] vs) { foreach (var v in vs) { VarWrite(v); } }





        #endregion

        #region var read funcs

        public void VarRead(ref uint v)
        {
            Bit7Read32(ref v, buf, ref offset, dataLen);
        }
        public void VarRead(ref int v)
        {
            uint tmp = 0;
            Bit7Read32(ref tmp, buf, ref offset, dataLen);
            v = ZigZagDecode32(tmp);
        }
        public void VarRead(ref ulong v)
        {
            Bit7Read64(ref v, buf, ref offset, dataLen);
        }
        public void VarRead(ref long v)
        {
            ulong tmp = 0;
            Bit7Read64(ref tmp, buf, ref offset, dataLen);
            v = ZigZagDecode64(tmp);
        }
        public void VarRead(ref double v)
        {
            switch (buf[offset++])
            {
                case 0:
                    v = 0;
                    break;
                case 1:
                    int rtv = 0;
                    VarRead(ref rtv);
                    v = rtv;
                    break;
                case 2:
                    Read(ref v);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }


        public void VarRead(ref List<int> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { int s = 0; VarRead(ref s); vs.Add(s); } }
        public void VarRead(ref List<uint> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { uint s = 0; VarRead(ref s); vs.Add(s); } }
        public void VarRead(ref List<long> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { long s = 0; VarRead(ref s); vs.Add(s); } }
        public void VarRead(ref List<ulong> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { ulong s = 0; VarRead(ref s); vs.Add(s); } }
        public void VarRead(ref List<double> vs, int minLen, int maxLen) { int len = ReadLength(minLen, maxLen); vs.Clear(); for (int i = 0; i < len; i++) { double s = 0; VarRead(ref s); vs.Add(s); } }




        public void VarRead(ref int[] vs) { for (int i = 0; i < vs.Length; i++) { int s = 0; VarRead(ref s); vs[i] = s; } }
        public void VarRead(ref uint[] vs) { for (int i = 0; i < vs.Length; i++) { uint s = 0; VarRead(ref s); vs[i] = s; } }
        public void VarRead(ref long[] vs) { for (int i = 0; i < vs.Length; i++) { long s = 0; VarRead(ref s); vs[i] = s; } }
        public void VarRead(ref ulong[] vs) { for (int i = 0; i < vs.Length; i++) { ulong s = 0; VarRead(ref s); vs[i] = s; } }
        public void VarRead(ref double[] vs) { for (int i = 0; i < vs.Length; i++) { double s = 0; VarRead(ref s); vs[i] = s; } }




        #endregion

        #region binary helper funcs

        /// <summary>
        /// 统计有多少个 1
        /// </summary>
        protected static int Popcnt(uint x)
        {
            x -= ((x >> 1) & 0x55555555);
            x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
            x = (((x >> 4) + x) & 0x0f0f0f0f);
            x += (x >> 8);
            x += (x >> 16);
            return (int)(x & 0x0000003f);
        }

        /// <summary>
        /// 统计高位有多少个 0
        /// </summary>
        protected static int Clz(uint x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (int)(32 - Popcnt(x));
        }

        /// <summary>
        /// 求大于 v 的 2^n 值
        /// </summary>
        protected static uint Round2n(uint v)
        {
            int bits = 31 - Clz(v);
            var rtv = (uint)(1u << bits);
            if (rtv == v)
                return v;
            return rtv << 1;
        }

        // 负转正：利用单数来存负数，双数来存正数
        // 等效代码： if( v < 0 ) return -v * 2 - 1; else return v * 2;
        protected static ushort ZigZagEncode16(short v) { return (ushort)((v << 1) ^ (v >> 15)); }

        protected static uint ZigZagEncode32(int v) { return (uint)((v << 1) ^ (v >> 31)); }

        protected static ulong ZigZagEncode64(long v) { return (ulong)((v << 1) ^ (v >> 63)); }

        // 等效代码： if( (v & 1) > 0 ) return -(v + 1) / 2; else return v / 2;
        protected static short ZigZagDecode16(ushort v) { return (short)((short)(v >> 1) ^ (-(short)(v & 1))); }

        protected static int ZigZagDecode32(uint v) { return (int)(v >> 1) ^ (-(int)(v & 1)); }

        protected static long ZigZagDecode64(ulong v) { return (long)(v >> 1) ^ (-(long)(v & 1)); }

        // need ensure 5
        protected static void Bit7Write32(byte[] buf, ref int offset, uint v)
        {
            Lab1:
            byte b7 = (byte)v;
            v >>= 7;
            if (v > 0)
            {
                buf[offset++] = (byte)(b7 | (byte)0x80);
                goto Lab1;
            }
            buf[offset++] = b7;
        }

        // need ensure 9
        protected static void Bit7Write64(byte[] buf, ref int offset, ulong v)
        {
            var idx8 = offset + 8;
            Lab1:
            byte b7 = (byte)v;
            v >>= 7;
            if (v > 0)
            {
                buf[offset++] = (byte)(b7 | (byte)0x80);
                if (offset == idx8)
                    buf[offset++] = (byte)v;
                else
                    goto Lab1;
            }
            else
            {
                buf[offset++] = b7;
            }
        }


        protected static void Bit7Read64(ref ulong v, byte[] buf, ref int offset, int dataLen)
        {
            var idx8 = offset + 8;
            v = 0;
            int lshift = 0;
            Lab1:
            ulong b7 = buf[offset++];
            if (b7 > 0x7F)
            {
                if (offset < idx8)
                {
                    v |= (b7 & 0x7F) << lshift;
                    lshift += 7;
                    goto Lab1;
                }
                else
                    v |= ((b7 & 0x7F) << lshift) | ((ulong)buf[offset++] << 28 << 28);
            }
            else
                v |= b7 << lshift;
        }

        protected static void Bit7Read32(ref uint v, byte[] buf, ref int offset, int dataLen)
        {
            var idx5 = offset + 5;
            int lshift = 0;
            v = 0;
            Lab1:
            uint b7 = buf[offset++];
            if (b7 > 0x7F)
            {
                if (offset == idx5)
                    throw new OverflowException();
                v |= (b7 & 0x7F) << lshift;
                lshift += 7;
                goto Lab1;
            }
            else
            {
                if (offset == idx5 && b7 > 15)
                    throw new OverflowException();
                else
                    v |= b7 << lshift;
            }
        }


        protected static void DumpAscII(ref StringBuilder s, byte[] buf, int offset, int len)
        {
            for (int i = offset; i < offset + len; ++i)
            {
                byte c = buf[i];
                if (c < 32 || c > 126)
                    s.Append('.');
                else
                    s.Append((char)c);
            }
        }

        // write buf's binary dump text to s
        protected static void Dump(ref StringBuilder s, byte[] buf, int len = 0)
        {
            if (buf == null || buf.Length == 0)
                return;
            if (len == 0)
                len = buf.Length;
            s.Append("--------  0  1  2  3 | 4  5  6  7 | 8  9  A  B | C  D  E  F");
            s.Append("   dataLen = " + len);
            int i = 0;
            for (; i < len; ++i)
            {
                if ((i % 16) == 0)
                {
                    if (i > 0)
                    {           // output ascii to the end of the line
                        s.Append("  ");
                        DumpAscII(ref s, buf, i - 16, 16);
                    }
                    s.Append('\n');
                    s.Append(i.ToString("x8"));
                    s.Append("  ");
                }
                else if ((i > 0 && (i % 4 == 0)))
                {
                    s.Append("  ");
                }
                else
                    s.Append(' ');
                s.Append(BitConverter.ToString(buf, i, 1));
            }
            int left = i % 16;
            if (left == 0)
            {
                left = 16;
            }
            if (left > 0)
            {
                len = len + 16 - left;
                for (; i < len; ++i)
                {
                    if (i > 0 && (i % 4 == 0))
                        s.Append("  ");
                    else
                        s.Append(' ');
                    s.Append("  ");
                }
                s.Append("  ");
                DumpAscII(ref s, buf, i - 16, left);
            }
            s.Append('\n');
        }

        protected static string Dump(byte[] buf, int len = 0)
        {
            var sb = new StringBuilder();
            Dump(ref sb, buf, len);
            return sb.ToString();
        }

        #endregion

        #region protected funcs

        protected void ReserveCore(int capacity)
        {
            Array.Resize<byte>(ref buf, (int)Round2n((uint)(capacity * 2)));
        }

        public int ReadLength(int minLen, int maxLen)
        {
            uint len = 0;
            Bit7Read32(ref len, buf, ref offset, dataLen);
            if (len < minLen || (maxLen > 0 && len > maxLen))
                throw new OverflowException();
            return (int)len;
        }

        public void WriteLength(int v)
        {
            VarWrite((uint)v);
        }

        #endregion
    }

    public static partial class ByteBuffer_Ext_Internal
    {
        public static ByteBuffer ToByteBuffer(this byte o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }

        public static ByteBuffer ToByteBuffer(this sbyte o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ushort o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this short o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this uint o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this int o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ulong o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this long o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this float o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this double o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this bool o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this string o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this DateTime o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ByteBuffer o)
        {
            var rtv = new ByteBuffer();
            rtv.Write(o);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ByteBuffer v, int offset, int len)
        {
            var rtv = new ByteBuffer();
            rtv.Write(v, offset, len);
            return rtv;
        }

        public static ByteBuffer ToByteBuffer<T>(this T v) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            v.WriteTo(rtv);
            return rtv;
        }

        // 数组都算作定长, 不写长度值
        public static ByteBuffer ToByteBuffer<T>(this T[] vs) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }

        public static ByteBuffer ToByteBuffer(this sbyte[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this short[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ushort[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this int[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this uint[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this long[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ulong[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this float[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this double[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this string[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this DateTime[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this ByteBuffer[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }

        public static ByteBuffer ToByteBuffer(this byte[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this byte[] v, int offset, int len)
        {
            var rtv = new ByteBuffer();
            rtv.Write(v, offset, len);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this bool[] vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer<T>(this List<T> vs) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<byte> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<sbyte> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<short> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<ushort> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<int> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<uint> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<long> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<ulong> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<float> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<double> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<string> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<DateTime> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<ByteBuffer> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<bool> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer<T>(this List<List<T>> vs) where T : IBBWriter
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<byte>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<sbyte>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<short>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<ushort>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<int>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<uint>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<long>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<ulong>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<float>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<double>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<bool>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<string>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<DateTime>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer ToByteBuffer(this List<List<ByteBuffer>> vs)
        {
            var rtv = new ByteBuffer();
            rtv.Write(vs);
            return rtv;
        }
        public static ByteBuffer VarToByteBuffer(this uint v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarToByteBuffer(this int v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarToByteBuffer(this ulong v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarToByteBuffer(this long v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }
        public static ByteBuffer VarToByteBuffer(this double v)
        {
            var rtv = new ByteBuffer();
            rtv.VarWrite(v);
            return rtv;
        }

    }

    #region interface

    public interface IBBReader
    {
        void ReadFrom(ByteBuffer tb);
    }

    public interface IBBWriter
    {
        void WriteTo(ByteBuffer tb);
        short PackageId { get; }
    }

    #endregion

}
