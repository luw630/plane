#pragma warning disable 0169, 0414
using TemplateLibrary;

class Map
{
    [Compress, Title("地图id"), Desc("地图id")]
    int id;

    [Title("地图大小"), Desc("地图大小")]
    Size size;

    /***************************************************************************************/
    // Gas 相关( 基本已确定的东西 )
    /***************************************************************************************/

    [Title("气体分区"), Desc("地图中气体将被分割成 weight * height 个大格子区域")]
    Size gasAreaSize;

    [Compress, Title("初始气体数量范围"), Desc("初始气体数量范围( 包含值本身 )")]
    Range gasNumInit;       // 20, 30

    [Compress, Title("气体检测间隔时间"), Desc("气体检测间隔时间: 按气体分区的一个随机序列来遍历检测, 每换一个区, 要等待这个间隔时间")]
    int gasScanInterval;    // 3000;

    [Compress, Title("气体检测值"), Desc("气体检测最低阈值，低于这个值就需要补充")]
    int gasScanThreshold;   // 20;

    [Compress, Title("补充间隔"), Desc("补充生成时每生成一个 gas 的 时间间隔. 通常非常短")]
    int gasSupplementInterval;  // 100;

    [Compress, Title("补充数量范围"), Desc("补充数量范围值. 随机得到之后, 将每隔 补充间隔 时间生成一个, 直到达到该值为止")]
    Range gasNumSupplement; // 5, 10

    [Title("气体种类生成权重表"), Limit(1, 3), Desc("气体种类生成权重表. 以 sum(weight) 值为分母, 按这个比例来补充生成 gas")]
    List<GasWeight> gasWeights;

    /***************************************************************************************/
    // 待定的........
    /***************************************************************************************/

    [Compress, Title("堡垒移动方式"), Desc("堡垒移动方式: 0-不移动, 1-椭圆轨迹,......待定")]
    int fortressMoveType;

    [Limit(0, 2), Title("焦点"), Desc("椭圆有两个焦点，圆只有一个")]
    List<Point> focusPoints;

    [Compress, Title("半径"), Desc("半径")]
    int radius;             // = 1200;

    [Compress, Title("堡垒Id"), Desc("堡垒Id")]
    int fortressId;

    [Compress, Title("气眼Id"), Desc("气眼Id")]
    int gasholeId;

}
