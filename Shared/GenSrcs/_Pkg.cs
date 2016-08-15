using System;
using System.Collections.Generic;
using xxlib;
namespace Pkg
{
namespace CS
{
    /// <summary>
    /// 连接成功后发送以进入游戏( 表示客户端已就绪 )
    /// </summary>
    public partial class Join
    {
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public partial class Quit
    {
    }
    /// <summary>
    /// 飞机朝向
    /// </summary>
    public partial class Rotate
    {
        public float angle = 0;
    }
    /// <summary>
    /// 移动
    /// </summary>
    public partial class Move
    {
    }
    /// <summary>
    /// 停止移动
    /// </summary>
    public partial class StopMove
    {
    }
    /// <summary>
    /// 开始放技能
    /// </summary>
    public partial class Cast
    {
        /// <summary>
        /// 第几个( 不是 id )
        /// </summary>
        public int index = 0;
    }
    /// <summary>
    /// 停止放技能
    /// </summary>
    public partial class StopCast
    {
        /// <summary>
        /// 第几个( 不是 id )
        /// </summary>
        public int index = 0;
    }
    /// <summary>
    /// 设置焦点( 如果超出最远距离 或对象无效 则取消 )
    /// </summary>
    public partial class Aim
    {
        public int planeId = -1;
    }
    /// <summary>
    /// 取消焦点
    /// </summary>
    public partial class CancelAim
    {
    }
    /// <summary>
    /// 进化飞机
    /// </summary>
    public partial class Evolution
    {
        /// <summary>
        /// 第几个( 不是 id )
        /// </summary>
        public int index = 0;
    }
    /// <summary>
    /// 强化
    /// </summary>
    public partial class Enhance
    {
        /// <summary>
        /// 第几个( 不是 id )
        /// </summary>
        public int index = 0;
    }
}
namespace SC
{
    /// <summary>
    /// 单发给刚 join 的客户端( 先于 FullSync 下发 )
    /// </summary>
    public partial class Join
    {
        /// <summary>
        /// 服务器分配给玩家的 id
        /// </summary>
        public int selfId = 0;
    }
    /// <summary>
    /// 广播给刚 Join 的这批玩家 整个场景的完整数据快照
    /// </summary>
    public partial class FullSync
    {
        /// <summary>
        /// 服务器完整的场景数据, 含各种自增id, 随机数池, 当前ticks......
        /// </summary>
        public PT.Scene scene = new PT.Scene();

    }
    /// <summary>
    /// 广播给 所有玩家 当前帧的行为同步( 于帧尾干这事, 对刚 Join 的人来说, 跟在 FullSync 后面发 )
    /// </summary>
    public partial class EventSync
    {
        /// <summary>
        /// 服务器逻辑帧编号
        /// </summary>
        public int ticks = 0;
        /// <summary>
        /// 本帧加入游戏的 玩家个数
        /// </summary>
        public int joins = 0;
        /// <summary>
        /// 本帧退出游戏的 玩家id 列表
        /// </summary>
        public List<int> quits = new List<int>();
        /// <summary>
        /// 本帧有改变方向行为的 列表
        /// </summary>
        public List<Pkg.SC.Events.Rotate> rotates = new List<Pkg.SC.Events.Rotate>();
        /// <summary>
        /// 本帧开始放技能的 列表
        /// </summary>
        public List<Pkg.SC.Events.PlayerAction> casts = new List<Pkg.SC.Events.PlayerAction>();
        /// <summary>
        /// 本帧停止放技能的 列表
        /// </summary>
        public List<Pkg.SC.Events.PlayerAction> stopCasts = new List<Pkg.SC.Events.PlayerAction>();
        /// <summary>
        /// 本帧开始移动的 玩家id 列表
        /// </summary>
        public List<int> moves = new List<int>();
        /// <summary>
        /// 本帧停止移动的 玩家id 列表
        /// </summary>
        public List<int> stopMoves = new List<int>();
        /// <summary>
        /// 本帧发起焦点锁定的 玩家id + 目标id 列表
        /// </summary>
        public List<Pkg.SC.Events.Aim> aims = new List<Pkg.SC.Events.Aim>();
        /// <summary>
        /// 本帧产生强化行为的 列表
        /// </summary>
        public List<Pkg.SC.Events.PlayerAction> enhances = new List<Pkg.SC.Events.PlayerAction>();
        /// <summary>
        /// 本帧进阶飞机的 列表
        /// </summary>
        public List<Pkg.SC.Events.PlayerAction> evolutions = new List<Pkg.SC.Events.PlayerAction>();
        /// <summary>
        /// 附加的调试信息。当前用于放置 FullSync 以便于 Client 做差异对比
        /// </summary>
        public ByteBuffer debugInfo = new ByteBuffer();
    }
}
namespace SC.Events
{
    public partial class Rotate
    {
        public int playerId = 0;
        public float angle = 0;
        public xxlib.XY xyIncBase = new xxlib.XY();
    }
    public partial class Aim
    {
        public int playerId = 0;
        public int planeId = 0;
    }
    public partial class PlayerAction
    {
        public int playerId = 0;
        public int index = 0;
    }
}
namespace Sim
{
    public partial class Connect
    {
    }
    public partial class Disconnect
    {
    }
    public partial class Rotate
    {
        public float angle = 0;
        public xxlib.XY xyIncBase = new xxlib.XY();
    }
}
}
