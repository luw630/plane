using System;
using System.Collections.Generic;

public class Template
{
    public List<Struct> Structs = new List<Struct>();
    public List<string> Namespaces = new List<string>();
    public List<Project> Projects = new List<Project>();
    public List<Interface> Interfaces = new List<Interface>();
    public int MaxPackageId = 1;    // 指的是不属于任何项目的那一段
    public string Name = "";
}



public enum StructTypes
{
    Package,    // 包( 用于收发 )
    Struct,     // 结构体( 不直接收发 )
    Enum        // 枚举
}




public class Project
{
    public int ProjectId = 1;               // 包id前缀. 0 被用作全局的包
    public int PackageIdFrom;               // 当前以 500 累加
    public int PackageIdTo;                 // 当前以 500 累加
    public string Name = "";
    public string Desc = "";
    public int MaxPackageId = 1;   // 如果有固化 id. 则该值不一定准确
}


public class StructProject
{
    public Project Project;
    public TemplateLibrary.SendRecvTypes SendRecvType;
}




public enum SqlPlatforms
{
    MYSQL, MSSQL
}

public class Interface
{
    public string Namespace = "";
    public string Name = "";
    public string Desc = "";
    public SqlPlatforms SqlPlatform = SqlPlatforms.MYSQL;
    public List<Function> Functions = new List<Function>();
}

public class Function
{
    public string Name = "";
    public string Desc = "";
    public string Sql = "";
    public DataType ReturnType = new DataType();
    public List<Parameter> Parameters = new List<Parameter>();
}

public class Parameter
{
    public string Name = "";
    public string Desc = "";
    public DataType Type = new DataType();
    public object DefaultValue = null;
    public bool HasDefaultValue = true;
}


public class Struct
{
    // 公共部分
    public StructTypes StructType;
    public string Namespace = "";
    public string Name = "";
    public string Desc = "";
    public bool? Deprecated = null;
    public List<Member> Members = new List<Member>();
    public int RefCounter = 0;      // 被别的 Struct 引用的次数( 从大到小排可实现被引用的类前置 )
    public List<StructProject> Projects = new List<StructProject>();            // 包 于 项目中可收 或/和 发
    public bool IsValueType = false; // 以 struct 方式声明的类, 产生紧凑布局
    public bool IsExternal = false; // true: 外部结构，不再有 Members
    public string ExternalNamespace = "";
    public Struct BaseType = null;    // 基类

    public bool IsPackage
    {
        get
        {
            return StructType == StructTypes.Package;
        }
    }
    public bool IsStruct
    {
        get
        {
            return StructType == StructTypes.Struct;
        }
    }

    public bool IsEnum
    {
        get
        {
            return StructType == StructTypes.Enum;
        }
    }

    public void EnsureEnum()
    {
        if (StructType != StructTypes.Enum)
            throw new Exception("only Enum type have this property");
    }

    public void EnsurePackage()
    {
        if (StructType != StructTypes.Package)
            throw new Exception("only Package type have this property");
    }

    // 仅当 StructType 为 Enum 时有效, 包自增编号
    private int _PackageId = 0;
    public int PackageId
    {
        get
        {
            EnsurePackage();
            return _PackageId;
        }
        set
        {
            EnsurePackage();
            _PackageId = value;
        }
    }

    // 下面几个成员仅当 StructType 为 Enum 时有效
    private string _EnumUnderlyingType = "";
    public string EnumUnderlyingType
    {
        get
        {
            EnsureEnum();
            return _EnumUnderlyingType;
        }
        set
        {
            EnsureEnum();
            _EnumUnderlyingType = value;
        }
    }

    private int _EnumSize = 0;        // numBytes
    public int EnumSize
    {
        get
        {
            EnsureEnum();
            return _EnumSize;
        }
        set
        {
            EnsureEnum();
            _EnumSize = value;
        }
    }

    public bool EnumIsUnsigned
    {
        get
        {
            EnsureEnum();
            return _EnumUnderlyingType == "Byte"
                || _EnumUnderlyingType == "UInt16"
                || _EnumUnderlyingType == "UInt32"
                || _EnumUnderlyingType == "UInt64";
        }
    }
}

public class Member
{
    // 公共部分
    public Struct Parent = null;
    public string Name = "";
    public string Title = "";
    public string Desc = "";
    public bool Compress = true;
    public bool NoDirty = false;
    public bool PrimaryKey = false;
    public object DefaultValue = null;      // 默认值为空就是没设置. 当前不存在 HasDefaultValue 的说法 .
    public string NaNValue = null;
    public string InfinityValue = null;

    public DataType Type = new DataType();

    public int Ordinal = -1;

    // 仅当 Parent.StructType 为 Enum 时有效
    private object _EnumValue = 0;
    public object EnumValue
    {
        get
        {
            Parent.EnsureEnum();
            return _EnumValue;
        }
        set
        {
            Parent.EnsureEnum();
            _EnumValue = value;
        }
    }



    // 只针对 各式容器
    private int _MinLen = 0;
    public int MinLen
    {
        get
        {
            if (!Type.IsContainer)
                throw new Exception("is not a container");
            return _MinLen;
        }
        set
        {
            if (!Type.IsContainer)
                throw new Exception("is not a container");
            _MinLen = value;
        }
    }


    // 只针对 各式容器
    private int _MaxLen = 0;
    public int MaxLen
    {
        get
        {
            if (!Type.IsContainer)
                throw new Exception("is not a container");
            return _MaxLen;
        }
        set
        {
            if (!Type.IsContainer)
                throw new Exception("is not a container");
            _MaxLen = value;
        }
    }


}




public enum DataTypeCategories
{
    BuiltIn,    // 内置 各种整数浮点
    Custom,     // 用户类，枚举
    Generic,    // 泛型
    Array       // 数组
}

public class DataType
{
    public DataTypeCategories Category;
    // 这几个不针对 Custom
    public string Namespace = "";
    public string Name = "";
    public string Desc = "";

    #region 各种 Is

    public bool IsContainer
    {
        get
        {
            return IsArray || IsGeneric || IsBuiltIn && Namespace == "" && (Name == "String" || Name == "ByteBuffer");
        }
    }
    public bool IsEnum
    {
        get
        {
            return IsCustom && Custom.StructType == StructTypes.Enum;
        }
    }
    public bool IsPackage
    {
        get
        {
            return IsCustom && Custom.StructType == StructTypes.Package;
        }
    }
    public bool IsStruct
    {
        get
        {
            return IsCustom && Custom.StructType == StructTypes.Struct;
        }
    }

    public bool IsBuiltIn
    {
        get
        {
            return Category == DataTypeCategories.BuiltIn;
        }
    }
    public bool IsArray
    {
        get
        {
            return Category == DataTypeCategories.Array;
        }
    }
    public bool IsGeneric
    {
        get
        {
            return Category == DataTypeCategories.Generic;
        }
    }
    public bool IsCustom
    {
        get
        {
            return Category == DataTypeCategories.Custom;
        }
    }

    #endregion

    // 只针对 Custom 
    private Struct _Custom = null;
    public Struct Custom
    {
        get
        {
            if (Category != DataTypeCategories.Custom)
                throw new Exception("Category is not a Custom");
            return _Custom;
        }
        set
        {
            if (Category != DataTypeCategories.Custom)
                throw new Exception("Category is not a Custom");
            _Custom = value;
        }
    }

    // 只针对 Generic 或 Array
    private List<DataType> _ChildTypes = null;
    public bool Nullable = false;

    public List<DataType> ChildTypes
    {
        get
        {
            if (Category != DataTypeCategories.Array
                && Category != DataTypeCategories.Generic)
                throw new Exception("only Array Or Generic type have this property");
            if (_ChildTypes == null)
            {
                _ChildTypes = new List<DataType>();
            }
            return _ChildTypes;
        }
    }

    // 取 ChildTypes 的第 1 个
    public DataType ChildType
    {
        get
        {
            return ChildTypes[0];
        }
    }
}
