using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class GenCS_Class
{
    public static void Gen_tn_pn_ByteBuffer_Ext1(StringBuilder sb, Template t, List<Struct> proj_enums, List<Struct> proj_packages_and_structs)
    {

        sb.Clear();
        // 文件头部
        sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;
using xxlib;

namespace " + t.Name + @"
{
    public static partial class ByteBuffer_Ext
    {");
        //        foreach (var e in proj_enums.Where(a => !a.IsExternal))
        foreach (var e in proj_enums)
        {
            var tn = GetNamespace(e) + "." + e.Name;
            var etn = GetEnumTypeKeyword(e);
            sb.Append(@"
        public static void WriteEnum(this ByteBuffer bb, " + tn + @" v)
        {
            bb.Write((" + etn + @")v);
        }
        public static void WriteEnum(this ByteBuffer bb, " + tn + @"[] vs)
        {
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Write((" + etn + @")vs[i]);
            }
        }
        public static void WriteEnum(this ByteBuffer bb, List<" + tn + @"> vs)
        {
            bb.WriteLength(vs.Count);
            foreach (var v in vs)
            {
                bb.Write((" + etn + @")v);
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref " + tn + @" v)
        {
            " + etn + @" tmp = 0;
            bb.Read(ref tmp);
            v = (" + tn + @")tmp;
        }
        public static void ReadEnum(this ByteBuffer bb, ref " + tn + @"[] vs)
        {
            " + etn + @" tmp = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                bb.Read(ref tmp);
                vs[i] = (" + tn + @")tmp;
            }
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<" + tn + @"> vs)
        {
            ReadEnum(bb, ref vs, 0, 0);
        }
        public static void ReadEnum(this ByteBuffer bb, ref List<" + tn + @"> vs, int minLen, int maxLen)
        {
            int len = bb.ReadLength(minLen, maxLen);
            vs.Clear();
            for (int i = 0; i < len; i++)
            {
                " + etn + @" tmp = 0;
                bb.Read(ref tmp);
                vs.Add((" + tn + @")tmp);
            }
        }
");
        }

        sb.Append(@"
    }
}
");
    }

    public static void Gen_tn_pn(StringBuilder sb, Template t, List<Struct> proj_packages_and_structs, Project proj)
    {
        // 文件头部
        sb.Clear();
        sb.Append(@"using System;
using System.Collections.Generic;
using xxlib;");


        // namespace root {
        sb.Append(@"
namespace " + t.Name + @"
{");

        /*******************************************************/
        // 结构体部分
        /*******************************************************/

        for (int i = 0; i < proj_packages_and_structs.Count; ++i)
        {
            var c = proj_packages_and_structs[i];
            var cob = "";
            if (c.Deprecated != null)
            {
                cob = @"
[Obsolete(""""," + c.Deprecated.Value + ")]";
            }

            // namespace xxx {
            if (c.Namespace != "" && (i == 0 || (i > 0 && proj_packages_and_structs[i - 1].Namespace != c.Namespace)))
            {
                sb.Append(@"
namespace " + c.Namespace + @"
{");
            }

            // ///  /// desc  ///
            sb.Append(GetComment(c.Desc, 4) + cob + @"
    public partial " + (c.IsValueType ? "struct" : "class") + " " + c.Name + (c.BaseType == null ? "" : (" : " + GetTypeKeyword(c.BaseType, GetNamespace(c)))) + @"
    {");
            // members = default vals;
            foreach (var f in c.Members)
            {
                var dv = f.DefaultValue == null ? "" : (" = " + f.DefaultValue.ToString());
                if (f.Type.IsBuiltIn && f.Type.Name == "Boolean") dv = dv.ToLower();
                if (f.Type.IsEnum || dv == "") dv = " = " + GetDefaultValueByType(f, GetNamespace(c));
                if (f.Type.IsCustom && f.Type.Custom.IsValueType) dv = "";
                sb.Append(GetComment(f.Desc, 8) + @"
        public " + GetTypeKeyword(f, GetNamespace(c)) + " " + f.Name + (c.IsValueType ? "" : dv) + ";");
            }

            // class }
            sb.Append(@"
    }");

            // namespace } // xxx
            if (c.Namespace != "" && ((i < proj_packages_and_structs.Count - 1 && proj_packages_and_structs[i + 1].Namespace != c.Namespace)
                || i == proj_packages_and_structs.Count - 1))
            {
                sb.Append(@"
}");
            }
        }



        // namespace } // root
        sb.Append(@"
}
");
    }

    public static void Gen_tn_pn_Enums(StringBuilder sb, Template t, List<Struct> proj_enums)
    {
        // 文件头部
        sb.Clear();
        sb.Append(@"using System;");


        // namespace root {
        sb.Append(@"
namespace " + t.Name + @"
{");

        /*******************************************************/
        // 所有项目枚举
        /*******************************************************/

        var es = proj_enums.Where(a => !a.IsExternal).ToList();
        for (int i = 0; i < es.Count; ++i)
        {
            var e = es[i];

            // namespace xxx {
            if (e.Namespace != "" && (i == 0 || (i > 0 && es[i - 1].Namespace != e.Namespace)))
            {
                sb.Append(@"
namespace " + e.Namespace + @"
{");
            }

            string ob = "";
            if (e.Deprecated != null)
            {
                ob = @"
    [Obsolete(""""," + e.Deprecated.Value + ")]";
            }

            // ///  /// desc  ///
            sb.Append(GetComment(e.Desc, 4) + ob + @"
    public enum " + e.Name + @" : " + GetEnumTypeKeyword(e) + @"
    {");
            // enum item = val
            foreach (var ef in e.Members)
            {
                sb.Append(GetComment(ef.Desc, 8) + @"
        " + ef.Name + " = " + ef.EnumValue + ", ");
            }
            // enum class }; // xxx
            sb.Append(@"
    }");


            // namespace } // xxx
            if (e.Namespace != "" && ((i < es.Count - 1 && es[i + 1].Namespace != e.Namespace)
    || i == es.Count - 1))
            {
                sb.Append(@"
}");
            }
        }

        // namespace } // root
        sb.Append(@"
}
");
    }

    public static void Gen_tn_pn_Partial(StringBuilder sb, Template t, List<Struct> proj_enums, List<Struct> proj_packages_and_structs, Project proj = null)
    {
        // 文件头部
        sb.Clear();
        sb.Append(@"#pragma warning disable 0109
using System;
using System.Collections.Generic;
using xxlib;");

        // namespace root {
        sb.Append(@"
namespace " + t.Name + @"
{");

        /*******************************************************/
        // 结构体部分
        /*******************************************************/

        foreach (var c in proj_packages_and_structs)
        {
            bool genWriteTo = true, genReadFrom = true;
            if (proj != null)
            {
                var srt = c.Projects.First(a => a.Project == proj)?.SendRecvType;
                genWriteTo = (srt == null || srt == TemplateLibrary.SendRecvTypes.SendOnly || srt == TemplateLibrary.SendRecvTypes.SendAndRecv);
                genReadFrom = (srt == null || srt == TemplateLibrary.SendRecvTypes.RecvOnly || srt == TemplateLibrary.SendRecvTypes.SendAndRecv);
            }

            // namespace xxx {
            if (c.Namespace != "")
            {
                sb.Append(@"
namespace " + c.Namespace + @"
{");
            }

            // partial class xxx

            var intrface = "";
            if (genReadFrom)
            {
                intrface += "IBBReader";
            }
            if (genWriteTo)
            {
                if (intrface != "")
                {
                    intrface += ", ";
                }
                intrface += "IBBWriter";
            }

            sb.Append(@"
public partial " + (c.IsValueType ? "struct" : "class") + " " + c.Name + @" : " + intrface + @" 
{
    public" + (c.BaseType != null ? " new" : "") + @" void OverrideTo(" + c.Name + @" o)
    {");
            if (c.BaseType != null)
            {
                sb.Append(@"
        base.OverrideTo(o);");
            }
            foreach (var f in c.Members)
            {
                sb.Append(@"
        o." + f.Name + @" = this." + f.Name + ";");
            }
            sb.Append(@"
    }
    public" + (c.BaseType != null ? " new" : "") + @" string FindDiff(" + c.Name + @" o, string rootName = """")
    {
        string _rtv_ = null;");
            if (c.BaseType != null)
            {
                sb.Append(@"
        _rtv_ = base.FindDiff(o, rootName + ""/../"");
        if(_rtv_ != null) return _rtv_;");
            }
            foreach (var f in c.Members)
            {
                var ft = f.Type;
                // 原生类型直接比。 容器 foreach 比。 custom 调其 FindDiff 比
                if (ft.IsCustom && !ft.IsEnum || ft.IsBuiltIn && ft.Name == "ByteBuffer")
                {
                    sb.Append(@"
        _rtv_ = this." + f.Name + @".FindDiff(o." + f.Name + @", rootName + ""/"");
        if(_rtv_ != null) return _rtv_;");
                }
                else if (ft.IsGeneric)
                {
                    sb.Append(@"
        for (int _i_ = 0; _i_ < this." + f.Name + @".Count; ++_i_)
        {");



                    ft = f.Type.ChildType;
                    if (ft.IsCustom && !ft.IsEnum || ft.IsBuiltIn && ft.Name == "ByteBuffer")
                    {
                        sb.Append(@"
            _rtv_ = this." + f.Name + @"[_i_].FindDiff(o." + f.Name + @"[_i_], rootName + ""/" + f.Name + @"["" + _i_ + ""]/"");
            if(_rtv_ != null) return _rtv_;");
                    }
                    else if (ft.IsGeneric)
                    {
                        throw new Exception("not support now");
                    }
                    else
                    {
                        sb.Append(@"
            if(o." + f.Name + @"[_i_] != this." + f.Name + @"[_i_]) return rootName + @""/" + f.Name + @"["" + _i_ + @""] is diff! 
this  = "" + this." + f.Name + @"[_i_] + @"",
other = "" + o." + f.Name + @"[_i_];");
                    }



                    sb.Append(@"
        }");
                }
                else
                {
                    sb.Append(@"
        if(o." + f.Name + @" != this." + f.Name + @") return rootName + @""/" + f.Name + @" is diff! 
this  = "" + this." + f.Name + @" + @"",
other = "" + o." + f.Name + @";");
                }
            }
            sb.Append(@"
        return _rtv_;
    }
");
            if (c.IsPackage)
            {
                sb.Append(@"
    public" + (c.BaseType != null ? " new" : "") + @" const short packageId = " + c.PackageId + @";");
            }

            if (genWriteTo)
            {
                if (c.IsPackage)
                {
                    sb.Append(@"
    public static readonly" + (c.BaseType != null ? " new" : "") + @" " + c.Name + @" DefaultInstance = new " + c.Name + @"();
    public static readonly" + (c.BaseType != null ? " new" : "") + @" byte[] DefaultPackageData = ByteBuffer.MakePackageData(new " + c.Name + @"());");
                }
                //                if (!c.IsValueType)
                //                {
                //                    sb.Append(@"
                //    public static implicit operator bool(" + c.Name + @" self)
                //    {
                //        return self != null;
                //    }
                //");
                //                }
                sb.Append(@"

    public" + (c.BaseType != null ? " new" : "") + @" short PackageId { get {");
                if (c.IsStruct)
                {
                    sb.Append(@" throw new Exception(""struct has no package id"");");
                }
                else
                {
                    sb.Append(@" return packageId;");
                }
                sb.Append(@" } }

    public" + (c.BaseType != null ? " new" : "") + @" void WriteTo(ByteBuffer _bb_)
    {");
                if (c.BaseType != null)
                {
                    sb.Append(@"
        base.WriteTo(_bb_);");
                }
                foreach (var f in c.Members)
                {
                    var e = (f.Type.IsEnum || f.Type.IsGeneric && f.Type.ChildType.IsEnum) ? "Enum" : "";

                    if (f.Type.IsCustom && HasDerivedPkgs(t, f.Type.Custom))
                    {
                        sb.Append(@"
        _bb_.WritePackage(" + f.Name + ");");
                    }
                    else if (f.Type.IsGeneric && f.Type.ChildType.IsCustom && HasDerivedPkgs(t, f.Type.ChildType.Custom))
                    {
                        sb.Append(@"
        _bb_.WriteLength(" + f.Name + @".Count);
        for (int i = 0; i < " + f.Name + @".Count; ++i)
        {
            _bb_.WritePackage(" + f.Name + @"[i]);
        }");
                    }
                    else sb.Append(@"
        _bb_." + (f.Compress && f.Type.Compressable() ? "Var" : "") + "Write" + e + "(" + f.Name + ");");
                }
                sb.Append(@" 
    }
");
            }
            if (genReadFrom)
            {
                sb.Append(@"
    public" + (c.BaseType != null ? " new" : "") + @" void ReadFrom(ByteBuffer _bb_)
    {");
                if (c.BaseType != null)
                {
                    sb.Append(@"
        base.ReadFrom(_bb_);");
                }
                foreach (var f in c.Members)
                {

                    if (f.Type.IsCustom && HasDerivedPkgs(t, f.Type.Custom))
                    {
                        sb.Append(@"
        switch(_bb_.ReadPackageId())
        {");
                        var dps = GetDerivedPkgs(t, f.Type.Custom);
                        foreach (var dp in dps)
                        {
                            var dptn = GetTypeKeyword(dp, GetNamespace(c));
                            sb.Append(@"
            case " + dptn + @".packageId: " + f.Name + ".Add(_bb_.Read<" + dptn + @">()); break;");
                        }
                        sb.Append(@"
        }");
                    }
                    else if (f.Type.IsGeneric && f.Type.ChildType.IsCustom && HasDerivedPkgs(t, f.Type.ChildType.Custom))
                    {
                        sb.Append(@"
        {
            int _len_ = _bb_.ReadLength(" + f.MinLen + ", " + f.MaxLen + @");
            for (int _i_ = 0; _i_ < _len_; _i_++)
            {
                switch(_bb_.ReadPackageId())
                {");
                        var dps = GetDerivedPkgs(t, f.Type.ChildType.Custom);
                        foreach (var dp in dps)
                        {
                            var dptn = GetTypeKeyword(dp, GetNamespace(c));
                            sb.Append(@"
                    case " + dptn + @".packageId: " + f.Name + ".Add(_bb_.Read<" + dptn + @">()); break;");
                        }
                        sb.Append(@"
                }
            }
        }
");
                    }
                    else
                    {

                        var e = (f.Type.IsEnum || f.Type.IsGeneric && f.Type.ChildType.IsEnum) ? "Enum" : "";
                        sb.Append(@"
        _bb_." + (f.Compress && f.Type.Compressable() ? "Var" : "") + "Read" + e + "(ref " + f.Name);
                        if (f.Type.IsContainer && !f.Type.IsArray)
                        {
                            sb.Append(@", " + f.MinLen + ", " + f.MaxLen);
                        }
                        sb.Append(@");");

                        if (f.Type.IsBuiltIn && (f.Type.Name == "Float" || f.Type.Name == "Single" || f.Type.Name == "Double"))
                        {
                            var dn = f.Type.Name == "Double" ? "double" : "float";
                            if (f.NaNValue == null && f.InfinityValue == null)
                            {
                                sb.Append(@"
        if (" + dn + @".IsNaN(" + f.Name + ") || " + dn + @".IsInfinity(" + f.Name + @")) throw new Exception(""the " + f.Name + @" can't be NaN or Infinity."");");
                            }
                            else
                            {
                                if (f.NaNValue == null)
                                {
                                    sb.Append(@"
        if (" + dn + @".IsNaN(" + f.Name + @")) throw new Exception(""the " + f.Name + @" can't be NaN."");");
                                }
                                else if (f.NaNValue != "")
                                {
                                    sb.Append(@"
        if (" + dn + @".IsNaN(" + f.Name + @")) " + f.Name + " = " + f.NaNValue.ToString() + ";");
                                }
                                if (f.InfinityValue == null)
                                {
                                    sb.Append(@"
        " + (f.NaNValue != null ? "else " : "") + @"if (" + dn + @".IsInfinity(" + f.Name + @")) throw new Exception(""the " + f.Name + @" can't be Infinity."");");
                                }
                                else if (f.InfinityValue != "")
                                {
                                    sb.Append(@"
        " + (f.NaNValue != null ? "else " : "") + @"if (" + dn + @".IsInfinity(" + f.Name + @")) " + f.Name + " = " + f.InfinityValue.ToString() + ";");
                                }
                            }
                        }
                    }
                }
                sb.Append(@"
    }
");
            }

            sb.Append(@"
}");
            // namespace } // xxx
            if (c.Namespace != "")
            {
                sb.Append(@"
} // " + c.Namespace);
            }
        }

        // namespace } // root
        sb.Append(@"
} // " + t.Name + @"
");
    }

    public static void Gen_tn_pn_Handlers(StringBuilder sb, Template t, List<Struct> proj_packages)
    {
        sb.Clear();
        sb.Append(@"using System;
using System.Collections.Generic;
using xxlib;
using " + t.Name + @";
namespace " + t.Name + @"
{
    public static partial class Pkg2Obj
    {
        public static T ConvertCore<T>(this ByteBuffer _bb_) where T : IBBReader, new()
        {
            var o = new T();
            _bb_.Read(ref o);
            return o;
        }

        public static IBBWriter Convert(this ByteBuffer _bb_)
        {
            try
            {
                switch(_bb_.ReadPackageId())
                {");
        foreach (var c in proj_packages)
        {
            var ns = GetNamespace(c);
            var tn = (ns == "" ? "" : (ns + ".")) + c.Name;
            sb.Append(@"
                    case " + tn + @".packageId : return ConvertCore<" + tn + @">(_bb_);");
        }

        sb.Append(@"
                    default: return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static bool Convert<T>(ByteBuffer _bb_, List<T> output) where T : IBBWriter
        {
            output.Clear();
            while(_bb_.offset < _bb_.dataLen)
            {
                var o = " + t.Name + @".Pkg2Obj.Convert(_bb_);
                if (o != null) output.Add((T)o);
                else return false;
            }
            return true;
        }

    }
}");
    }

    public static void Gen_packageId_config(StringBuilder sb, List<Struct> allpkgs)
    {
        // 生成 packageId -- ProjectName + Namespace + Name 的 csv 文件
        // 根据 接收者生成 pkgId 的规则，记录接收者的 ProjectName.
        sb.Clear();
        var lst = new List<string>();
        foreach (var pkg in allpkgs)
        {
            var pn = "";
            if (pkg.Projects.Count > 0)
            {
                var o = pkg.Projects.Find(a => a.SendRecvType == TemplateLibrary.SendRecvTypes.SendAndRecv
                || a.SendRecvType == TemplateLibrary.SendRecvTypes.RecvOnly);
                if (o != null)
                {
                    pn = o.Project.Name;
                }
            }
            lst.Add(pkg.PackageId + "," + pn + "," + pkg.Namespace + "," + pkg.Name);
        }
        lst.Sort((a, b) =>
        {
            var aa = a.Split(',');
            var bb = b.Split(',');
            return long.Parse(aa[0]).CompareTo(long.Parse(bb[0]));
        });
        sb.Append(string.Join(Environment.NewLine, lst.AsEnumerable()));
    }

    public static void Read_packageId_config(string csv, List<Struct> allpkgs, Template t)
    {
        if (string.IsNullOrEmpty(csv))
        {
            return;
        }

        // 先根据 Namespace + Name 定位到 pkg. 然后检查是否包含 接收Project 以最终确认
        // 剩下的需要避开固定下来的 pkgid , 按所在 接收Project 顺序生成

        // 先扫描 allpkgs, 按  接收Project + Namespace + Name  为 key 构造 Dict
        var dict = new Dictionary<string, Struct>();
        foreach (var pkg in allpkgs)
        {
            var pn = "";
            if (pkg.Projects.Count > 0)
            {
                var o = pkg.Projects.Find(a => a.SendRecvType == TemplateLibrary.SendRecvTypes.SendAndRecv
                || a.SendRecvType == TemplateLibrary.SendRecvTypes.RecvOnly);
                if (o != null)
                {
                    pn = o.Project.Name;
                }
            }
            dict[pn + "|" + pkg.Namespace + "|" + pkg.Name] = pkg;
        }

        var fixedidpkgs = new List<Struct>();
        var usedids = new HashSet<int>();

        var rows = csv.Replace("\r", "\n").Replace("\n\n", "\n").Split('\n');
        foreach (var row in rows)
        {
            var cols = row.Split(',');
            var key = cols[1] + "|" + cols[2] + "|" + cols[3];
            if (dict.ContainsKey(key))
            {
                var pkg = dict[key];
                fixedidpkgs.Add(pkg);
                pkg.PackageId = int.Parse(cols[0]);
                usedids.Add(pkg.PackageId);
            }
        }

        var autoidpkgs = allpkgs.Except(fixedidpkgs).ToList();
        // 先清空 MaxPackageId 备用
        t.MaxPackageId = 1;
        foreach (var p in t.Projects)
        {
            p.MaxPackageId = p.PackageIdFrom + 1;
        }

        // 与 TemplateScaner 里面的生成规则几乎一致。按 Project id段递增生成。 跳过已被占用的 id
        foreach (var pkg in autoidpkgs)
        {
            Project p = null;
            if (pkg.Projects.Count > 0)
            {
                var o = pkg.Projects.Find(a => a.SendRecvType == TemplateLibrary.SendRecvTypes.SendAndRecv
                || a.SendRecvType == TemplateLibrary.SendRecvTypes.RecvOnly);
                if (o != null)
                {
                    p = o.Project;
                }
            }
            if (p != null)
            {
                pkg.PackageId = p.MaxPackageId++;
                if (pkg.PackageId > p.PackageIdTo)
                {
                    throw new Exception("too many pkgs in single project");
                }
                while (usedids.Contains(pkg.PackageId))
                {
                    pkg.PackageId = p.MaxPackageId++;
                    if (pkg.PackageId > p.PackageIdTo)
                    {
                        throw new Exception("too many pkgs in single project");
                    }
                }
            }
            else
            {
                pkg.PackageId = t.MaxPackageId++;
                if (pkg.PackageId >= TemplateScaner.NumOfProjectPackages)
                {
                    throw new Exception("too many pkgs more than TemplateScaner.NumOfProjectPackages");
                }
                while (usedids.Contains(pkg.PackageId))
                {
                    pkg.PackageId = t.MaxPackageId++;
                    if (pkg.PackageId >= TemplateScaner.NumOfProjectPackages)
                    {
                        throw new Exception("too many pkgs more than TemplateScaner.NumOfProjectPackages");
                    }
                }
            }
        }

    }

    public static void Gen(Template t, string outDir)
    {
        _tn = t.Name;

        // 分类预处理( 跨所有项目 )
        var enums = t.Structs.Where(a => a.IsEnum).ToList();
        var packages = t.Structs.Where(a => a.IsPackage && !a.IsExternal).ToList();
        var packages_and_structs = t.Structs.Where(a => (a.IsPackage || a.IsStruct) && !a.IsExternal).ToList();
        var sb = new StringBuilder();


        var pkgid_cfg_fn = Path.Combine(outDir, "_" + t.Name + "_packageId_config.csv");
        var csv_content = "";
        try
        {
            csv_content = File.ReadAllText(pkgid_cfg_fn, Encoding.UTF8);
        }
        catch (Exception)
        {
        }
        Read_packageId_config(csv_content, packages, t);

        // 保存新的 PackageId 配置文件
        Gen_packageId_config(sb, packages);
        sb.WriteToFile(pkgid_cfg_fn);


        // 生成项目的
        foreach (var proj in t.Projects)
        {
            var proj_enums = enums.Where(a => a.Projects.Any(b => b.Project == proj)).ToList();
            var proj_packages_and_structs = packages_and_structs.Where(a => a.Projects.Any(b => b.Project == proj)).ToList();
            var proj_packages = packages.Where(a => a.Projects.Any(b => b.Project == proj)).ToList();

            Gen_tn_pn_ByteBuffer_Ext1(sb, t, proj_enums, proj_packages_and_structs);
            sb.WriteToFile(Path.Combine(outDir, t.Name + "_" + proj.Name + "_ByteBuffer_Ext.cs"));

            Gen_tn_pn_Enums(sb, t, proj_enums);
            sb.WriteToFile(Path.Combine(outDir, t.Name + "_" + proj.Name + "_Enums.cs"));

            Gen_tn_pn(sb, t, proj_packages_and_structs, proj);
            sb.WriteToFile(Path.Combine(outDir, t.Name + "_" + proj.Name + ".cs"));

            Gen_tn_pn_Partial(sb, t, proj_enums, proj_packages_and_structs, proj);
            sb.WriteToFile(Path.Combine(outDir, t.Name + "_" + proj.Name + "_Partial.cs"));

            Gen_tn_pn_Handlers(sb, t, proj_packages);
            sb.WriteToFile(Path.Combine(outDir, t.Name + "_" + proj.Name + "_Pkg2Obj.cs"));
        }

        // 生成全局的
        {
            var global_enums = enums.Where(a => a.Projects.Count == 0).ToList();
            var global_packages_and_structs = packages_and_structs.Where(a => a.Projects.Count == 0).ToList();
            var global_packages = packages.Where(a => a.Projects.Count == 0).ToList();

            Gen_tn_pn_ByteBuffer_Ext1(sb, t, global_enums, global_packages_and_structs);
            sb.WriteToFile(Path.Combine(outDir, "_" + t.Name + "_ByteBuffer_Ext.cs"));

            Gen_tn_pn_Enums(sb, t, global_enums);
            sb.WriteToFile(Path.Combine(outDir, "_" + t.Name + "_Enums.cs"));

            Gen_tn_pn(sb, t, global_packages_and_structs, null);
            sb.WriteToFile(Path.Combine(outDir, "_" + t.Name + ".cs"));

            Gen_tn_pn_Partial(sb, t, global_enums, global_packages_and_structs);
            sb.WriteToFile(Path.Combine(outDir, "_" + t.Name + "_Partial.cs"));

            Gen_tn_pn_Handlers(sb, t, global_packages);
            sb.WriteToFile(Path.Combine(outDir, "_" + t.Name + "_Pkg2Obj.cs"));
        }

        // 跨越的

        var forwardpkgs = new List<KeyValuePair<Project, Struct>>();
        forwardpkgs.AddRange(t.Structs.Where(a => a.IsPackage && a.Projects.Count == 0).OrderBy(a => a.PackageId)
            .Select(a => new KeyValuePair<Project, Struct>(null, a)));
        foreach (var proj in t.Projects)
        {
            forwardpkgs.AddRange(t.Structs.Where(a => a.IsPackage && a.Projects.Any(b => b.Project == proj
            && (b.SendRecvType == TemplateLibrary.SendRecvTypes.SendAndRecv
            || b.SendRecvType == TemplateLibrary.SendRecvTypes.RecvOnly
            ))).OrderBy(a => a.PackageId)
            .Select(a => new KeyValuePair<Project, Struct>(proj, a)));
        }
    }

    // 项目名上下文，省掉下面的传参了
    public static string _tn;


    #region GetComment

    public static string GetComment(string s, int space)
    {
        if (s.Trim() == "")
            return "";
        var sps = new string(' ', space);
        s = s.Replace("\r\n", "\n")
         .Replace("\r", "\n")
         .Replace("\n", "\r\n" + sps + "/// ");
        return "\r\n" +
    sps + @"/// <summary>
" + sps + @"/// " + s + @"
" + sps + @"/// </summary>";
    }

    #endregion


    #region GetDefaultValueByType

    public static string GetDefaultValueByType(Member f, string currNamespace = null)
    {
        return GetDefaultValueByType(f.Type, f, currNamespace);
    }
    public static string GetDefaultValueByType(DataType d, Member f = null, string currNamespace = null)
    {
        if (d.IsArray)
        {
            return "new " + GetTypeKeyword(d.ChildType, currNamespace) + "[ " + f.MinLen + " ]";   // "null"
        }
        else if (d.IsGeneric)
        {
            return "new " + d.Name + "<" + GetTypeKeyword(d.ChildType, currNamespace) + ">()";   // "null"
        }
        else if (d.IsBuiltIn)
        {
            switch (d.Name)
            {
                case "Byte":
                case "UInt8":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "SByte":
                case "Int8":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Double":
                case "Float":
                case "Single":
                    return "0";
                case "Boolean":
                case "Bool":
                    return "false";
                case "String":
                    return "\"\"";
                case "DateTime":
                    return "new DateTime( 1991, 1, 1, 0, 0, 0 )"; //"DateTime.MinValue";
                case "ByteBuffer":
                    return "new ByteBuffer()";
                default:
                    throw new Exception("unhandled data type");
            }
        }
        else    // custom / enum
        {
            if (d.IsEnum)
            {
                if (d.Custom.IsExternal)
                {
                    return "(" + d.Custom.ExternalNamespace + "." + d.Name + ")0";
                }
                if (d.Custom.Members.Count == 0)
                {
                    return "(" + (GetNamespace(d) == currNamespace || d.Namespace == "" ? "" : (GetNamespace(d) + ".")) + d.Name + ")0";
                }
                return (GetNamespace(d) == currNamespace || d.Namespace == "" ? "" : (GetNamespace(d) + ".")) + d.Name + "." + d.Custom.Members.First().Name;
            }
            else
            {
                if (d.Custom.IsExternal)
                {
                    return "new " + d.Custom.ExternalNamespace + "." + d.Name + "()";
                }
                return "new " + (GetNamespace(d) == currNamespace || d.Namespace == "" ? "" : (GetNamespace(d) + ".")) + d.Name + "()";    // "null"; 
            }
        }
    }

    #endregion


    #region GetTypeKeyword

    public static string GetTypeKeyword(Member d, string currNamespace = null)
    {
        return GetTypeKeyword(d.Type, currNamespace);
    }
    public static string GetTypeKeyword(DataType d, string currNamespace = null)
    {
        if (d.IsArray)                // 当前特指 byte[]
        {
            return GetTypeKeyword(d.ChildType, currNamespace) + "[]";
        }
        else if (d.IsGeneric)
        {
            string rtv = d.Name + "<";
            for (int i = 0; i < d.ChildTypes.Count; ++i)
            {
                if (i > 0)
                    rtv += ", ";
                rtv += GetTypeKeyword(d.ChildTypes[i], currNamespace);
            }
            rtv += ">";
            return rtv;
        }
        else if (d.IsBuiltIn)
        {
            var tn = "";
            switch (d.Name)
            {
                case "Void":
                    tn = "void";
                    break;
                case "Byte":
                    tn = "byte";
                    break;
                case "UInt8":
                    tn = "byte";
                    break;
                case "UInt16":
                    tn = "ushort";
                    break;
                case "UInt32":
                    tn = "uint";
                    break;
                case "UInt64":
                    tn = "ulong";
                    break;
                case "SByte":
                    tn = "sbyte";
                    break;
                case "Int8":
                    tn = "sbyte";
                    break;
                case "Int16":
                    tn = "short";
                    break;
                case "Int32":
                    tn = "int";
                    break;
                case "Int64":
                    tn = "long";
                    break;
                case "Double":
                    tn = "double";
                    break;
                case "Float":
                    tn = "float";
                    break;
                case "Single":
                    tn = "float";
                    break;
                case "Boolean":
                    tn = "bool";
                    break;
                case "Bool":
                    tn = "bool";
                    break;
                case "String":
                    tn = "string";
                    break;
                case "Literal":
                    tn = "string";
                    break;
                case "DateTime":
                    tn = "DateTime";
                    break;
                case "ByteBuffer":
                    tn = "ByteBuffer";
                    break;
                default:
                    throw new Exception("unhandled data type");
            }
            if (d.Nullable)
            {
                tn += "?";
            }
            return tn;
        }
        else // ( d.IsCustom )
        {
            return (GetNamespace(d) == currNamespace || d.Namespace == "" ? "" : (GetNamespace(d) + ".")) + d.Name;
        }
    }

    public static string GetTypeKeyword(Struct d, string currNamespace = null)
    {
        return (GetNamespace(d) == currNamespace || d.Namespace == "" ? "" : (GetNamespace(d) + ".")) + d.Name;
    }
    #endregion


    #region GetEnumTypeKeyword

    public static string GetEnumTypeKeyword(Member f)
    {
        return GetEnumTypeKeyword(f.Type.Custom);
    }
    public static string GetEnumTypeKeyword(Struct e)
    {
        switch (e.EnumUnderlyingType)
        {
            case "Byte":
                return "byte";
            case "SByte":
                return "sbyte";
            case "UInt16":
                return "ushort";
            case "Int16":
                return "short";
            case "UInt32":
                return "uint";
            case "Int32":
                return "int";
            case "UInt64":
                return "ulong";
            case "Int64":
                return "long";
        }
        throw new Exception("unsupported data type");
    }

    #endregion


    #region GetByteBufferReadFuncName
    public static string GetByteBufferReadFuncName(DataType d)
    {
        string rtv = " ";
        if (d.IsBuiltIn)
        {
            switch (d.Name)
            {

                case "Byte":
                    rtv = "ReadByte";
                    break;
                case "Int16":
                    rtv = "ReadShort";
                    break;
                case "Int32":
                    rtv = "ReadInt";
                    break;
                case "Int64":
                    rtv = "ReadLong";
                    break;
                case "Char":
                    rtv = "ReadChar";
                    break;
                case "Double":
                    rtv = "ReadDouble";
                    break;
                case "Single":
                    rtv = "ReadFloat";
                    break;
                case "Boolean":
                    rtv = "ReadBoolean";
                    break;
                case "DateTime":
                    rtv = "ReadDate";
                    break;
                case "String":
                    rtv = "ReadString";
                    break;
                default:
                    rtv = (d.Namespace != "" ? (d.Namespace + ".") : "") + d.Name;
                    break;
            }

        }
        else if (d.IsCustom)
        {
            if (d.IsEnum)
            {
                rtv = "ReadInt";
                return rtv;
            }
            else
            {
                rtv = "Read";
                return rtv;
            }

        }
        else if (d.IsGeneric)
        {
            if (d.IsArray)
            {
                rtv = "ReadBytes";      // 当前只支持 byte[]
            }
            else
            {
                switch (GetByteBufferReadFuncName(d.ChildType))
                {
                    case "ReadByte":
                        rtv = "ReadListByte";
                        break;
                    case "ReadShort":
                        rtv = "ReadListShort";
                        break;
                    case "ReadInt":
                        rtv = "ReadListInt";
                        break;
                    case "ReadLong":
                        rtv = "ReadListLong";
                        break;
                    case "ReadChar":
                        rtv = "ReadListChar";
                        break;
                    case "ReadDouble":
                        rtv = "ReadListDouble";
                        break;
                    case "ReadFloat":
                        rtv = "ReadListFloat";
                        break;
                    case "ReadBoolean":
                        rtv = "ReadListBool";
                        break;
                    case "ReadDate":
                        rtv = "ReadListDate";
                        break;
                    case "ReadString":
                        rtv = "ReadListString";
                        break;
                    case "Read":
                        rtv = "ReadListObject";
                        break;
                }
            }
        }
        return rtv;
    }

    #endregion


    #region GetNamespace

    public static string GetNamespace(DataType e)
    {
        if (e.IsCustom && e.Custom.IsExternal)
        {
            return e.Custom.ExternalNamespace;
        }
        return e.Namespace == "" ? _tn : (_tn + "." + e.Namespace);  // "global::" + 
    }
    public static string GetNamespace(Struct e)
    {
        if (e.IsExternal)
        {
            return e.ExternalNamespace;
        }
        return e.Namespace == "" ? _tn : (_tn + "." + e.Namespace);  // "global::" + 
    }

    #endregion



    public static bool HasDerivedPkgs(Template t, Struct s)
    {
        var list = GetDerivedPkgs(t, s);
        return list.Count > 0;
    }

    public static List<Struct> GetDerivedPkgs(Template t, Struct s)
    {
        var rtv = new List<Struct>();

        // 从 s 中找出 派生类( 递归 )， 放入 rtv
        rtv.Add(s);
        for (int i = 0; i < rtv.Count; i++)
        {
            rtv.AddRange(t.Structs.Where(a => a.BaseType == rtv[i]));
        }

        // 从 rtv 中移除自己和非 Package
        rtv.RemoveAll(a => !a.IsPackage);
        rtv.Remove(s);

        return rtv;
    }
}

