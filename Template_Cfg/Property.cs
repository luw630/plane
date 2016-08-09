#pragma warning disable 0169, 0414
using TemplateLibrary;

enum PropertyTypes : byte
{
    [Desc("攻击方式")]
    atkType = 0,
    [Desc("每一发的数量")]
    numOnce = 1,
    [Desc("连射数量")]
    numTotal = 2,
    [Desc("连射间隔, 单位毫秒")]
    interval = 3,
    [Desc("子弹速度, 单位 像素/秒")]
    speed = 4,
    [Desc("攻击降速, 开火时飞机的速度应该为 planeSpeed * (1 + (atkMoveSpdScaling * 0.01))")]
    atkMoveSpdScaling = 5,
    [Desc("攻击前摇, 单位毫秒")]
    startDelay = 6,
    [Desc("攻击后摇, 单位毫秒")]
    endDelay = 7,
    [Desc("伤害值")]
    damage = 8,
    [Desc("子弹最大射程")]
    range = 9,
    [Desc("子弹半径, 用于检测是否命中")]
    bulletRadius = 10,
    [Desc("建筑伤害比例， 对建筑造成的最终伤害 = (damage - ar) * (buildingDamageFactor * 0.01)")]
    buildingDamageFactor = 11,
    [Desc("发射时是否面向目标")]
    toward = 12,
    [Desc("0表示会被阻挡，1表示不能被阻挡，2表示有穿透效果(不仅不被阻挡，还会对沿途目标造成伤害)")]
    penetration = 13,
    [Desc("冷却时间")]
    cd = 14,
    [Desc("能量消耗")]
    consumption = 15,
    [Desc("作用区域")]
    area = 16,
    [Desc("是否无敌")]
    invincible = 17,
    [Desc("无敌时间")]
    invincibleTime = 18,
    [Desc("生命恢复值")]
    restore = 19,
    [Desc("生命恢复比例")]
    restorePercent = 20,
    [Desc("表 SkillOffset中查找内容")]
    offsets = 21,
    [Desc("反隐间隔时间")]
    detectInterval = 22,
    [Desc("反隐范围")]
    detectRange = 23,

    [Desc("增加生命值")]
    extraHp = 50,
    [Desc("增加护甲")]
    extraArmor = 51,
    [Desc("增加护盾")]
    extraShield = 52,
    [Desc("增加飞行速度")]
    extraSpeed = 53,
    [Desc("增加攻击范围")]
    extraAtkRange = 54,
    [Desc("普通攻击时间缩短")]
    atkTimeFactor = 55,
    [Desc("能量自增")]
    energyAutoIncrement = 56,
    [Desc("收集能量系数")]
    energyCatchFactor = 57,
    [Desc("攻击力提高")]
    fireExtraDamage = 58,
    [Desc("技能攻击提高")]
    skillExtraDamage = 59
}

[Struct]
class Property
{
    PropertyTypes type;
    [Compress]
    int value;
}
