using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using LIB = TemplateLibrary;

public static class TemplateScaner
{
    public static int NumOfProjectPackages = 500;

    public static string libNS = "TemplateLibrary";                   // 重要：这个字串需要保持与模板 Lib 名一致
    public static string libNSdot = libNS + ".";

    public static Template GetTemplate(Assembly asm)
    {
        var template = new Template();

        // todo: ProjectType 标记相关的结构，有可能在实际使用中，漏填错填，造成生成后残缺不全。需要写检测排查代码

        // 扫枚举
        var r_enums = (from t in asm.GetTypes() where (t.IsEnum) select t).ToList();
        foreach (var r_enum in r_enums)
        {
            // 剔除掉标记为 ProjectTypes 的全局 enum
            if (r_enum.IsProjectTypes())
            {
                if (template.Projects.Count > 0)
                {
                    throw new Exception("存在两个或以上的 enum 都被标记了 [ProjectTypes].");
                }
                else
                {
                    var fs = r_enum.GetFields(BindingFlags.Static | BindingFlags.Public);
                    if (fs.Count() == 0)
                    {
                        throw new Exception("enum " + r_enum.FullName + " can't be empty");
                    }
                    foreach (var f in fs)
                    {
                        var proj = new Project();
                        proj.Name = f.Name;
                        proj.Desc = f.GetAttrDesc();
                        template.Projects.Add(proj);
                        var ev = (int)f.GetValue(null);
                        if (ev == 0)
                        {
                            throw new Exception("enum " + r_enum.FullName + "." + f.Name + " can't be 0");
                        }
                        proj.ProjectId = ev;
                        proj.PackageIdFrom = ev * NumOfProjectPackages;
                        proj.PackageIdTo = proj.PackageIdFrom + NumOfProjectPackages - 1;
                        // template.Projects.Count;   // 1 开始
                    }
                }
                //continue;
            }

            var e = new Struct();
            e.StructType = StructTypes.Enum;
            e.Namespace = r_enum.Namespace ?? "";
            e.Name = r_enum.Name.ToString();
            e.Desc = r_enum.GetAttrDesc();
            e.Deprecated = r_enum.GetAttrDeprecated();
            e.IsValueType = r_enum.IsValueType;
            var ut = r_enum.GetEnumUnderlyingType();
            e.EnumUnderlyingType = ut.Name;
            e.IsExternal = r_enum.IsExternal();
            if (e.IsExternal)
            {
                e.ExternalNamespace = r_enum.GetExternalNamespace();
            }

            var r_fields = r_enum.GetFields(BindingFlags.Static | BindingFlags.Public);
            if (e.IsExternal && r_fields.Count() > 0)
            {
                throw new Exception("External enum " + r_enum.FullName + " must be be empty");
            }
            if (r_fields.Count() == 0 && !e.IsExternal)
            {
                throw new Exception("enum " + r_enum.FullName + " can't be empty");
            }
            foreach (var r_field in r_fields)
            {
                var ef = new Member();
                ef.Parent = e;
                ef.Name = r_field.Name;
                ef.Desc = r_field.GetAttrDesc();
                ef.EnumValue = r_field.GetValue(null).ToIntegerString(ut);
                e.Members.Add(ef);
            }
            template.Structs.Add(e);
        }

        // 浅扫包和结构体
        // Members 在下面填, 需要先确保容器有所有类才方便引用
        var r_classes = from t in asm.GetTypes() where (t.IsClass || t.IsValueType && !t.IsEnum) && t.Namespace != libNS select t;  //  && !t.IsBaseOnly()
        foreach (var r_class in r_classes)
        {
            var c = new Struct();
            c.StructType = r_class.IsStruct() ? StructTypes.Struct : StructTypes.Package;
            c.Namespace = r_class.Namespace ?? "";
            c.Name = r_class.Name;
            c.Desc = r_class.GetAttrDesc();
            c.Deprecated = r_class.GetAttrDeprecated();
            c.IsValueType = r_class.IsValueType;
            c.IsExternal = r_class.IsExternal();
            if (c.IsExternal)
            {
                c.ExternalNamespace = r_class.GetExternalNamespace();
            }

            var ps = r_class.GetStructProjects(template);
            c.Projects = ps;

            if (c.IsPackage)
            {
                if (c.Projects.Count > 0)
                {
                    foreach (var p in c.Projects)
                    {
                        if (p.SendRecvType == TemplateLibrary.SendRecvTypes.RecvOnly
                            || p.SendRecvType == TemplateLibrary.SendRecvTypes.SendAndRecv)
                        {
                            c.PackageId = p.Project.PackageIdFrom + p.Project.MaxPackageId++;         // 填包自增
                            System.Diagnostics.Debug.Assert(c.PackageId <= p.Project.PackageIdTo);
                        }
                    }
                }
                else
                {
                    c.PackageId = template.MaxPackageId++;      // 填包自增
                    System.Diagnostics.Debug.Assert(c.PackageId < NumOfProjectPackages);
                }
            }
            template.Structs.Add(c);
        }

        // 继续扫包和结构体
        foreach (var r_class in r_classes)
        {
            // 创建 r_class 的实例 为了取成员的默认值.
            var o = asm.CreateInstance(r_class.FullName);

            // 定位到已填充的 struct
            var c = template.Structs.Find(a => a.Name == r_class.Name && a.Namespace == (r_class.Namespace ?? ""));

            // 扫继承字段( 展开到当前类 )
            var r_fields = r_class.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList();
            if (r_class.BaseType != typeof(object))
            {
                //r_fields.InsertRange(0, r_class.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                c.BaseType = template.Structs.Find(a => a.Name == r_class.BaseType.Name && a.Namespace == (r_class.BaseType.Namespace ?? ""));
            }

            if (c.IsExternal && r_fields.Count() > 0)
            {
                throw new Exception("External class " + r_class.FullName + " must be be empty");
            }

            // 扫字段
            foreach (var r_field in r_fields)
            {
                var f = new Member();
                f.Parent = c;
                f.Name = r_field.Name;
                f.Desc = r_field.GetAttrDesc();
                FillDataType(template, f.Type, r_field.FieldType);
                if (f.Type.IsContainer)
                {
                    var limit = r_field.GetAttrLimit();
                    f.MinLen = limit.Key;
                    f.MaxLen = limit.Value;
                }
                if (f.Type.IsArray
                    && (f.MinLen != f.MaxLen || f.MinLen == 0))
                {
                    throw new Exception(r_class.FullName + "." + r_field.Name + " need Limit( n )");
                }
                f.Compress = r_field.IsCompress();
                f.NoDirty = r_field.IsNoDirty();
                f.PrimaryKey = r_field.IsPrimaryKey();
                f.DefaultValue = r_field.GetValue(o);
                f.NaNValue = r_field.GetNan();
                f.InfinityValue = r_field.GetInfinity();
                f.Title = r_field.GetTitle();

                c.Members.Add(f);
            }
        }

        // 算 RefCounter
        var structs = template.Structs.Where(a => !a.IsEnum).ToList();
        foreach (var s in structs)
        {
            IncRefCounter(s);
        }


        // 整理命名空间
        template.Namespaces = template.Structs.Where(a => !a.IsExternal).Select(a => a.Namespace).Distinct().ToList();






        // 扫 interfaces, funcs, func parms
        var r_interfaces = (from t in asm.GetTypes() where (t.IsInterface) select t).ToList();
        foreach (var r_interface in r_interfaces)
        {
            var iface = new Interface
            {
                Namespace = r_interface.Namespace ?? "",
                Name = r_interface.Name,
                Desc = GetAttrDesc(r_interface),
                SqlPlatform = GetAttrSqlPlatform(r_interface),
            };
            template.Interfaces.Add(iface);

            var r_funcs = r_interface.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList();
            foreach (var r_func in r_funcs)
            {
                var func = new Function
                {
                    Name = r_func.Name,
                    Desc = GetAttrDesc(r_func),
                    Sql = GetAttrSql(r_func),
                };
                FillDataType(template, func.ReturnType, r_func.ReturnType);
                iface.Functions.Add(func);

                var r_parms = r_func.GetParameters();
                foreach (var r_parm in r_parms)
                {
                    var parm = new Parameter
                    {
                        Name = r_parm.Name,
                        Desc = GetAttrDesc(r_parm),
                        DefaultValue = r_parm.DefaultValue,
                        HasDefaultValue = r_parm.HasDefaultValue
                    };
                    FillDataType(template, parm.Type, r_parm.ParameterType);
                    func.Parameters.Add(parm);
                }
            }
        }




        return template;
    }


    public static void IncRefCounter(Struct s)
    {
        // todo: 递归引用检测
        foreach (var m in s.Members)
        {
            if (m.Type.IsStruct || m.Type.IsPackage)
            {
                m.Type.Custom.RefCounter++;
                IncRefCounter(m.Type.Custom);
            }
        }
    }

    public static string GetAttrDesc<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Desc)
                return ((LIB.Desc)r_attribute).Value;
        }
        return "";
    }

    public static SqlPlatforms GetAttrSqlPlatform<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.MSSQL)
                return SqlPlatforms.MSSQL;
            if (r_attribute is LIB.MYSQL)
                return SqlPlatforms.MYSQL;
        }
        return SqlPlatforms.MYSQL;
    }

    public static string GetAttrSql<T>(this T t) where T : ICustomAttributeProvider
    {
       
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Sql)
                return ((LIB.Sql)r_attribute).Value;
        }
        return "";
    }


    public static string GetNan<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.NaN)
                return ((LIB.NaN)r_attribute).Value;
        }
        return null;
    }

    public static string GetInfinity<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Infinity)
                return ((LIB.Infinity)r_attribute).Value;
        }
        return null;
    }

    public static string GetTitle<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Title)
                return ((LIB.Title)r_attribute).Value;
        }
        return "";
    }



    public static bool IsStruct<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Struct)
                return true;
        }
        return false;
    }
    public static bool IsBaseOnly<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.BaseOnly)
                return true;
        }
        return false;
    }
    public static bool IsCompress<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Compress)
                return true;
        }
        return false;
    }
    public static bool IsNoDirty<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.NoDirty)
                return true;
        }
        return false;
    }
    public static bool IsPrimaryKey<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.PrimaryKey)
                return true;
        }
        return false;
    }

    public static bool IsExternal<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.External)
                return true;
        }
        return false;
    }


    public static KeyValuePair<int, int> GetAttrLimit<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Limit)
            {
                return new KeyValuePair<int, int>(((LIB.Limit)r_attribute).Min, ((LIB.Limit)r_attribute).Max);
            }
        }
        return new KeyValuePair<int, int>(0, 0);
    }


    public static bool IsProjectTypes<T>(this T t) where T : Type
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.ProjectTypes)
            {
                var ns = t.Namespace;
                if (string.IsNullOrEmpty(ns))
                    return true;
                throw new Exception("[ProjectTypes] 必须标记在全局范围( 即不在任何 namespace 内的 )的 enum 头上");
            }
        }
        return false;
    }

    public static List<StructProject> GetStructProjects<MI>(this MI mi, Template t) where MI : ICustomAttributeProvider
    {
        var rtv = new List<StructProject>();
        foreach (var r_attribute in mi.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.ProjectType)
            {
                var att = ((LIB.ProjectType)r_attribute);
                rtv.Add(new StructProject
                {
                    Project = t.Projects.First(a => a.Name == att.Name),
                    SendRecvType = att.SendRecvType
                });
            }
            else if (r_attribute is LIB.From)
            {
                var att = ((LIB.From)r_attribute);
                rtv.Add(new StructProject
                {
                    Project = t.Projects.First(a => a.Name == att.Sender),
                    SendRecvType = LIB.SendRecvTypes.SendOnly
                });
            }
            else if (r_attribute is LIB.To)
            {
                var att = ((LIB.To)r_attribute);
                rtv.Add(new StructProject
                {
                    Project = t.Projects.First(a => a.Name == att.Receiver),
                    SendRecvType = LIB.SendRecvTypes.RecvOnly
                });
            }
            else if (r_attribute is LIB.FromTo)
            {
                var att = ((LIB.FromTo)r_attribute);
                rtv.Add(new StructProject
                {
                    Project = t.Projects.First(a => a.Name == att.Receiver),
                    SendRecvType = LIB.SendRecvTypes.RecvOnly
                });
                rtv.Add(new StructProject
                {
                    Project = t.Projects.First(a => a.Name == att.Sender),
                    SendRecvType = LIB.SendRecvTypes.SendOnly
                });
            }
        }
        return rtv;
    }


    public static bool? GetAttrDeprecated<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.Deprecated)
                return ((LIB.Deprecated)r_attribute).Error;
        }
        return null;
    }

    public static string GetExternalNamespace<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var r_attribute in t.GetCustomAttributes(false))
        {
            if (r_attribute is LIB.External)
                return ((LIB.External)r_attribute).Value;
        }
        return "";
    }



    // 转枚举值专用
    public static string ToIntegerString(this object enumValue, System.Type ut)
    {
        switch (ut.Name)
        {
        case "Byte":
            return Convert.ToByte(enumValue).ToString();
        case "SByte":
            return Convert.ToSByte(enumValue).ToString();
        case "UInt16":
            return Convert.ToUInt16(enumValue).ToString();
        case "Int16":
            return Convert.ToInt16(enumValue).ToString();
        case "UInt32":
            return Convert.ToUInt32(enumValue).ToString();
        case "Int32":
            return Convert.ToInt32(enumValue).ToString();
        case "UInt64":
            return Convert.ToUInt64(enumValue).ToString();
        case "Int64":
            return Convert.ToInt64(enumValue).ToString();
        }
        throw new Exception("unknown Underlying Type");
    }



    public static void FillDataType(Template template, DataType d, System.Type t)
    {
        var tn = t.Name;
        if (t.IsArray)
        {
            d.Category = DataTypeCategories.Array;
            var cd = new DataType();
            d.ChildTypes.Add(cd);
            FillDataType(template, cd, t.GetElementType());
        }
        else if (t.IsGenericType)
        {
            // 特例：Nullable`1
            if (t.Namespace == "System" && tn == "Nullable`1")
            {
                d.Category = DataTypeCategories.BuiltIn;
                d.Name = t.GenericTypeArguments[0].Name;
                d.Namespace = "";
                d.Nullable = true;
            }
            else
            {
                if (t.Namespace != libNS)
                    throw new Exception("unknown data type.");
                d.Category = DataTypeCategories.Generic;
                d.Name = tn.Substring(0, tn.LastIndexOf('`'));
                d.Namespace = "";
                foreach (var ct in t.GenericTypeArguments)
                {
                    var cd = new DataType();
                    d.ChildTypes.Add(cd);
                    FillDataType(template, cd, ct);
                }
            }
        }
        else if (t.Namespace == "System" || t.Namespace == libNS)
        {
            switch (t.Name)
            {
            case "Void":
            case "Byte":
            case "UInt16":
            case "UInt32":
            case "UInt64":
            case "SByte":
            case "Int16":
            case "Int32":
            case "Int64":
            case "Double":
            case "Single":
            case "Boolean":
            case "String":
            case "Literal":
            case "DateTime":
            case "ByteBuffer":
                d.Category = DataTypeCategories.BuiltIn;
                d.Name = t.Name;
                d.Namespace = "";
                break;

            default:
                throw new Exception("unknown data type.");
            }
        }
        else
        {
            d.Category = DataTypeCategories.Custom;
            d.Name = t.Name;
            d.Namespace = t.Namespace ?? "";
            d.Custom = template.Structs.Find(a => a.Name == t.Name && a.Namespace == d.Namespace);
        }
    }

}
