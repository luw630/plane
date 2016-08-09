using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateGen_Cfg
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var fn in Directory.GetFiles(Application.StartupPath, "Template_*.dll"))
            {
                var asm = Assembly.LoadFile(fn);
                var t = TemplateScaner.GetTemplate(asm);
                var shortfn = new FileInfo(fn).Name;
                shortfn = shortfn.Substring(0, shortfn.LastIndexOf('.'));
                t.Name = shortfn.Substring("Template_".Length);

                var path = System.IO.Path.Combine(Application.StartupPath, "../../../../Excels");
                GenExcel.Gen(t, path);
            }
        }
    }
}
