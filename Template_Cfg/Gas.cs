#pragma warning disable 0169, 0414
using TemplateLibrary;


[Struct, Desc("气体Id和权重")]
class GasWeight
{
    [Compress]
    int id;
    [Compress]
    int weight;
}

class Gas
{
    [Compress, Title("气体id"), Desc("气体id")]
    int id;

    [Compress, Title("气体半径"), Desc("气体半径( 与飞机 搜集半径相交的话会被搜集 )")]
    int radius;

    [Compress, Title("气体能量"), Desc("气体能量")]
    int energy;

    /******************************************************************************************/
    // 前端专用( 感觉还要继续修订 )
    /******************************************************************************************/

    [Title("名称"), Desc("气体名称")]
    string name;

    [Title("资源名"), Desc("资源名")]
    string resourceImg;
}
