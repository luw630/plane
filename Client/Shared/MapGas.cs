using System.Collections.Generic;
using xxlib;


/***************************************************************************************/
// 配置附加部分
/***************************************************************************************/

namespace Cfg
{
    partial class Map
    {
        /// <summary>
        /// 换算为 ticks 值的 gasScanInterval
        /// </summary>
        public int gasScanIntervalTicks;

        /// <summary>
        /// 换算为 ticks 值的 gasSupplementInterval
        /// </summary>
        public int gasSupplementIntervalTicks;

        /// <summary>
        /// 长度 = 所有权值加相的数组. 用于随机分配 gas
        /// </summary>
        public List<Gas> gasRandoms = new List<Gas>();

        /// <summary>
        /// 加载 / 填充配置之后执行该函数以填充计算字段
        /// </summary>
        public void Init(Cfgs cfgs)
        {
            // ms to ticks
            gasScanIntervalTicks = gasScanInterval.ToTicks();
            gasSupplementIntervalTicks = gasSupplementInterval.ToTicks();

            // fill gasWeightsItems
            gasRandoms.Clear();
            foreach (var w in gasWeights)
            {
                var c = cfgs.gass[w.id];
                for (int i = 0; i < w.weight; i++)
                {
                    gasRandoms.Add(c);
                }
            }
        }
    }
}


/***************************************************************************************/
// 原型附加部分
/***************************************************************************************/

namespace PT
{
    partial class Gas
    {
        /// <summary>
        /// 实际坐标 
        /// </summary>
        public XY xy;

        /// <summary>
        /// 指向配置项
        /// </summary>
        public Cfg.Gas cfg;
    }

    partial class GasArea
    {
        /// <summary>
        /// 区域的起始坐标
        /// </summary>
        public float offsetX, offsetY;
    }

    partial class Map
    {
        /// <summary>
        /// 指向场景
        /// </summary>
        public Scene scene;

        /// <summary>
        /// 指向地图配置项
        /// </summary>
        public Cfg.Map cfg;

        /// <summary>
        /// 临时用于存飞机横跨区域
        /// </summary>
        List<int> aidxs = new List<int>();

        /// <summary>
        /// 方便计算坐标: 每个区域的宽高
        /// </summary>
        float areaWidth, areaHeight;

        /// <summary>
        /// 方便计算坐标: 每个区域又分成 256 * 256 格, 每一格的宽高, 以及格子中心点的偏移量
        /// </summary>
        float cellWidth, cellHeight, cellOffsetX, cellOffsetY;


        public void Init(Scene scene, int cfgId)
        {
            // 预运算 / 填充 / 关联
            this.scene = scene;
            this.cfgId = cfgId;
            cfg = scene.cfgs.maps[cfgId];

            areaWidth = cfg.size.width / cfg.gasAreaSize.width;
            areaHeight = cfg.size.height / cfg.gasAreaSize.height;
            cellWidth = areaWidth / 256;
            cellHeight = areaHeight / 256;
            cellOffsetX = cellWidth / 2;
            cellOffsetY = cellHeight / 2;

            // 下次扫描时间
            gasScanIntervalTicks = scene.ticks + cfg.gasScanIntervalTicks;

            // 创建 gas area
            for (int i = 0; i < cfg.gasAreaSize.height; i++)
            {
                for (int j = 0; j < cfg.gasAreaSize.width; j++)
                {
                    var area = new PT.GasArea();
                    area.offsetX = areaWidth * j;       // 填充方便计算的坐标偏移量
                    area.offsetY = areaHeight * i;
                    areas.Add(area);
                    areaSequence.Add(i * cfg.gasAreaSize.width + j);

                    // 初始生成一批 gas
                    var count = cfg.gasNumInit.min + scene.rnd.Next(cfg.gasNumInit.max - cfg.gasNumInit.min + 1);
                    for (int k = 0; k < count; k++)
                    {
                        var r = scene.rnd.Next(65536);
                        var xy = (ushort)r;
                        var idx = r % cfg.gasRandoms.Count;
                        var gas = new Gas { cfgId = cfg.gasRandoms[idx].id, cfg = cfg.gasRandoms[idx], col = (byte)xy, row = (byte)(xy >> 8) };
                        gas.xy.x = area.offsetX + gas.col * cellWidth + cellOffsetX;
                        gas.xy.y = area.offsetY + gas.row * cellHeight + cellOffsetY;
                        area.gass.Add(gas);
                    }
                }
            }
            // 打乱 sequence 的内容
            var numAreas = cfg.gasAreaSize.height * cfg.gasAreaSize.width;

            for (int i = numAreas - 1; i > 0; --i)
            {
                var idx = scene.rnd.Next(i);  // 得到 0 ~ i 的随机值 但不包含 i
                // idx, i 所在交换位置
                var bak = areaSequence[idx];
                areaSequence[idx] = areaSequence[i];
                areaSequence[i] = bak;
            }

        }

        public void Restore(Scene scene)
        {
            this.scene = scene;
            cfg = scene.cfgs.maps[cfgId];

            areaWidth = cfg.size.width / cfg.gasAreaSize.width;
            areaHeight = cfg.size.height / cfg.gasAreaSize.height;
            cellWidth = areaWidth / 256;
            cellHeight = areaHeight / 256;
            cellOffsetX = cellWidth / 2;
            cellOffsetY = cellHeight / 2;

            // 填充各种扩展字段
            for (int i = 0; i < cfg.gasAreaSize.height; i++)
            {
                for (int j = 0; j < cfg.gasAreaSize.width; j++)
                {
                    var area = areas[i * cfg.gasAreaSize.width + j];
                    area.offsetX = areaWidth * j;       // 填充方便计算的坐标偏移量
                    area.offsetY = areaHeight * i;

                    foreach (var gas in area.gass)
                    {
                        gas.cfg = scene.cfgs.gass[gas.cfgId];
                        gas.xy.x = area.offsetX + gas.col * cellWidth + cellOffsetX;
                        gas.xy.y = area.offsetY + gas.row * cellHeight + cellOffsetY;
                    }
                }
            }
        }


        public void Update(int ticks)
        {
            // 判断飞机有没有吃到 gas
            foreach (var p in scene.planes)
            {
                aidxs.Clear();

                // 这里先把 plane 中心点所在 area 放入容器
                var row = (int)(p.xy.y / areaHeight);
                var col = (int)(p.xy.x / areaWidth);

                // 越界修正( float 除法是有误差的 )
                if (row >= cfg.gasAreaSize.height) row = cfg.gasAreaSize.height - 1;
                if (col >= cfg.gasAreaSize.width) col = cfg.gasAreaSize.width - 1;

                aidxs.Add(row * cfg.gasAreaSize.width + col);          // todo: 判断飞机覆盖了哪些格子, 使用 AB 检测, 填到 gas. 

                foreach (var aidx in aidxs)
                {
                    var gass = areas[aidx].gass;
                    for (int i = gass.Count - 1; i >= 0; --i)
                    {
                        var gas = gass[i];
                        var dist = p.cfg.colletRadius + gas.cfg.radius;           // 半径之和
                        var dist2 = dist * dist;                            // 半径之和 ^ 2

                        if (XY.DistancePow2(p.xy, gas.xy) < dist2)          // 判断距离是否小于半径( 吃到 )
                        {
#if SERVER
#else
                            gas.eater = p;
                            gas.DrawDestroy();
#endif
                            p.player.energy += gas.cfg.energy;
                            gass.FastRemoveAt(i);
                        }
                    }
                }
            }

            // 如果当前处于正在增生状态
            if (gasNumSupplement > 0)
            {
                // 看看间隔 cd 有没有到.没到就退出. 否则就重置 cd, --gasNumSupplements, 生成一个
                if (gasSupplementIntervalTicks <= ticks)
                {
                    gasSupplementIntervalTicks = ticks + cfg.gasSupplementIntervalTicks;
                    --gasNumSupplement;

                    var area = areas[areaSequence[currentAreaSequenceIndex]];
                    var r = scene.rnd.Next(65536);
                    var xy = (ushort)r;
                    var idx = r % cfg.gasRandoms.Count;
                    var gas = new Gas { cfgId = cfg.gasRandoms[idx].id, cfg = cfg.gasRandoms[idx], col = (byte)xy, row = (byte)(xy >> 8) };
                    gas.xy.x = area.offsetX + gas.col * cellWidth + cellOffsetX;
                    gas.xy.y = area.offsetY + gas.row * cellHeight + cellOffsetY;
                    area.gass.Add(gas);
#if SERVER
#else
                    gas.DrawInit();
#endif
                }
            }
            else
            {
                // 到时间点 gas 扫描再生: 向 gasNumSupplements 中记录要生成多少个
                if (gasScanIntervalTicks <= ticks)
                {
                    gasScanIntervalTicks = ticks + cfg.gasScanIntervalTicks;

                    currentAreaSequenceIndex++;
                    if (currentAreaSequenceIndex == areaSequence.Count)
                    {
                        currentAreaSequenceIndex = 0;
                    }

                    var area = areas[areaSequence[currentAreaSequenceIndex]];
                    // 需要再生
                    if (area.gass.Count < cfg.gasScanThreshold)
                    {
                        gasNumSupplement = cfg.gasNumSupplement.min + scene.rnd.Next(cfg.gasNumSupplement.max - cfg.gasNumSupplement.min + 1);
                    }
                }
            }
        }

    }
}
