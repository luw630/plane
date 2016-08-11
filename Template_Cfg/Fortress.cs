#pragma warning disable 0169, 0414
using TemplateLibrary;

class Fortress
{
    [Compress, Title("堡垒id"), Desc("堡垒Id")]
    int id;

    [Compress, Title("是否是建筑"), Desc("在计算伤害的时候，是否按照建筑伤害来算")]
    bool isBuilding;

    [Compress, Title("生命值"), Desc("初始生命值")]
    int hp;

    [Compress, Title("移动速度"), Desc("移动速度 像素/秒")]
    int moveSpeed = 50;

    [Compress, Title("开始移动时间"), Desc("从游戏开始后，经过这么多 毫秒才开始移动堡垒")]
    int startMoveTime;

    [Compress, Title("首次攻击时间"), Desc("从游戏开始后，经过这么多 毫秒进行第一次攻击")]
    int firstAttackTime;

    [Compress, Title("攻击间隔"), Desc("从首次攻击后开始算，下次攻击的间隔, 单位毫秒")]
    int atkInterval;

    [Compress, Title("禁足时间"), Desc("每次攻击后，有一段时间是不能移动的, 单位毫秒")]
    int restrictionTime;

    [Compress, Title("攻击伤害"), Desc("攻击伤害")]
    int damage;

    [Compress, Title("建筑伤害比例"), Desc("建筑伤害比例")]
    int buildingDamageFactor;

    [Compress, Title("损毁血量"), Desc("当本方堡垒生命值低于该值，则不再发起攻击.")]
    int atkHpThreshold;

    [Compress, Title("停火血量"), Desc("当敌方堡垒生命值低于该值，则不再发起攻击.")]
    int ceasefireHpThreshold;

    [Compress, Title("击杀扣血"), Desc("当我方飞机被击杀时, 扣除我方堡垒生命值。扣除生命值等于 killPlaneLoss * 被击杀飞机的rank")]
    int killPlaneLoss = 50;

    [Compress, Title("地图边距"), Desc("堡垒产生时的地图边距")]
    int margins = 10;

    /****************************************************************************************/
    // client only
    /****************************************************************************************/

    [Title("名字"), Desc("堡垒名字")]
    string name;

    [Title("资源名"), Desc("资源名")]
    string resourceImg;


}
