#pragma warning disable 0169, 0414
using TemplateLibrary;

class Plane
{
    [Compress, Title("id"), Desc("飞机Id")]
    int id;

    /******************************************************************************************/
    // 血量计算相关
    /******************************************************************************************/

    [Compress, Title("血量"), Desc("基础生命值")]
    int hp;

    [Compress, Title("护甲"), Desc("基础护甲, 于 hp 的伤害值等于 攻击力 - 护甲，最小是1")]
    int armor;

    [Compress, Title("护盾"), Desc("基础护盾, 护盾是真实伤害，不考虑护甲( 先于 hp 扣除 )")]
    int shield;

    [Compress, Title("护盾回复每跳间隔时长"), Desc("护盾回复每跳间隔时长,每回复 1点护盾需要的时间,单位毫秒")]
    int shieldRecoverInterval;

    [Compress, Title("护盾回复间隔"), Desc("护盾回复间隔,飞机在间隔时间内未受到攻击,才开始回复护盾,单位毫秒")]
    int shieldBeginRecoverInterval;

    [Compress, Title("是否为建筑"), Desc("是否为建筑护甲. 某些技能针对此属性有不一样的效果")]
    bool isBuilding;

    [Compress, Title("阶级"), Desc("飞机的阶级 ****** 听说本方的飞机被击杀后，堡垒要扣血，扣血量就是  击杀扣血 * 被击杀飞机的阶级")]
    int rank;

    /******************************************************************************************/
    // 移动 / 半径相关
    /******************************************************************************************/

    [Compress, Title("飞行速度"), Desc("飞行速度,单位 像素/秒")]
    int moveSpeed;

    [Compress, Title("收集资源半径"), Desc("收集半径,能源与飞机在半径范围内则被吸收")]
    int colletRadius;

    [Compress, Title("机体碰撞半径"), Desc("机体碰撞半径")]
    int collisionRadius;

    [Compress, Title("受击半径"), Desc("受击半径")]
    int strikeRadius;

    [Compress, Title("角速度"), Desc("角速度 每秒旋转的角度 360度标准")]
    int angleLimit;

    /******************************************************************************************/
    // 技能 / 发射 / 焦点相关
    /******************************************************************************************/

    [Compress, Title("目标侦测距离"), Desc("焦点侦测距离")]
    int targetDetectDistance;

    [Compress, Title("目标侦测角度"), Desc("焦点侦测扇形角度, 范围0~360, 是扇形圆心角度的一半")]
    int targetDetectAngle;

    [Compress, Title("发射偏移坐标表"), Limit(1, 5), Desc("炮管位置偏移坐标")]
    List<Point> shootOffsets;

    [Compress, Title("技能id表"), Limit(2, 3), Desc("指向 Skill 表的行. [0]是普通攻击. [1]是主动技, [2]是被动技(反隐啥的)")]
    List<int> skillIds;

    /******************************************************************************************/
    // 进阶 / 强化相关
    /******************************************************************************************/

    [Compress, Title("进阶飞机id表"), Limit(0, 3), Desc("能进阶到目标飞机Id的列表. 就在 plane 表中查. 不能进阶则该表为空")]
    List<int> evolutionIds;

    [Compress, Title("进阶消耗"), Desc("进阶需要的总能量( 实际开支需要减去 强化所花掉的总能量 )")]
    int evolutionEnergyCost;

    [Compress, Title("强化消耗表"), Limit(4), Desc("每一级的强化能量消耗值. 以某部分当前强化级别作为下标可查. 越界则表示无法继续强化")]
    List<int> enhanceEnergyCosts;

    [Compress, Title("强化效果id表"), Limit(3), Desc("指向 Enhance 表的行. 当前案子中的可强化部位是 3 个")]
    List<int> enhanceIds;

    /******************************************************************************************/
    // 初生 / 死亡相关
    /******************************************************************************************/

    [Compress, Title("初生无敌盾寿命"), Desc("初生无敌盾寿命")]
    int bornShieldLifespan;

    [Compress, Title("初生等待时长"), Desc("初生等待时长")]
    int bornInterval;


    /******************************************************************************************/
    // 前端专用
    /******************************************************************************************/

    [Title("name"), Desc("前端 飞机名称")]
    string name;

    [Compress, Title("视野范围"), Desc("前端 摄像机视野范围")]
    int vision;
}
