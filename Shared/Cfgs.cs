using System;
using System.Collections.Generic;
using System.Text;

// 放置当前配置的容器( 放置于 scene 中. 逐级传递 )
public class Cfgs
{
    // 临时搞搞 先这样
    //public const string serverIP = "svn.yushengame.com:11123";
    public const string serverIP = "127.0.0.1:11123";

    /// <summary>
    /// 设定的每秒多少帧逻辑
    /// </summary>
    public const int numLogicFramesPerSecond = 25;

    /// <summary>
    /// 每帧经历多少 milliseconds
    /// </summary>
    public const int logicFrameMS = 1000 / numLogicFramesPerSecond;

    /// <summary>
    /// 以间隔 ms 换算出来的实际每秒多少帧
    /// </summary>
    public const int logicFrameTicksPerSeconds = 1000 / logicFrameMS;

    public List<Cfg.Map> maps = new List<Cfg.Map>();
    public List<Cfg.Gas> gass = new List<Cfg.Gas>();

    public List<Cfg.Plane> planes = new List<Cfg.Plane>();
    public List<Cfg.Skill> skills = new List<Cfg.Skill>();
    public List<Cfg.Enhance> enhances = new List<Cfg.Enhance>();
}

namespace Cfg
{
    partial class Skill
    {
        public int GetValue(PropertyTypes t)
        {
            return properties.Find(a => a.type == t).value;
        }
    }

    partial class Enhance
    {
        public int GetValue(PropertyTypes t)
        {
            return properties.Find(a => a.type == t).value;
        }
    }

}
