#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

[Desc("所有飞机的基类")]
class Plane
{
    [Compress, Desc("自增id")]
    int id = -1;

    [Compress, Desc("所属玩家id")]
    int playerId = -1;

    [Compress, Desc("配置id")]
    int cfgId;

    [Desc("中心点于 map 的当前坐标")]
    XY xy;

    [Desc("当前角度. 360度 转为 -1 ~ 1 的浮点")]
    float angle;

    [Desc("玩家旋转的目标角度( 这个会令 angle 值以每帧变化 角速度 来贴最近, 最终等于 )")]
    float angleTarget;

    [Compress, Desc("当前 / 剩余血量")]
    int hp;

    [Compress, Desc("当前 / 剩余护盾值")]
    int shield;

    [Desc("无敌/初生护盾 timer")]
    int bornShieldLifespanTicks;


    [Desc("护盾恢复每跳间隔 timer")]
    int shieldRecoverIntervalTicks;

    [Desc("护盾开始恢复间隔 timer. 挨打会重置")]
    int shieldBeginRecoverIntervalTicks;


    [Desc("是否正在前进")]
    bool moving = false;

    [Desc("所有技能的开关状态标志( 取代之前的 firing 之类 )")]
    List<bool> skillSwitches;

    [Compress, Desc("焦点飞机id( 受 aim 指令影响 )")]
    int targetId = -1;
}

// 下面是各种特化飞机
// 使用技能代表了 前摇 + 发射 + 后摇 + cd 一套流程. 前摇途中可能因失去目标而终止, 此时不算后续.  前摇 + 发射 + 后摇 会影响飞机朝向显示
// 放大招会打断本次小招. 

[Desc("普攻是 小激光 的飞机的基类")]
class Plane_Laser : Plane
{
    [Compress, Desc("开火时锁定的目标")]
    int fireTargetId = 0;

    [Compress, Desc("前摇 timer")]
    int fireBeforeTicks = 0;

    [Compress, Desc("后摇 timer")]
    int fireAfterTicks = 0;

    [Compress, Desc("冷却时间")]
    int fireCDTicks = 0;

    [Compress, Desc("本次开火被终止 但会继续计算 cd 时间")]
    bool fireCancel = false;
}

// 基础版本飞机.
class Plane0 : Plane_Laser
{
    
}

// 奶: 单激光, 回血
class Plane1 : Plane_Laser
{
    [Compress, Desc("大招技能CD")]
    int castCDTicks = 0;
}

// 攻: 单激光, 粗单激光
class Plane2 : Plane_Laser
{
    [Desc("状态标志")]
    bool casting;

    [Compress, Desc("大招技能CD")]
    int castCDTicks = 0;
}


// 坦: 单激光, 三连射( 因为放大会中断小招释放, 故这里小招的状态变量可复用 )
class Plane3 : Plane_Laser {
    [Compress, Desc("大招剩余连射次数")]
    int castLeftFireTimes;

    [Compress, Desc("大招技能CD")]
    int castCDTicks = 0;
}