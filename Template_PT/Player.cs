#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

[Desc("阵营")]
enum Camps : byte
{
    [Desc("联盟")]
    Alliance,
    [Desc("部落")]
    Horde,
    [Desc("中立")]
    Neutral,
}

[Desc("玩家基本信息")]
class Player
{
    [Compress, Desc("经由 currentPlayerId 自增填充")]
    int id = -1;

    [Compress, Desc("当前的飞机(配置)类型")]
    int planeCfgId;

    [Desc("阵营")]
    Camps camp;

    [Compress, Desc("重生冷却 timer")]
    int bornCDTicks;

    [Compress, Desc("当前收集到的能量.")]
    int energy = 0;

    [Compress, Desc("当前强化等级")]
    List<int> enhanceLevels;

    [Compress, Desc("玩家当前击杀数")]
    int numKills = 0;

    [Compress, Desc("玩家被击杀数")]
    int numDeads = 0;

    [Compress, Desc("玩家助攻数")]
    int numAssists = 0;
}
