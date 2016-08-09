#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

/******************************************************************************************************/
// 收发数据包区
/******************************************************************************************************/

// Client -> Server
namespace CS
{
    [Desc("连接成功后发送以进入游戏( 表示客户端已就绪 )")]
    class Join { }

    [Desc("退出游戏")]
    class Quit { }

    [Desc("飞机朝向")]
    class Rotate
    {
        float angle;
    }

    [Desc("移动")]
    class Move { }

    [Desc("停止移动")]
    class StopMove { }

    [Desc("开始放技能")]
    class Cast
    {
        [Compress, Desc("第几个( 不是 id )")]
        int index;
    }

    [Desc("停止放技能")]
    class StopCast
    {
        [Compress, Desc("第几个( 不是 id )")]
        int index;
    }

    [Desc("设置焦点( 如果超出最远距离 或对象无效 则取消 )")]
    class Aim
    {
        [Compress]
        int planeId = -1;
    }

    [Desc("取消焦点")]
    class CancelAim { }

    [Desc("进化飞机")]
    class Evolution
    {
        [Compress, Desc("第几个( 不是 id )")]
        int index;
    }

    [Desc("强化")]
    class Enhance
    {
        [Compress, Desc("第几个( 不是 id )")]
        int index;
    }
}
