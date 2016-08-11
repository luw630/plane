using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Shared;
using xxlib;
using System.Windows.Threading;
using System.Diagnostics;

public class FortreesCfg
{
    // 原配
    public int stopTimespan = 1000;
    public int moveTimespan = 2000;
    public int fireTimespan = 4000;

    public int bulletRadius = 20;
    public int bulletMoveSpeed = 200;
    public int bulletDamage = 100;
    public int bulletHurtInterval = 100;

    public int hp = 1000;
    public int stopFireHpThreshold = 200;

    // 翻译
    public int stopTimespanTicks;
    public int moveTimespanTicks;
    public int fireTimespanTicks;

    public int bulletMoveSpeedPerFrame;
    public int bulletHurtIntervalTicks;


    public void InitCfg()
    {
        stopTimespanTicks = stopTimespan / Cfgs.logicFrameMS;
        moveTimespanTicks = moveTimespan / Cfgs.logicFrameMS;
        fireTimespanTicks = fireTimespan / Cfgs.logicFrameMS;

        bulletMoveSpeedPerFrame = bulletMoveSpeed / Cfgs.numLogicFramesPerSecond;
        bulletHurtIntervalTicks = bulletHurtInterval / Cfgs.logicFrameMS;
    }
}

public enum FortreesStates : byte
{
    Stop,           // 停止中( 初始状态. 持续时长对应配置为 首次移动间隔 )
    Move,           // 移动中( 蓄力 )
    Fire            // 开火( 以及后续僵直 )
}


public class Fortrees
{
    Ellipse circle;             // 显示对象
    CardinalSplineTo action;    // 动作控制器

    FortreesCfg cfg;
    Fortrees others;            // 指向另外一个实例( 当前游戏模式就是两个 )

    int hp;                     // 当前血量
    FortreesStates currState;   // 当前状态
    int stateTicks;             // 当前状态的 timer
    int moveTicks;

    int radius = 40;            // 堡垒半径
    int margin = 100;           // 控制点距离地图的边距
    int timeScale = 10;         // 时间除法系数. 值越大, 速度越慢
    int tension = 300;          // 千分之一 张力
    int timespan;               // 偏移时间 典型值 670 ( 可能因地图尺寸不同而需要调整 )

    public void Init(Fortrees others, Canvas root, int ticks, int timespan = 670)
    {
        cfg = new FortreesCfg();
        cfg.InitCfg();
        this.others = others;
        hp = cfg.hp;
        this.timespan = timespan;
        stateTicks = ticks + cfg.stopTimespanTicks;
        moveTicks = 0;
        currState = FortreesStates.Stop;

        // 假设这个就是地图大小
        var s = new Size { width = (int)root.Width, height = (int)root.Height };

        // 原点坐标
        XY op;
        op.x = op.y = margin;

        // 轨迹矩形大小
        var wh = new Size { width = s.width - margin * 2, height = s.height - margin * 2 };

        var array = new List<XY>();
        array.Add(new XY(0, 0));
        array.Add(new XY(wh.width, 0));
        array.Add(new XY(wh.width, wh.height));
        array.Add(new XY(0, wh.height));
        action = new CardinalSplineTo(array, tension / 1000.0f);

        #region 初始化显示控件
        // area
        var rect = new Rectangle();
        rect.Width = wh.width;
        rect.Height = wh.height;
        //rect.Fill = Brushes.Green;
        rect.StrokeThickness = 2;
        rect.Stroke = Brushes.Red;
        rect.SetPositon(op);
        root.Children.Add(rect);

        // circle
        circle = new Ellipse();
        circle.Width = circle.Height = radius * 2;
        //e.Fill = Brushes.Red;
        circle.StrokeThickness = 2;
        circle.Stroke = Brushes.Blue;
        root.Children.Add(circle);
        circle.SetPositon(op + new XY(-radius, -radius));
        #endregion

        action.StartWithTarget(circle, op, radius);


        // 合并计算公式
        ticks2secScale = Cfgs.logicFrameMS / 1000.0f / timeScale;
        floatTimespan = timespan / 1000.0f;

        // 初始化初始坐标
        var time = moveTicks * ticks2secScale + floatTimespan;
        action.Update(time);
    }

    float ticks2secScale;
    float floatTimespan;

    internal void Update(int ticks)
    {
        switch (currState)
        {
            case FortreesStates.Stop:
                if (stateTicks <= ticks)
                {
                    currState = FortreesStates.Move;
                    stateTicks = ticks + cfg.moveTimespanTicks;
                }
                break;
            case FortreesStates.Move:
                if (stateTicks <= ticks && hp > cfg.stopFireHpThreshold && others.hp > cfg.stopFireHpThreshold)
                {
                    currState = FortreesStates.Fire;
                    stateTicks = ticks + cfg.fireTimespanTicks;
                    // todo: 发射子弹
                }
                else
                {
                    ++moveTicks;
                    var time = moveTicks * ticks2secScale + floatTimespan;
                    action.Update(time);
                }
                break;
            case FortreesStates.Fire:
                if (stateTicks <= ticks)
                {
                    currState = FortreesStates.Move;
                    stateTicks = ticks + cfg.moveTimespanTicks;
                }
                break;
            default:
                break;
        }

    }
}
