using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using xxlib;

public class Program
{
    static void Main(string[] args)
    {
        var inDir = Path.Combine(Application.StartupPath, "../../Cfgs");
        var fn = Path.Combine(inDir, typeof(Cfg.Map).FullName);
        var bytes = File.ReadAllBytes(fn);
        var bb = new ByteBuffer(bytes, bytes.Length);

        var os = new List<Cfg.Map>();
        bb.Read(ref os);
        Console.WriteLine(os);
    }

    public static XY CalcPos(int x, int y, int rx, int ry, int a)
    {
        XY rtv;
        rtv.x = (float)((rx >> 1) * Math.Cos(a * Math.PI / 180.0) + x);
        rtv.y = (float)((ry >> 1) * Math.Sin(a * Math.PI / 180.0) + y);
        return rtv;
    }
}
