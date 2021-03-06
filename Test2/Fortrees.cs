﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using xxlib;

public class FortreesCfg
{
    // 原配
    public int stopTimespan = 1000;
    public int moveTimespan = 2000;
    public int fireTimespan = 4000;

    public int bulletRadius = 10;
    public int bulletMoveSpeed = 200;
    public int bulletDamage = 100;
    public int bulletHurtInterval = 100;

    public int hp = 1000;
    public int stopFireHpThreshold = 200;

    public int radius = 20;                 // 堡垒半径
    public int margin = 100;                // 控制点距离地图的边距
    public int timeScale = 10;              // 时间除法系数. 值越大, 速度越慢
    public int tension = 300;               // 千分之一 张力
    public int timespan = 670;              // 对面那个基地的时间偏移


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
    Canvas root;
    Ellipse circle;             // 本体 显示对象
    Ellipse bullet;             // 子弹 显示对象
    Ellipse circlefill;
    ArcSegment arcline;
    ArcSegment arclinedown;
    Path myPath;
    PathGeometry pathgeometry;
    CardinalSplineTo action;    // 动作控制器
    SolidColorBrush mySolidColorBrush;
    Thickness thickness;

    Rectangle rectFortrees;
    Rectangle rectFill;

    FortreesCfg cfg;
    Fortrees others;            // 指向另外一个实例( 当前游戏模式就是两个 )
    int timespan;               // 偏移时间 典型值 670 ( 可能因地图尺寸不同而需要调整 )

    public int hp;              // 当前血量
    public XY pos;              // 当前坐标
    FortreesStates currState;   // 当前状态
    int stateTicks;             // 当前状态的 timer
    int moveTicks;              // 当前移动 ticks ( 与移动位置对应 )( move 状态专属 )
    bool bulletAlive;           // 子弹存活中
    XY bulletPos;               // 当前子弹位置( fire 状态专属 )
    XY bulletMoveInc;           // 帧移动增量( 发射瞬间算出来的 )( fire 状态专属 )
    XYTABLE xytable;
    XY bulletMove;

    public void DrawInit()
    {
        var s = new Size { width = (int)root.Width, height = (int)root.Height };
        var wh = new Size { width = s.width - cfg.margin * 2, height = s.height - cfg.margin * 2 };

        // area
        var rect = new Rectangle();
        rect.Width = wh.width;
        rect.Height = wh.height;
        //rect.Fill = Brushes.Green;
        rect.StrokeThickness = 2;
        rect.Stroke = Brushes.Red;
        rect.SetPositon(new XY(cfg.margin, cfg.margin));
        root.Children.Add(rect);

        mySolidColorBrush = new SolidColorBrush();
        mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);

        rectFortrees = new Rectangle();
        rectFortrees.Width = cfg.radius * 4;
        rectFortrees.Height = cfg.radius*2;
        rectFortrees.StrokeThickness = 2;
        rectFortrees.Fill = mySolidColorBrush;
        rectFortrees.SetPositon(pos);
        root.Children.Add(rectFortrees);

        rectFill = new Rectangle();
        rectFill.Width = cfg.radius * 4;
        rectFill.Height = cfg.radius * 2;
        rectFill.StrokeThickness = 1;
        rectFill.Stroke = Brushes.Red;
        rectFill.SetPositon(pos);
        root.Children.Add(rectFill);

        // circle
        circle = new Ellipse();
        circle.Width = circle.Height = cfg.radius * 2;
        //circle.Width = circle.Height * 2;
      
      //  circle.Fill = mySolidColorBrush;
        //e.Fill = Brushes.Red;
        circle.StrokeThickness = 2;
        circle.Stroke = Brushes.Red;
        
         root.Children.Add(circle);
//         root.Children.Add(circlefill);

        // bullet
        bullet = new Ellipse();
        bullet.Width = bullet.Height = cfg.bulletRadius * 2;
        //e.Fill = Brushes.Red;
        bullet.StrokeThickness = 2;
        bullet.Stroke = Brushes.Blue;
        root.Children.Add(bullet);

        hp = cfg.hp;
//         arcline = new ArcSegment();
//         arcline.Point = new Point(pos.x+ circle.Width/2, pos.y);
//         arcline.Size = new System.Windows.Size(circlefill.Width/2, circlefill.Width/2);
//         arcline.RotationAngle = 360;
//         arcline.IsLargeArc = true;
//         arcline.SweepDirection = SweepDirection.Clockwise;
// 
//         arclinedown = new ArcSegment();
//         arclinedown.Point = new Point(pos.x + circle.Width / 2, pos.y);
//         arclinedown.Size = new System.Windows.Size(circlefill.Width / 2, circlefill.Width / 2);
//         arclinedown.RotationAngle = 360;
//         arclinedown.IsLargeArc = true;
//         arclinedown.SweepDirection = SweepDirection.Counterclockwise;
// 
//         PathSegmentCollection pathsegmentCollection = new PathSegmentCollection();
//         pathsegmentCollection.Add(arcline);
//         pathsegmentCollection.Add(arclinedown);
// 
//         PathFigure pathFigure = new PathFigure();
//         pathFigure.StartPoint = new Point(pos.x- circle.Width/2, pos.y);
//         pathFigure.Segments = pathsegmentCollection;
// 
//         PathFigureCollection pathFigureCollection = new PathFigureCollection();
//         pathFigureCollection.Add(pathFigure);
// 
//         myPath = new Path();
//         pathgeometry = new PathGeometry();
//         pathgeometry.Figures = pathFigureCollection;
// 
// 
//         myPath.Stroke = Brushes.Red;
//         myPath.StrokeThickness = 1;
//         myPath.Fill = mySolidColorBrush;
//         myPath.Data = pathgeometry;
// 
//         root.Children.Add(myPath);
//  Size arcsize = new Size { width = (int)circlefill.Width, height = (int)circlefill.Height };


    }
    public void DrawUpdate()
    {
        if (circle == null)
        {
            DrawInit();
        }
        circle.SetPositon(pos + new XY(-cfg.radius, -cfg.radius));
     //   circlefill.SetPositon(pos + new XY(-cfg.radius, -cfg.radius));
        rectFortrees.SetPositon(pos + new XY(-cfg.radius, -cfg.radius));
        rectFill.SetPositon(pos + new XY(-cfg.radius, -cfg.radius));
        if (bulletAlive)
        {
            bullet.Visibility = Visibility.Visible;
            bullet.SetPositon(bulletPos + new XY(-cfg.bulletRadius, -cfg.bulletRadius));
        }
        else
        {
            bullet.Visibility = Visibility.Hidden;
        }
    }

    public void Init(Canvas root, Fortrees others, int ticks, bool isOthers = false)
    {
        this.root = root;
        cfg = new FortreesCfg();
        cfg.InitCfg();
        this.others = others;
        hp = cfg.hp;
        timespan = isOthers ? cfg.timespan : 0;
        stateTicks = ticks + cfg.stopTimespanTicks;
        moveTicks = 0;
        currState = FortreesStates.Stop;

        // 假设这个就是地图大小
        var s = new Size { width = (int)root.Width, height = (int)root.Height };
        xytable = XYTABLE.GetInstance((int)root.Width, (int)root.Height);

        // 轨迹矩形大小
        var wh = new Size { width = s.width - cfg.margin * 2, height = s.height - cfg.margin * 2 };

        // 构建控制点数组
        var array = new List<XY>();
        array.Add(new XY(0, 0));
        array.Add(new XY(wh.width, 0));
        array.Add(new XY(wh.width, wh.height));
        array.Add(new XY(0, wh.height));
        action = new CardinalSplineTo(array, cfg.tension / 1000.0f);

        // 初始化移动行为控制器
        action.StartWithTarget(new XY(cfg.margin, cfg.margin));


        // 合并计算公式
        ticks2secScale = Cfgs.logicFrameMS / 1000.0f / cfg.timeScale;
        floatTimespan = timespan / 1000.0f;

        // 初始化初始坐标
        var time = moveTicks * ticks2secScale + floatTimespan;
        pos = action.Calc(time);
        DrawUpdate();

       long nsecont= System.DateTime.Now.Ticks;
        for (int i = 0; i < (int)root.Height; i++)
        {
            for (int j = 0; j < (int)root.Width; j++)
            {
                XY a = new XY(i, j);
                XY b = new XY((int)root.Height - i - 1, (int)root.Width - j - 1);
                 double avalue = xytable.GetAngle(a,b);
              //  bulletMoveInc = xytable.GetForward(avalue);
            //    int index = xytable.GetAngleIndex(a, b);
                //int index = i * (j + 1);
//                 if (index >= 0)
//                 {
//                     var avalue = xytable.GetAngle(index);
//                     bulletMoveInc = xytable.GetForward(index);
//                 }
//                 else
//                 {
//                     Console.WriteLine("GetIndex Faile ");
//                 }
            }
        }


        long nowsec = System.DateTime.Now.Ticks - nsecont;
        Console.WriteLine("secxytable " + nowsec);

        nsecont = System.DateTime.Now.Ticks;
        for (int i = 0; i < (int)root.Width; i++)
        {
            for (int j = 0; j < (int)root.Height; j++)
            {
                XY a = new XY(i, j);
                XY b = new XY((int)root.Width - i - 1, (int)root.Height - j - 1);


                var avalue = XY.Angle(a,b);
                bulletMoveInc = XY.Forward(avalue);
            }
        }

         nowsec = System.DateTime.Now.Ticks - nsecont;
        Console.WriteLine("secxy " + nowsec);

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
                    // 发射子弹: 根据两点算角度, 得增量, 以当前圆心, 半径来算出初始坐标
                    int index = xytable.GetAngleIndex(pos, others.pos);
                    if(index >=0)
                    {
                        var b = xytable.GetAngle(index);
                        bulletMove = xytable.GetForward(index);
                        bulletPos = pos + bulletMove * cfg.radius;
                        bulletAlive = true;
                    }
//                     var b = xytable.GetAngle(pos, others.pos);
//                     bulletMove = xytable.GetForward(b);

//                     var a = XY.Angle(pos, others.pos);
//                     bulletMoveInc = XY.Forward(a);
//                     bulletPos = pos + bulletMoveInc * cfg.radius;
//                     bulletAlive = true;
                }
                else
                {
                    ++moveTicks;
                    var time = moveTicks * ticks2secScale + floatTimespan;
                    pos = action.Calc(time);
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

        if (bulletAlive && stateTicks != ticks + cfg.fireTimespanTicks)      // move 刚转 fire 的时候的初始坐标就不必计算移动了
        {
            //bulletPos += bulletMoveInc * cfg.bulletMoveSpeedPerFrame;
            bulletPos += bulletMove * cfg.bulletMoveSpeedPerFrame;
            if (XY.DistancePow2(bulletPos, others.pos) <= cfg.bulletRadius * cfg.bulletRadius + cfg.radius * cfg.radius)
            {
                if(hp >= cfg.bulletDamage)
                {
                    hp -= cfg.bulletDamage;
                    rectFortrees.Height -= (double)cfg.bulletDamage / (double)cfg.hp * rectFortrees.Height;
                }
                else
                {
                    
                }
                // todo: 击中减血
                
//                 circlefill.Width-= 2;
//                 circlefill.Height-=2;
                bulletAlive = false;
            }
        }

        DrawUpdate();
    }
}
