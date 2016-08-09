using System;
namespace Cfg
{
    public enum PropertyTypes : byte
    {
        /// <summary>
        /// 攻击方式
        /// </summary>
        atkType = 0, 
        /// <summary>
        /// 每一发的数量
        /// </summary>
        numOnce = 1, 
        /// <summary>
        /// 连射数量
        /// </summary>
        numTotal = 2, 
        /// <summary>
        /// 连射间隔, 单位毫秒
        /// </summary>
        interval = 3, 
        /// <summary>
        /// 子弹速度, 单位 像素/秒
        /// </summary>
        speed = 4, 
        /// <summary>
        /// 攻击降速, 开火时飞机的速度应该为 planeSpeed * (1 + (atkMoveSpdScaling * 0.01))
        /// </summary>
        atkMoveSpdScaling = 5, 
        /// <summary>
        /// 攻击前摇, 单位毫秒
        /// </summary>
        startDelay = 6, 
        /// <summary>
        /// 攻击后摇, 单位毫秒
        /// </summary>
        endDelay = 7, 
        /// <summary>
        /// 伤害值
        /// </summary>
        damage = 8, 
        /// <summary>
        /// 子弹最大射程
        /// </summary>
        range = 9, 
        /// <summary>
        /// 子弹半径, 用于检测是否命中
        /// </summary>
        bulletRadius = 10, 
        /// <summary>
        /// 建筑伤害比例， 对建筑造成的最终伤害 = (damage - ar) * (buildingDamageFactor * 0.01)
        /// </summary>
        buildingDamageFactor = 11, 
        /// <summary>
        /// 发射时是否面向目标
        /// </summary>
        toward = 12, 
        /// <summary>
        /// 0表示会被阻挡，1表示不能被阻挡，2表示有穿透效果(不仅不被阻挡，还会对沿途目标造成伤害)
        /// </summary>
        penetration = 13, 
        /// <summary>
        /// 冷却时间
        /// </summary>
        cd = 14, 
        /// <summary>
        /// 能量消耗
        /// </summary>
        consumption = 15, 
        /// <summary>
        /// 作用区域
        /// </summary>
        area = 16, 
        /// <summary>
        /// 是否无敌
        /// </summary>
        invincible = 17, 
        /// <summary>
        /// 无敌时间
        /// </summary>
        invincibleTime = 18, 
        /// <summary>
        /// 生命恢复值
        /// </summary>
        restore = 19, 
        /// <summary>
        /// 生命恢复比例
        /// </summary>
        restorePercent = 20, 
        /// <summary>
        /// 表 SkillOffset中查找内容
        /// </summary>
        offsets = 21, 
        /// <summary>
        /// 反隐间隔时间
        /// </summary>
        detectInterval = 22, 
        /// <summary>
        /// 反隐范围
        /// </summary>
        detectRange = 23, 
        /// <summary>
        /// 增加生命值
        /// </summary>
        extraHp = 50, 
        /// <summary>
        /// 增加护甲
        /// </summary>
        extraArmor = 51, 
        /// <summary>
        /// 增加护盾
        /// </summary>
        extraShield = 52, 
        /// <summary>
        /// 增加飞行速度
        /// </summary>
        extraSpeed = 53, 
        /// <summary>
        /// 增加攻击范围
        /// </summary>
        extraAtkRange = 54, 
        /// <summary>
        /// 普通攻击时间缩短
        /// </summary>
        atkTimeFactor = 55, 
        /// <summary>
        /// 能量自增
        /// </summary>
        energyAutoIncrement = 56, 
        /// <summary>
        /// 收集能量系数
        /// </summary>
        energyCatchFactor = 57, 
        /// <summary>
        /// 攻击力提高
        /// </summary>
        fireExtraDamage = 58, 
        /// <summary>
        /// 技能攻击提高
        /// </summary>
        skillExtraDamage = 59, 
    }
    public enum AtkType : byte
    {
        laser = 1, 
        bullet = 2, 
    }
}
