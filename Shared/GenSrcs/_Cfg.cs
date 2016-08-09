using System;
using System.Collections.Generic;
using xxlib;
namespace Cfg
{
    public partial class Map
    {
        /// <summary>
        /// 地图id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 地图大小
        /// </summary>
        public Size size;
        /// <summary>
        /// 地图中气体将被分割成 weight * height 个大格子区域
        /// </summary>
        public Size gasAreaSize;
        /// <summary>
        /// 初始气体数量范围( 包含值本身 )
        /// </summary>
        public Range gasNumInit;
        /// <summary>
        /// 气体检测间隔时间: 按气体分区的一个随机序列来遍历检测, 每换一个区, 要等待这个间隔时间
        /// </summary>
        public int gasScanInterval = 0;
        /// <summary>
        /// 气体检测最低阈值，低于这个值就需要补充
        /// </summary>
        public int gasScanThreshold = 0;
        /// <summary>
        /// 补充生成时每生成一个 gas 的 时间间隔. 通常非常短
        /// </summary>
        public int gasSupplementInterval = 0;
        /// <summary>
        /// 补充数量范围值. 随机得到之后, 将每隔 补充间隔 时间生成一个, 直到达到该值为止
        /// </summary>
        public Range gasNumSupplement;
        /// <summary>
        /// 气体种类生成权重表. 以 sum(weight) 值为分母, 按这个比例来补充生成 gas
        /// </summary>
        public List<GasWeight> gasWeights = new List<GasWeight>();
        /// <summary>
        /// 堡垒移动方式: 0-不移动, 1-椭圆轨迹,......待定
        /// </summary>
        public int fortressMoveType = 0;
        /// <summary>
        /// 椭圆有两个焦点，圆只有一个
        /// </summary>
        public List<Point> focusPoints = new List<Point>();
        /// <summary>
        /// 半径
        /// </summary>
        public int radius = 0;
        /// <summary>
        /// 堡垒Id
        /// </summary>
        public int fortressId = 0;
        /// <summary>
        /// 气眼Id
        /// </summary>
        public int gasholeId = 0;
    }
    public partial struct Point
    {
        public int x;
        public int y;
    }
    public partial struct Size
    {
        public int width;
        public int height;
    }
    public partial struct Range
    {
        public int min;
        public int max;
    }
    public partial class Fire
    {
        /// <summary>
        /// 攻击类型Id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 攻击属性
        /// </summary>
        public List<Property> properties = new List<Property>();
    }
    public partial class Enhance
    {
        /// <summary>
        /// 效果Id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 强化名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 强化属性列表
        /// </summary>
        public List<Property> properties = new List<Property>();
    }
    public partial class Fortress
    {
        /// <summary>
        /// 堡垒Id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 在计算伤害的时候，是否按照建筑伤害来算
        /// </summary>
        public bool isBuilding = false;
        /// <summary>
        /// 初始生命值
        /// </summary>
        public int hp = 0;
        /// <summary>
        /// 移动速度 像素/秒
        /// </summary>
        public int moveSpeed = 50;
        /// <summary>
        /// 从游戏开始后，经过这么多 毫秒才开始移动堡垒
        /// </summary>
        public int startMoveTime = 0;
        /// <summary>
        /// 从游戏开始后，经过这么多 毫秒进行第一次攻击
        /// </summary>
        public int firstAttackTime = 0;
        /// <summary>
        /// 从首次攻击后开始算，下次攻击的间隔, 单位毫秒
        /// </summary>
        public int atkInterval = 0;
        /// <summary>
        /// 每次攻击后，有一段时间是不能移动的, 单位毫秒
        /// </summary>
        public int restrictionTime = 0;
        /// <summary>
        /// 攻击伤害
        /// </summary>
        public int damage = 0;
        /// <summary>
        /// 建筑伤害比例
        /// </summary>
        public int buildingDamageFactor = 0;
        /// <summary>
        /// 当本方堡垒生命值低于该值，则不再发起攻击.
        /// </summary>
        public int atkHpThreshold = 0;
        /// <summary>
        /// 当敌方堡垒生命值低于该值，则不再发起攻击.
        /// </summary>
        public int ceasefireHpThreshold = 0;
        /// <summary>
        /// 当我方飞机被击杀时, 扣除我方堡垒生命值。扣除生命值等于 killPlaneLoss * 被击杀飞机的rank
        /// </summary>
        public int killPlaneLoss = 50;
        /// <summary>
        /// 堡垒名字
        /// </summary>
        public string name = "";
        /// <summary>
        /// 资源名
        /// </summary>
        public string resourceImg = "";
    }
    public partial class Property
    {
        public PropertyTypes type;
        public int value = 0;
    }
    /// <summary>
    /// 气体Id和权重
    /// </summary>
    public partial class GasWeight
    {
        public int id = 0;
        public int weight = 0;
    }
    public partial class Gas
    {
        /// <summary>
        /// 气体id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 气体半径( 与飞机 搜集半径相交的话会被搜集 )
        /// </summary>
        public int radius = 0;
        /// <summary>
        /// 气体能量
        /// </summary>
        public int energy = 0;
        /// <summary>
        /// 气体名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 资源名
        /// </summary>
        public string resourceImg = "";
    }
    public partial class SkillOffset
    {
        /// <summary>
        /// id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 位置相对飞机中心点的坐标
        /// </summary>
        public List<Point> point = new List<Point>();
    }
    public partial class Skill
    {
        /// <summary>
        /// 技能Id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// [属性:值] 键值对数组
        /// </summary>
        public List<Property> properties = new List<Property>();
    }
    public partial class Plane
    {
        /// <summary>
        /// 飞机Id
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 基础生命值
        /// </summary>
        public int hp = 0;
        /// <summary>
        /// 基础护甲, 于 hp 的伤害值等于 攻击力 - 护甲，最小是1
        /// </summary>
        public int armor = 0;
        /// <summary>
        /// 基础护盾, 护盾是真实伤害，不考虑护甲( 先于 hp 扣除 )
        /// </summary>
        public int shield = 0;
        /// <summary>
        /// 护盾回复每跳间隔时长,每回复 1点护盾需要的时间,单位毫秒
        /// </summary>
        public int shieldRecoverInterval = 0;
        /// <summary>
        /// 护盾回复间隔,飞机在间隔时间内未受到攻击,才开始回复护盾,单位毫秒
        /// </summary>
        public int shieldBeginRecoverInterval = 0;
        /// <summary>
        /// 是否为建筑护甲. 某些技能针对此属性有不一样的效果
        /// </summary>
        public bool isBuilding = false;
        /// <summary>
        /// 飞机的阶级 ****** 听说本方的飞机被击杀后，堡垒要扣血，扣血量就是  击杀扣血 * 被击杀飞机的阶级
        /// </summary>
        public int rank = 0;
        /// <summary>
        /// 飞行速度,单位 像素/秒
        /// </summary>
        public int moveSpeed = 0;
        /// <summary>
        /// 收集半径,能源与飞机在半径范围内则被吸收
        /// </summary>
        public int colletRadius = 0;
        /// <summary>
        /// 机体碰撞半径
        /// </summary>
        public int collisionRadius = 0;
        /// <summary>
        /// 受击半径
        /// </summary>
        public int strikeRadius = 0;
        /// <summary>
        /// 角速度 每秒旋转的角度 360度标准
        /// </summary>
        public int angleLimit = 0;
        /// <summary>
        /// 焦点侦测距离
        /// </summary>
        public int targetDetectDistance = 0;
        /// <summary>
        /// 焦点侦测扇形角度, 范围0~360, 是扇形圆心角度的一半
        /// </summary>
        public int targetDetectAngle = 0;
        /// <summary>
        /// 炮管位置偏移坐标
        /// </summary>
        public List<Point> shootOffsets = new List<Point>();
        /// <summary>
        /// 指向 Skill 表的行. [0]是普通攻击. [1]是主动技, [2]是被动技(反隐啥的)
        /// </summary>
        public List<int> skillIds = new List<int>();
        /// <summary>
        /// 能进阶到目标飞机Id的列表. 就在 plane 表中查. 不能进阶则该表为空
        /// </summary>
        public List<int> evolutionIds = new List<int>();
        /// <summary>
        /// 进阶需要的总能量( 实际开支需要减去 强化所花掉的总能量 )
        /// </summary>
        public int evolutionEnergyCost = 0;
        /// <summary>
        /// 每一级的强化能量消耗值. 以某部分当前强化级别作为下标可查. 越界则表示无法继续强化
        /// </summary>
        public List<int> enhanceEnergyCosts = new List<int>();
        /// <summary>
        /// 指向 Enhance 表的行. 当前案子中的可强化部位是 3 个
        /// </summary>
        public List<int> enhanceIds = new List<int>();
        /// <summary>
        /// 初生无敌盾寿命
        /// </summary>
        public int bornShieldLifespan = 0;
        /// <summary>
        /// 初生等待时长
        /// </summary>
        public int bornInterval = 0;
        /// <summary>
        /// 前端 飞机名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 前端 摄像机视野范围
        /// </summary>
        public int vision = 0;
    }
}
