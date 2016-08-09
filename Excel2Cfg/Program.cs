using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class Program
{
    static void Main(string[] args)
    {
        foreach (var dll in Directory.GetFiles(Application.StartupPath, "Template_*.dll"))
        {
            var asm = Assembly.LoadFile(dll);
            var t = TemplateScaner.GetTemplate(asm);
            var shortfn = new FileInfo(dll).Name;
            shortfn = shortfn.Substring(0, shortfn.LastIndexOf('.'));
            t.Name = shortfn.Substring("Template_".Length);

            var inDir = Path.Combine(Application.StartupPath, "../../../../Excels");
            var outDir = Path.Combine(Application.StartupPath, "../../../../Cfgs");

            foreach (var c in t.Structs.Where(a => a.IsPackage))
            {
                var fn = Path.Combine(inDir, c.GetFullName(t) + ".xlsx");
                var md = Excel2DataTable(fn, "Metadata");
                var cd = Excel2DataTable(fn, "ConfigData");
                Excel2Cfg.Gen(fn, t, c, cd, outDir);
            }
        }
    }

    public static DataTable Excel2DataTable(string fn, string sheet)
    {
        string sql = "SELECT * FROM [" + sheet + "$]";
        var connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fn + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;'";
        var da = new OleDbDataAdapter(sql, connStr);
        var ds = new DataSet();
        da.Fill(ds);
        return ds.Tables[0];
    }
}
