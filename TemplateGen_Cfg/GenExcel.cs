using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

public static class GenExcel
{
    public static void Gen(Template t, string outDir)
    {
        foreach (var c in t.Structs)
        {
            foreach (var m in c.Members)
            {
                if (m.Title == "") m.Title = m.Name;
            }
        }

        foreach (var c in t.Structs.Where(a => a.IsPackage))
        {
            var excel = new Excel.Application();
            try
            {
                var wb = excel.Workbooks.Add();
                while (wb.Sheets.Count < 3) wb.Sheets.Add();

                var fn = Path.Combine(outDir, c.GetFullName(t) + ".xlsx");
                Console.WriteLine("Exporting Excel File: {0}", fn);



                // 第一页 描述
                var ws = (Excel._Worksheet)wb.Sheets.Item[1];
                ws.Name = "Metadata";
                ws.Cells[1, "A"] = "警告本工作薄(Worksheet)仅用于程序描述";
                ws.Cells[2, "A"] = "不可人为修改";

                // Color: abgr
                var A1Z1 = ws.Range["A1", "Z1"];
                A1Z1.Interior.Color = 0x00FFFF;
                A1Z1.Font.Color = 0x0000FF;
                A1Z1.Font.Bold = true;
                A1Z1.MergeCells = true;

                var A2Z2 = ws.Range["A2", "Z2"];
                A2Z2.Interior.Color = 0x00FFFF;
                A2Z2.Font.Color = 0x0000FF;
                A2Z2.Font.Bold = true;
                A2Z2.MergeCells = true;

                ws.Cells[4, "A"] = "Class Name";
                ws.Cells[4, "B"] = c.GetFullName(t);

                ws.Columns[1].Font.Bold = true;
                ws.Columns[1].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                ws.Columns[1].AutoFit();
                ws.Columns[2].AutoFit();

                // 导出相关 enum
                int enum_row = 3;
                int curr_col = 3;
                var enums = new HashSet<DataType>();
                GetEnumTypes(c, ref enums);
                foreach (var cc in enums)
                {
                    int row = enum_row;
                    int col = curr_col++;

                    ws.Cells[row, col] = cc.Name;
                    ws.Rows[row].Font.Bold = true;
                    row++;

                    foreach (var m in cc.Custom.Members)
                    {
                        ws.Cells[row++, col] = m.Name;
                    }
                }

                for (int i = 1; i < curr_col; i++)
                    ws.Columns[i].AutoFit();




                // 第二页 数据
                ws = (Excel._Worksheet)wb.Sheets.Item[2];
                ws.Name = "ConfigData";
                // 导出表头
                int title_row = 2;
                int sub_title_row = 3;
                int name_row = 1;
                int cidx = 1;
                foreach (var fi in c.Members)
                {
                    var vt = fi.Type;
                    if (vt.IsArray || (vt.IsGeneric && vt.Name == "List") || vt.IsCustom)
                    {
                        List<Member> ms;
                        if (vt.IsCustom)
                        {
                            ms = vt.Custom.Members;
                        }
                        else
                        {
                            var ct = vt.ChildType;
                            if (ct.IsCustom) ms = ct.Custom.Members;
                            else
                            {
                                ms = new List<Member>();
                                ms.Add(new Member { Name = "", Title = ct.Name, Desc = "", Type = ct });       // 模拟成 member
                            }
                        }

                        // 展开并生成多少组
                        int repeats = vt.IsCustom ? 1 : Math.Max(1, fi.MaxLen);
                        for (int i = 1; i <= repeats; i++)
                        {
                            for (int j = 0; j < ms.Count; j++)
                            {
                                var m = ms[j];

                                if (j == 0)
                                {
                                    ws.Cells[title_row, cidx] = (fi.Title == "" ? fi.Name : fi.Title) + "[" + i + "]";

                                    var s = string.Format("{0}{2}:{1}{2}", cidx.ToString26(), (cidx + ms.Count - 1).ToString26(), title_row);
                                    ws.Range[s].MergeCells = true;

                                    s = string.Format("{0}:{1}", cidx.ToString26(), (cidx + ms.Count - 1).ToString26());
                                    var r = ws.Range[s];

                                    var b = r.Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                    b.LineStyle = Excel.XlLineStyle.xlDash;
                                    b.Weight = Excel.XlBorderWeight.xlThin;

                                    b = r.Borders[Excel.XlBordersIndex.xlEdgeRight];
                                    b.LineStyle = Excel.XlLineStyle.xlDash;
                                    b.Weight = Excel.XlBorderWeight.xlThin;
                                }

                                var cell = ws.Cells[sub_title_row, cidx];
                                cell.Value2 = m.Title;
                                cell.Font.Bold = true;
                                cell.AddComment(fi.Title + "." + m.Title + " " + m.Desc);

                                cell = ws.Cells[name_row, cidx];
                                cell.Value2 = (j == 0 ? "*" : "") + fi.Name + (m.Name == "" ? "" : ("." + m.Name));
                                cell.Font.Italic = true;
                                cell.Font.Bold = false;
                                cell.Font.Size = 6;

                                cidx++;
                            }
                        }
                    }
                    else
                    {
                        if (vt.IsGeneric)
                        {
                            throw new Exception("Unsupport GenericType: " + vt.Name);
                        }

                        var cell = ws.Cells[sub_title_row, cidx];
                        cell.Value2 = fi.Title;
                        cell.Font.Bold = true;
                        cell.AddComment(vt.Name + " " + vt.Desc);

                        cell = ws.Cells[name_row, cidx];
                        cell.Value2 = fi.Name;
                        cell.Font.Italic = true;
                        cell.Font.Size = 6;

                        cidx++;
                    }
                }

                var cfgHead = (Excel.Range)ws.Rows["1:3"];
                cfgHead.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                var border = cfgHead.Borders[Excel.XlBordersIndex.xlEdgeBottom];
                border.LineStyle = Excel.XlLineStyle.xlDouble;
                border.Weight = Excel.XlBorderWeight.xlThick;

                for (int i = 1; i < cidx; i++) ws.Columns[i].AutoFit();

                // 将数据页设为默认
                ws.Activate();



                // delete more sheets if exists
                ((Excel._Worksheet)wb.Sheets.Item[3]).Delete();

                if (File.Exists(fn)) File.Delete(fn);

                wb.SaveAs(fn);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.ReadKey();
            //}
            finally
            {
                excel.Quit();
            }
        }
    }

    public static string ToString26(this int i)
    {
        Debug.Assert(i > 0);

        i--;

        StringBuilder rtv = new StringBuilder();
        do
        {
            if (rtv.Length > 0) i--;
            int m = i % 26;
            rtv.Insert(0, ((char)('A' + m)));
            i /= 26;
        }
        while (i > 0);

        return rtv.ToString();
    }

    public static string GetFullName(this Struct s, Template t)
    {
        return t.Name + "." + (s.Namespace == "" ? "" : (s.Namespace + ".")) + s.Name;
    }
    public static void GetEnumTypes(Struct t, ref HashSet<DataType> enums)
    {
        foreach (var fi in t.Members)
        {
            var ft = fi.Type;
            if (ft.IsEnum)
            {
                enums.Add(ft); continue;
            }

            if (ft.IsCustom)
            {
                foreach (var n in ft.Custom.Members)
                {
                    GetEnumTypes(n.Type, ref enums);
                }
            }
            else if (ft.IsArray || ft.IsGeneric)
            {
                foreach (var ct in ft.ChildTypes)
                {
                    GetEnumTypes(ct, ref enums);
                }
            }
        }
    }

    public static void GetEnumTypes(DataType t, ref HashSet<DataType> enums)
    {
        if (!t.IsCustom) return;
        foreach (var fi in t.Custom.Members)
        {
            var ft = fi.Type;
            if (ft.IsEnum)
            {
                enums.Add(ft); continue;
            }

            if (ft.IsCustom)
            {
                foreach (var n in ft.Custom.Members)
                {
                    GetEnumTypes(n.Type, ref enums);
                }
            }
            else if (ft.IsArray || ft.IsGeneric)
            {
                foreach (var ct in ft.ChildTypes)
                {
                    GetEnumTypes(ct, ref enums);
                }
            }
        }
    }
}
