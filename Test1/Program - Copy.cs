using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 结论: 实现了接口的父子, 看上去是菱形继承, 然而并不是, 接口函数自动标虚, 子继承相同接口, 用 new 关键字就相当于 override 了.

namespace Test1
{
    public interface III
    {
        int GetID();
    }
    public class A : III
    {
        public int GetID()
        {
            return 1;
        }
    }
    public class B : A/*, III*/
    {
        public new  int GetID() { return base.GetID() + 1; }
        //public III GetBaseIII() { return (III)base; }
    }
    class Program
    {
        static void Output(III o)
        {
            Console.WriteLine(o.GetID());
        }
        static void Main(string[] args)
        {
            var b = new B();
            var t = b.GetType();
            Output(b);



            //var bb = new xxlib.ByteBuffer();
            //var p = new Pkg.Plane();
            //var bp = (Pkg.FlyerBase)p;
            ////bb.Write(p);
            //bb.Write(bp);
            //bb.Dump();
        }
    }
}
