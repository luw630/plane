#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

[Desc("游戏场景")]
class Scene
{
    [Desc("当前游戏随机数发生器")]
    Random rnd;

    [Desc("当前游戏地图")]
    Map map;

    [Desc("玩家代理容器")]
    List<Player> players;

    [Desc("当前还活着的所有飞机")]
    List<Plane> planes;

    [Desc("成功Join的玩家自增id( 因为当前玩家只可能有一架飞机. 故 planeId 直接用 playerId )")]
    int currentPlayerId = 0;

    [Desc("游戏帧进度值. 代表了某时间刻度")]
    int ticks;
}
