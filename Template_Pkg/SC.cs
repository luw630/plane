#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

namespace PT
{
    // 外部引用
    [External("PT")]
    class Scene { }
}

namespace xxlib
{
    // 外部引用
    [External("xxlib")]
    class XY { }
}


// Server -> Client
namespace SC
{
    [Desc("单发给刚 join 的客户端( 先于 FullSync 下发 )")]
    class Join
    {
        [Compress, Desc("服务器分配给玩家的 id")]
        int selfId;
    }

    [Desc("广播给刚 Join 的这批玩家 整个场景的完整数据快照")]
    class FullSync
    {
        [Desc("服务器完整的场景数据, 含各种自增id, 随机数池, 当前ticks......")]
        PT.Scene scene;
    }

    [Desc("广播给 所有玩家 当前帧的行为同步( 于帧尾干这事, 对刚 Join 的人来说, 跟在 FullSync 后面发 )")]
    class EventSync
    {
        [Compress, Desc("服务器逻辑帧编号")]
        int ticks;

        [Compress, Desc("本帧加入游戏的 玩家个数")]
        int joins;

        [Compress, Desc("本帧退出游戏的 玩家id 列表")]
        List<int> quits;

        [Desc("本帧有改变方向行为的 列表")]
        List<Events.Rotate> rotates;

        [Compress, Desc("本帧开始放技能的 列表")]
        List<Events.PlayerAction> casts;

        [Compress, Desc("本帧停止放技能的 列表")]
        List<Events.PlayerAction> stopCasts;

        [Compress, Desc("本帧开始移动的 玩家id 列表")]
        List<int> moves;

        [Compress, Desc("本帧停止移动的 玩家id 列表")]
        List<int> stopMoves;

        [Desc("本帧发起焦点锁定的 玩家id + 目标id 列表")]
        List<Events.Aim> aims;

        [Compress, Desc("本帧产生强化行为的 列表")]
        List<Events.PlayerAction> enhances;

        [Compress, Desc("本帧进阶飞机的 列表")]
        List<Events.PlayerAction> evolutions;

        [Desc("附加的调试信息。当前用于放置 FullSync 以便于 Client 做差异对比")]
        ByteBuffer debugInfo;
    }


    namespace Events
    {
        [Struct]
        class Rotate
        {
            [Compress]
            int playerId;
            float angle;
            XY xyIncBase;
        }

        [Struct]
        class Aim
        {
            [Compress]
            int playerId;
            [Compress]
            int planeId;
        }

        [Struct]
        class PlayerAction
        {
            [Compress]
            int playerId;
            [Compress]
            int index;
        }

    }
}
