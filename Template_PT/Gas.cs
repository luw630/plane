#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

[Desc("一缕惰气( 其坐标是相对于所在 area 来算的. 区域被分成 256 * 256 格 )")]
class Gas
{
    [Compress, Desc("气体配置 id")]
    int cfgId;

    [Desc("气体所在格子")]
    byte col, row;
}

[Desc("一片区惰气( 内含 xx 个 gas, 随机分布 )")]
class GasArea
{
    [Desc("N 缕惰气( 直接随机. 从 65536 个格子中随机 N 次 分离成 x, y. 不判断是否重叠. 不用抽牌算法是考虑到完整同步时数据量大 )")]
    List<Gas> gass;
}
