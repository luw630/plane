#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

[Desc("地图 / gas 容器. 挂在 scene 下面.")]
class Map
{
    [Compress, Desc("当前的地图配置 id")]
    int cfgId;

    [Desc("一地图惰气( 地图分为 ? * ? 的区域. 遍历扫描( 有间隔时间 )每片区内 gas 总数.如果少于某值, 就补充生成 xx 个 )")]
    List<GasArea> areas;

    [Compress, Desc("扫描次序队列( areas 的下标随机洗牌打散 )")]
    List<int> areaSequence;

    [Compress, Desc("gasss 的循环下标. 指向当前扫的区域")]
    int currentAreaSequenceIndex = -1;

    [Compress, Desc("扫描 gass 的间隔 timer. == ticks 之后 currentSequenceIndex +1 扫下一块 并重置该值")]
    int gasScanIntervalTicks;

    [Compress, Desc("生成 gas 的间隔 timer. == ticks 之后 gasNumSupplements-- 生成一个 gas 并重置该值( 如果 gasNumSupplements 不为 0 )")]
    int gasSupplementIntervalTicks;

    [Compress, Desc("本次补充一批还剩多少个没有生成")]
    int gasNumSupplement;
}
