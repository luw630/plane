using System;
using System.Collections.Generic;
using xxlib;
namespace PT
{
    /// <summary>
    /// 一个基地
    /// </summary>
    public partial class Fortress
    {
        /// <summary>
        /// 配置 id
        /// </summary>
        public int cfgId = 0;
        public int startMoveTimeTicks = 0;
        public int firstAttackTimeTicks = 0;
        public int atkIntervalTicks = 0;
        public int restrictionTimeTicks = 0;
    }
    /// <summary>
    /// 一缕惰气( 其坐标是相对于所在 area 来算的. 区域被分成 256 * 256 格 )
    /// </summary>
    public partial class Gas
    {
        /// <summary>
        /// 气体配置 id
        /// </summary>
        public int cfgId = 0;
        /// <summary>
        /// 气体所在格子
        /// </summary>
        public byte col = 0;
        /// <summary>
        /// 气体所在格子
        /// </summary>
        public byte row = 0;
    }
    /// <summary>
    /// 一片区惰气( 内含 xx 个 gas, 随机分布 )
    /// </summary>
    public partial class GasArea
    {
        /// <summary>
        /// N 缕惰气( 直接随机. 从 65536 个格子中随机 N 次 分离成 x, y. 不判断是否重叠. 不用抽牌算法是考虑到完整同步时数据量大 )
        /// </summary>
        public List<Gas> gass = new List<Gas>();
    }
    /// <summary>
    /// 地图 / gas 容器. 挂在 scene 下面.
    /// </summary>
    public partial class Map
    {
        /// <summary>
        /// 当前的地图配置 id
        /// </summary>
        public int cfgId = 0;
        /// <summary>
        /// 一地图惰气( 地图分为 ? * ? 的区域. 遍历扫描( 有间隔时间 )每片区内 gas 总数.如果少于某值, 就补充生成 xx 个 )
        /// </summary>
        public List<GasArea> areas = new List<GasArea>();
        /// <summary>
        /// 扫描次序队列( areas 的下标随机洗牌打散 )
        /// </summary>
        public List<int> areaSequence = new List<int>();
        /// <summary>
        /// gasss 的循环下标. 指向当前扫的区域
        /// </summary>
        public int currentAreaSequenceIndex = -1;
        /// <summary>
        /// 扫描 gass 的间隔 timer. == ticks 之后 currentSequenceIndex +1 扫下一块 并重置该值
        /// </summary>
        public int gasScanIntervalTicks = 0;
        /// <summary>
        /// 生成 gas 的间隔 timer. == ticks 之后 gasNumSupplements-- 生成一个 gas 并重置该值( 如果 gasNumSupplements 不为 0 )
        /// </summary>
        public int gasSupplementIntervalTicks = 0;
        /// <summary>
        /// 本次补充一批还剩多少个没有生成
        /// </summary>
        public int gasNumSupplement = 0;
    }
    /// <summary>
    /// 玩家基本信息
    /// </summary>
    public partial class Player
    {
        /// <summary>
        /// 经由 currentPlayerId 自增填充
        /// </summary>
        public int id = -1;
        /// <summary>
        /// 当前的飞机(配置)类型
        /// </summary>
        public int planeCfgId = 0;
        /// <summary>
        /// 阵营
        /// </summary>
        public Camps camp;
        /// <summary>
        /// 重生冷却 timer
        /// </summary>
        public int bornCDTicks = 0;
        /// <summary>
        /// 当前收集到的能量.
        /// </summary>
        public int energy = 0;
        /// <summary>
        /// 当前强化等级
        /// </summary>
        public List<int> enhanceLevels = new List<int>();
        /// <summary>
        /// 玩家当前击杀数
        /// </summary>
        public int numKills = 0;
        /// <summary>
        /// 玩家被击杀数
        /// </summary>
        public int numDeads = 0;
        /// <summary>
        /// 玩家助攻数
        /// </summary>
        public int numHelpKill = 0;
    }
    /// <summary>
    /// 游戏场景
    /// </summary>
    public partial class Scene
    {
        /// <summary>
        /// 当前游戏随机数发生器
        /// </summary>
        public xxlib.Random rnd = new xxlib.Random();
        /// <summary>
        /// 当前游戏地图
        /// </summary>
        public Map map = new Map();
        /// <summary>
        /// 玩家代理容器
        /// </summary>
        public List<Player> players = new List<Player>();
        /// <summary>
        /// 当前还活着的所有飞机
        /// </summary>
        public List<Plane> planes = new List<Plane>();
        /// <summary>
        /// 成功Join的玩家自增id( 因为当前玩家只可能有一架飞机. 故 planeId 直接用 playerId )
        /// </summary>
        public int currentPlayerId = 0;
        /// <summary>
        /// 游戏帧进度值. 代表了某时间刻度
        /// </summary>
        public int ticks = 0;


    }
    /// <summary>
    /// 所有飞机的基类
    /// </summary>
    public partial class Plane
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id = -1;
        /// <summary>
        /// 所属玩家id
        /// </summary>
        public int playerId = -1;
        /// <summary>
        /// 配置id
        /// </summary>
        public int cfgId = 0;
        /// <summary>
        /// 中心点于 map 的当前坐标
        /// </summary>
        public xxlib.XY xy = new xxlib.XY();
        /// <summary>
        /// 当前角度. 360度 转为 -1 ~ 1 的浮点
        /// </summary>
        public float angle = 0;
        /// <summary>
        /// 玩家旋转的目标角度( 这个会令 angle 值以每帧变化 角速度 来贴最近, 最终等于 )
        /// </summary>
        public float angleTarget = 0;
        /// <summary>
        /// 当前 / 剩余血量
        /// </summary>
        public int hp = 0;
        /// <summary>
        /// 当前 / 剩余护盾值
        /// </summary>
        public int shield = 0;
        /// <summary>
        /// 无敌/初生护盾 timer
        /// </summary>
        public int bornShieldLifespanTicks = 0;
        /// <summary>
        /// 护盾恢复每跳间隔 timer
        /// </summary>
        public int shieldRecoverIntervalTicks = 0;
        /// <summary>
        /// 护盾开始恢复间隔 timer. 挨打会重置
        /// </summary>
        public int shieldBeginRecoverIntervalTicks = 0;
        /// <summary>
        /// 是否正在前进
        /// </summary>
        public bool moving = false;
        /// <summary>
        /// 所有技能的开关状态标志( 取代之前的 firing 之类 )
        /// </summary>
        public List<bool> skillSwitches = new List<bool>();
        /// <summary>
        /// 焦点飞机id( 受 aim 指令影响 )
        /// </summary>
        public int targetId = -1;
    }
    /// <summary>
    /// 普攻是 小激光 的飞机的基类
    /// </summary>
    public partial class Plane_Laser : Plane
    {
        /// <summary>
        /// 开火时锁定的目标
        /// </summary>
        public int fireTargetId = 0;
        /// <summary>
        /// 前摇 timer
        /// </summary>
        public int fireBeforeTicks = 0;
        /// <summary>
        /// 后摇 timer
        /// </summary>
        public int fireAfterTicks = 0;
        /// <summary>
        /// 冷却时间
        /// </summary>
        public int fireCDTicks = 0;
        /// <summary>
        /// 本次开火被终止 但会继续计算 cd 时间
        /// </summary>
        public bool fireCancel = false;
    }
    public partial class Plane0 : Plane_Laser
    {
    }
    public partial class Plane1 : Plane_Laser
    {
        /// <summary>
        /// 大招技能CD
        /// </summary>
        public int castCDTicks = 0;
    }
    public partial class Plane2 : Plane_Laser
    {
        /// <summary>
        /// 状态标志
        /// </summary>
        public bool casting = false;
        /// <summary>
        /// 大招技能CD
        /// </summary>
        public int castCDTicks = 0;
    }
    public partial class Plane3 : Plane_Laser
    {
        /// <summary>
        /// 大招剩余连射次数
        /// </summary>
        public int castLeftFireTimes = 0;
        /// <summary>
        /// 大招技能CD
        /// </summary>
        public int castCDTicks = 0;
    }
}
