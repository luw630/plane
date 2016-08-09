using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extensions
{

    public static string FileName = "";

    public static StringBuilder WriteFormat(this StringBuilder sb, string format, params object[] ps)
    {
        var idx = 0;
        while (idx < format.Length)
        {
            var c = format[idx++];
            if (c == '%')
            {
                c = format[idx++];
                if (c == '%')
                    sb.Append(c);
                else
                    sb.Append(ps[c - '0']);
            }
            else
                sb.Append(c);
        }
        return sb;
    }
    public static string WriteFormat(this string s, string format, params object[] ps)
    {
        return WriteFormat(new StringBuilder(s), format, ps).ToString();
    }

    // 追加 一些 空格在当前行的最后面直到令当前行的长度满足 len
    public static StringBuilder AppendFormatSpace(this StringBuilder sb, int len)
    {
        // 倒着扫出当前行
        var i = sb.Length;
        for (; i > 0; --i)
        {
            if (sb[i] == '\n')
                break;
        }
        var ll = sb.Length - i;
        if (ll < len)
            sb.Append(new string(' ', len - ll));
        return sb;
    }

    public static string FormatDesc(this string s, string preFix)
    {
        var sb = new StringBuilder();
        sb.Append(preFix);
        foreach (var c in s)
        {
            sb.Append(c);
            if (c == '\n')
                sb.Append(preFix);
        }
        return sb.ToString();
    }

    public static string FormatDesc_CS_Quote(this string s)
    {
        var sb = new StringBuilder();
        foreach (var c in s)
        {
            sb.Append(c);
            if (c == '"')
                sb.Append("\"\"");
        }
        return sb.ToString();
    }

    public static byte[] _bom = { 0xEF, 0xBB, 0xBF };
    public static void Write(this string fn, StringBuilder sb, bool useBOM = true)
    {
        File.Delete(fn);
        using (var fs = File.OpenWrite(fn))
        {
            if (useBOM)
                fs.Write(_bom, 0, _bom.Length);
            var buf = Encoding.UTF8.GetBytes(sb.ToString());
            fs.Write(buf, 0, buf.Length);
            fs.Close();
        }
    }

    public static void WriteToFile(this StringBuilder sb, string fn, bool useBOM = true)
    {
        fn.Write(sb, useBOM);
    }











    public static string ToFirstUpper(this string s)
    {
        if (s.Length <= 1)
            return s.ToUpper();
        return s.Substring(0, 1).ToUpper() + s.Substring(1);
    }

    public static string ToFirstLower(this string s)
    {
        if (s.Length <= 1)
            return s.ToLower();
        return s.Substring(0, 1).ToLower() + s.Substring(1);
    }



    public static bool Compressable(this DataType t)
    {
        if (t.IsCustom || t.IsEnum || t.IsArray)
            return false;
        if (t.IsGeneric) return Compressable(t.ChildType);
        if (t.Name == "UInt32"
            || t.Name == "Int32"
            || t.Name == "UInt64"
            || t.Name == "Int64"
            || t.Name == "Double"
            )
        {
            return true;
        }
        return false;
    }
}
