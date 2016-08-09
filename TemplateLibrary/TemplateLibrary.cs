/********************************************************/
// 关键字
/********************************************************/

// enum                                     // 枚举
// class                                    // 自定义数据类型
// interface                                // sql 查询函数集

/********************************************************/
// 标记
/********************************************************/

// Title( "字段名" )                        // 用于导出为 excel 时的列名生成
// Desc( "注释" )                           // 用于类 及类成员
// Limit( [定长] | [ 最小长, 最大长 ] )     // 用于 string, 数组, 集合  还原时的长度安全限制
// Struct                                   // 用于标记某个 包 属于纯结构体( 不会直接用于收发 )
// BaseOnly                                 // 用于标记某个 模板类 纯用作被展开继承( 不属于包或结构体 )
// Compress                                 // 用于标记某成员 "使用变长存储". 仅作用于 32/64 位整型成员或 double
// Deprecated                               // 禁用标志( 还是会生成，但是可能会带 否决标记 )

// ProjectTypes                             // 用于标记某个 "位于全局范围的enum" 为 项目枚举
// ProjectType                              // 用于标记某个 包 属于某个项目( 可配置多个项目复用 ), 参数就是 ProjectTypes 所标注的 enum
// From, To, FromTo                         // ProjectType 的简化 / 复合版

// External( "path / namespace 啥的 " )     // 对标记为这个的 枚举或结构体，属于外部引用，不再填充或生成

// NaN, Infinity                            // 标识 float 或 double 成员是否可以解码出这些值, 默认构造函数为 抛异常. 传值为替换值




/********************************************************/
// 可用 数据类型:
/********************************************************/

// byte  
// ushort
// uint  
// ulong 
// sbyte 
// short 
// int   
// long  
// double
// float 
// bool
// string
// Literal                          // 对应无需转义的 string 参数
// DateTime
// ByteBuffer                       // 对应 byte[], 数据库的 blob
// T[]
// List<T>




/********************************************************/
// 跨语言数据类型映射表
/********************************************************/

//   c#/模板            |  c++                |  java               |  lua5.3
//----------------------+---------------------+---------------------+----------------
//   byte               |  byte               |  byte               |  double
//   ushort             |  ushort             |  short              |  double
//   uint               |  uint               |  int                |  double
//   ulong              |  uint64             |  long               |  int64
//   sbyte              |  sbyte              |  byte               |  double
//   short              |  short              |  short              |  double
//   int                |  int                |  int                |  double
//   long               |  int64              |  long               |  int64
//   double             |  double             |  double             |  double
//   float              |  float              |  float              |  double
//   bool               |  bool               |  boolean            |  bool
//   DateTime           |  xxlib::SimpleDate  |  Util.Date          |  string
//   byte[]/ByteBuffer  |  xxlib::ByteBuffer  |  byte[]/ByteBuffer  |  string
//   string             |  xxlib::String      |  string             |  string
//   List               |  xxlib::List        |  List<引用版>       |  table



namespace TemplateLibrary
{
    /// <summary>
    /// 用以描述一个可变长的集合。用于参数时将拼接为 ( 1, 2, 3 ... )。带括号。 不可空不可没有元素。
    /// </summary>
    public class List<V> { }

    /// <summary>
    /// BLOB 数据
    /// </summary>
    public class ByteBuffer { }

    /// <summary>
    /// 原始的不转义的字符串
    /// </summary>
    public class Literal { }


    /// <summary>
    /// 数据库相关，List 的别名。区别在于 List 将生成 ByteArray, 而 DbList 将视作多个对象
    /// </summary>
    public class DbList<V> { }


    /// <summary>
    /// 替代 System.DateTime
    /// </summary>
    public class DateTime { }

    // more data type here ...



    /// <summary>
    /// SQL 语句。可用于接口成员函数
    /// </summary>
    public class Sql : System.Attribute
    {
        public string Value;
        public Sql(string v) { Value = v; }
    }


    /// <summary>
    /// 标记在 interface 上，以声明该组函数是针对什么数据库平台( ms sql server )
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Interface)]
    public class MSSQL : System.Attribute
    {
    }
    /// <summary>
    /// 标记在 interface 上，以声明该组函数是针对什么数据库平台( mysql )
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Interface)]
    public class MYSQL : System.Attribute
    {
    }


    /// <summary>
    /// 备注。可用于类/枚举/函数 及其 成员
    /// </summary>
    public class Desc : System.Attribute
    {
        public Desc(string v) { Value = v; }
        public string Value;
    }


    /// <summary>
    /// 用于导出为 excel 时的列名生成
    /// </summary>
    public class Title : System.Attribute
    {
        public Title(string v) { Value = v; }
        public string Value;
    }
    

    /// <summary>
    /// 外部扩展。内容通常是 namespace。以 . 间隔的。
    /// </summary>
    public class External : System.Attribute
    {
        public External(string v) { Value = v; }
        public string Value;
    }


    /// <summary>
    /// 存储数据用的结构体. 生成时将不作为 "包" 对待
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class Struct : System.Attribute
    {
    }

    /// <summary>
    /// 用于标记某个 模板类 纯用作被展开继承( 不属于包或结构体 )
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class BaseOnly : System.Attribute
    {
    }

    /// <summary>
    /// 标记 32-64 位整型变量, double  传输时不压缩( 默认压缩 )
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class Compress : System.Attribute
    {
    }


    /// <summary>
    /// 标记当反列化 float 时, 如果值是 nan, 就设成 Value
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class NaN : System.Attribute
    {
        public string Value;
        public NaN(float v)
        {
            if(float.IsNaN(v)||float.IsInfinity(v))
            {
                throw new System.Exception("?????????");
            }
            Value = v.ToString() + "f";
        }
        public NaN()
        {
            Value = "";
        }
    }
    /// <summary>
    /// 标记当反列化 float 时, 如果值是 infinity, 就设成 Value
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class Infinity : System.Attribute
    {
        public string Value;
        public Infinity(float v)
        {
            if (float.IsNaN(v) || float.IsInfinity(v))
            {
                throw new System.Exception("?????????");
            }
            Value = v.ToString() + "f";
        }
        public Infinity()
        {
            Value = "";
        }
    }



    /// <summary>
    /// 针对全局位置的特殊 enum 标记，以说明该 enum 为“项目分类”，并应用于 ProjectType 特性当中
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Enum)]
    public class ProjectTypes : System.Attribute { }


    /// <summary>
    /// 针对 enum, 包/结构体 标记，以限定“项目分类”。参数为 ProjectTypes 限定的 enum 项
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Enum | System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class ProjectType : System.Attribute
    {
        public string Name = "";
        public SendRecvTypes SendRecvType = SendRecvTypes.SendAndRecv;
        public ProjectType(object pt)
        {
            System.Diagnostics.Debug.Assert(pt.GetType().IsEnum);
            Name = pt.ToString();
        }
    }


    /// <summary>
    /// 针对最外层级的 数组, byte[], string 做长度限制。单个长度值为定长
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.ReturnValue | System.AttributeTargets.ReturnValue)]
    public class Limit : System.Attribute
    {
        public Limit(int fix)
        {
            Min = fix;
            Max = fix;
        }
        public Limit(int min, int max)
        {
            if (max < min)
                throw new System.Exception("error: max < min");
            Min = min;
            Max = max;
        }
        public int Min;
        public int Max;
    }

    /// <summary>
    /// 收发限定
    /// </summary>
    public enum SendRecvTypes
    {
        SendAndRecv,
        SendOnly,
        RecvOnly,
    }
    /// <summary>
    ///  针对 enum, 包/结构体 标记，会根据语种生成 否决 相关代码或标记或注释
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Enum | System.AttributeTargets.Class | System.AttributeTargets.Struct | System.AttributeTargets.Field)]
    public class Deprecated : System.Attribute
    {
        public bool Error = false;
        public Deprecated() { }
        public Deprecated(bool isError)
        {
            Error = isError;
        }

    }


    // 下面是 ProjectType 的三种特化版本

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class FromTo : System.Attribute
    {
        public string Sender { get; private set; }
        public string Receiver { get; private set; }

        public FromTo(object sender, object receiver)
        {
            System.Diagnostics.Debug.Assert(sender.GetType().IsEnum);
            System.Diagnostics.Debug.Assert(receiver.GetType().IsEnum);
            System.Diagnostics.Debug.Assert(sender != receiver);
            Sender = sender.ToString();
            Receiver = receiver.ToString();
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class From : System.Attribute
    {
        public string Sender { get; private set; }

        public From(object sender)
        {
            System.Diagnostics.Debug.Assert(sender.GetType().IsEnum);
            Sender = sender.ToString();
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class To : System.Attribute
    {
        public string Receiver { get; private set; }

        public To(object receiver)
        {
            System.Diagnostics.Debug.Assert(receiver.GetType().IsEnum);
            Receiver = receiver.ToString();
        }
    }





    /// <summary>
    /// 属性变化不标脏
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class NoDirty : System.Attribute
    {
    }

    /// <summary>
    /// _id 属性
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class PrimaryKey : System.Attribute
    {
    }
    


    // more attribute here ...
}


