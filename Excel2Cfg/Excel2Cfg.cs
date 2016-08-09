using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using xxlib;

public static class Excel2Cfg
{
    public static void Gen(string fn, Template template, Struct pkg, DataTable dt, string outDir)
    {
        var cfn = pkg.GetFullName(template);
        if (dt == null || dt.Rows.Count <= 3)
        {
            Console.WriteLine($"Excel 文件: {fn} 读取错误( 没有数据? )");
            Console.WriteLine("按回车键 退出当前文件处理!");
            Console.ReadLine();
            return;
        }

        // 按类名定位到拥有这个类实例的 assembly
        var asm = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetTypes().Count(b => b.FullName == cfn) > 0).First();
        var t = asm.GetTypes().FirstOrDefault(x => x.FullName == cfn);
        var tn = t.Name;
        var fs = t.GetFields();             // 取其所有 fields

        // 所有列名塞字典
        var colNames = new Dictionary<int, string>();
        var numRows = dt.Rows.Count;
        var numCols = dt.Columns.Count;
        for (int i = 0; i < numCols; i++)
        {
            var cn = dt.Rows[0][i].ToString();
            if (string.IsNullOrEmpty(cn))
            {
                Console.WriteLine($"Excel 文件: {fn} 第 {i} 列 列名读取错误!");
                Console.WriteLine("按回车键 退出当前文件处理!");
                Console.ReadLine();
                return;
            }
            colNames.Add(i, cn.Trim());
        }

        // 类数据容器( 每一行将转为它的一个元素 )
        var os = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(t));

        // 主键冲突检查容器( 当前的潜规则为 id 要声明为类成员的第 1 个 )
        var pks = new HashSet<string>();

        // 从第 4 行开始扫( 这是填的数据区的开始 )
        for (int i = 3; i < numRows; i++)
        {
            // 读主键
            var pk = dt.Rows[i][0].ToString().Trim();

            // 主键为空的跳过
            if (string.IsNullOrEmpty(pk)) continue;

            // 主键重复检查
            if (!pks.Add(pk))
            {
                Console.WriteLine($"Excel文件({fn}) 主键重复：行号={(i + 1)}, 主键值={ dt.Rows[i][0].ToString()}");
                Console.WriteLine("按回车键继续...");
                Console.ReadLine();
                continue;
            }

            // 创建一个实例用于放置当前行数据
            var o = Activator.CreateInstance(t);
            os.Add(o);

            // 用以判断如果一个 子类 的 fields 全是 空. 那就从 list 中移除                
            var cfNullCount = 0;

            // 开始横扫一行
            for (int j = 0; j < colNames.Count; j++)
            {
                //try
                //{
                var colName = colNames[j];
                var value = dt.Rows[i][j].ToString();
                if (colName.Contains("."))
                {
                    // 按点切割名字
                    string[] names = colName.Replace("*", "").Split('.');

                    // 定位1级名字( 类成员变量 )
                    var f = fs.FirstOrDefault(a => a.Name == names[0]);
                    if (f == null)
                    {
                        Console.WriteLine($"未知字段名：{colName}，请检查是否忘记更新。");
                        Console.WriteLine("按回车键继续...");
                        Console.ReadLine();
                        continue;
                    }
                    var ft = f.FieldType;

                    // List 处理分支
                    if (ft.IsGenericType)
                    {
                        // 处理 List 的子的类型
                        var ct = ft.GenericTypeArguments[0];
                        var ctfs = ct.GetFields();

                        // 定位到子类的当前字段
                        var ctf = ctfs.FirstOrDefault(a => a.Name == names[1]);
                        if (ctf == null)
                        {
                            Console.WriteLine($"未知字段名：{colName}，请检查是否忘记更新。");
                            Console.WriteLine("按回车键继续...");
                            Console.ReadLine();
                            continue;
                        }

                        IList list;
                        object item;
                        if (colName.StartsWith("*"))
                        {
                            // 创建或取回 List
                            list = (IList)f.GetValue(o);
                            if (list == null)
                            {
                                list = (IList)Activator.CreateInstance(ft);
                                f.SetValue(o, list);
                            }

                            // 创建子并放入 List
                            item = Activator.CreateInstance(ct);
                            list.Add(item);

                            // 重置空值计数
                            cfNullCount = 0;
                        }
                        else
                        {
                            list = (IList)f.GetValue(o);
                            item = list[list.Count - 1];
                        }

                        // 向 listItem 填当前 field 的值( 如果有填的话 )
                        if (!string.IsNullOrEmpty(value))
                        {
                            var ctft = ctf.FieldType;
                            if (ctft.IsEnum)
                            {
                                ctf.SetValue(item, Enum.Parse(ctft, value));
                            }
                            else if (ctft == typeof(bool))
                            {
                                ctf.SetValue(item, value.GetBool());
                            }
                            else
                            {
                                ctf.SetValue(item, Convert.ChangeType(value, ctft));
                            }
                        }
                        else
                        {
                            cfNullCount++;

                            // 判断当前 listItem 是否填充完毕. 如果 全是空值. 则移除之
                            if (cfNullCount == ctfs.Length)
                            {
                                list.Remove(item);
                            }
                        }
                    }
                    // 直接展开类处理分支
                    else
                    {
                        var ctfs = ft.GetFields();

                        // 定位到子类的当前字段
                        var ctf = ctfs.FirstOrDefault(a => a.Name == names[1]);
                        if (ctf == null)
                        {
                            Console.WriteLine($"未知字段名：{colName}，请检查是否忘记更新。");
                            Console.WriteLine("按回车键继续...");
                            Console.ReadLine();
                            continue;
                        }

                        object item;
                        if (colName.StartsWith("*"))
                        {
                            // 创建子类型并赋给 o.ctf
                            item = Activator.CreateInstance(ft);
                            f.SetValue(o, item);
                        }
                        else
                        {
                            item = f.GetValue(o);
                        }

                        // 向 listItem 填当前 field 的值( 如果有填的话 )
                        if (!string.IsNullOrEmpty(value))
                        {
                            var ctft = ctf.FieldType;
                            if (ctft.IsEnum)
                            {
                                ctf.SetValue(item, Enum.Parse(ctft, value));
                            }
                            else if (ctft == typeof(bool))
                            {
                                ctf.SetValue(item, value.GetBool());
                            }
                            else
                            {
                                ctf.SetValue(item, Convert.ChangeType(value, ctft));
                            }
                        }
                    }
                }
                else if (colName.StartsWith("*"))  // List<基础类型>
                {
                    var fname = colName.Replace("*", "");
                    var f = fs.FirstOrDefault(a => a.Name == fname);
                    var ft = f.FieldType;
                    if (f == null)
                    {
                        Console.WriteLine($"未知字段名：{colName}，请检查是否忘记更新。");
                        Console.WriteLine("按回车键继续...");
                        Console.ReadLine();
                        continue;
                    }
                    if (ft.IsGenericType && ft.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var list = (IList)f.GetValue(o);
                        if (list == null)
                        {
                            list = (IList)Activator.CreateInstance(ft);
                            f.SetValue(o, list);
                        }

                        if (!string.IsNullOrEmpty(value))
                        {
                            var t2 = ft.GenericTypeArguments[0];
                            if (t2.IsEnum)
                            {
                                list.Add(Enum.Parse(t2, value));
                            }
                            else if (t2 == typeof(bool))
                            {
                                list.Add(value.GetBool());
                            }
                            else
                            {
                                list.Add(Convert.ChangeType(value, t2));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"未处理的字段类型：{colName}");
                        Console.WriteLine("按回车键继续...");
                        Console.ReadLine();
                        continue;
                    }
                }
                // 普通字段填充
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var field = fs.FirstOrDefault(f => f.Name == colName);
                        if (field == null)
                        {
                            Console.WriteLine($"未知字段名：{colName}，请检查是否忘记更新。");
                            Console.WriteLine("按回车键继续...");
                            Console.ReadLine();
                            continue;
                        }
                        if (field.FieldType.IsEnum)
                        {
                            field.SetValue(o, Enum.Parse(field.FieldType, value));
                        }
                        else if (field.FieldType == typeof(bool))
                        {
                            field.SetValue(o, value.GetBool());
                        }
                        else
                        {
                            field.SetValue(o, Convert.ChangeType(dt.Rows[i][j], field.FieldType));
                        }
                    }
                }
                //}
                //catch (Exception eee)
                //{
                //    Console.WriteLine("->处理excel文件:" + fn + "出错;*********错误位置行=" + (i + 1) + ",列=" + (j + 1) + ";*******错误原因为:\r\n" + eee.Message);
                //    Console.WriteLine("按回车键 退出当前文件处理!");
                //    Console.ReadLine();
                //    return;
                //}
            }
        }

        var bb = new ByteBuffer();
        bb.WriteLength(os.Count);
        foreach (IBBWriter c in os)
        {
            bb.Write(c);
        }
        var outfn = Path.Combine(outDir, cfn);
        File.WriteAllBytes(outfn, bb.DumpData());
    }

    public static string GetFullName(this Struct s, Template t)
    {
        return t.Name + "." + (s.Namespace == "" ? "" : (s.Namespace + ".")) + s.Name;
    }

    public static bool GetBool(this string v)
    {
        v = v.Trim();
        return v == "真" || v == "1" || v.ToLower() == "yes" || v.ToLower() == "true";
    }
}
