#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;


// 本周(2016.8.8)计划: 基地( 先以圆形移动吧 好算角速度 ). 
// 状态完整, 即: 先停个时间, 然后开始移动, 首次发射, 射后僵直, 移动, 再射, 再僵直, 再移, 如此循环.  射之行为会受自己的血和对方的血影响
// 基地有血, 可以被飞机和敌方基地发射的子弹击中.
// 另外, 飞机再生, 从基地中心点出现.



enum FortressStates : byte
{ 
    WaitMove,
    Move,
    Stop,
}

[Desc("堡垒")]
class Fortress
{
    [Compress, Desc("配置 id")]
    int cfgId;

    [Compress, Desc("首动等待")]
    int startMoveTimeTicks;

    [Compress, Desc("首射等待( 首动之后开始算 )")]
    int firstAttackTimeTicks;

    [Compress, Desc("射之间隔( 僵直完后开始算 )")]
    int atkIntervalTicks;

    [Compress, Desc("射后僵直( 射后开始算 )")]
    int restrictionTimeTicks;

    // todo: 位置，角度, 状态
}
