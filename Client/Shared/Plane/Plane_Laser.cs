using xxlib;

namespace PT
{
    partial class Plane_Laser
    {
        /// <summary>
        /// 为显示增加临时变量存 射击的两点坐标
        /// </summary>
        public XY fireXY1, fireXY2;

        /// <summary>
        /// 为显示增加临时变量存 开火瞬间飞机朝向
        /// </summary>
        public float fireAngle;

        /// <summary>
        /// 与 fireTargetId 同步, 方便使用( 开始发射时才赋值 )
        /// </summary>
        public Plane fireTarget;

        void InitCfg()
        {
            if (cfg.plane_Laser == null)
            {
                cfg.plane_Laser = new Cfg.Plane_Laser();
                cfg.plane_Laser.startDelayTicks = cfg.skills[0].GetValue(Cfg.PropertyTypes.startDelay).ToTicks();
                cfg.plane_Laser.endDelayTicks = cfg.skills[0].GetValue(Cfg.PropertyTypes.endDelay).ToTicks();
                cfg.plane_Laser.cdTicks = cfg.skills[0].GetValue(Cfg.PropertyTypes.cd).ToTicks();
                cfg.plane_Laser.range = cfg.skills[0].GetValue(Cfg.PropertyTypes.range);
                cfg.plane_Laser.damage = cfg.skills[0].GetValue(Cfg.PropertyTypes.damage);
                cfg.plane_Laser.atkMoveSpdScaling = cfg.skills[0].GetValue(Cfg.PropertyTypes.atkMoveSpdScaling);
            }
        }

        public override void Init(Scene scene, Player player, float a = 0)
        {
            base.Init(scene, player, a);
            InitCfg();
#if SERVER
#else
            ClientInit();
#endif
        }

        public override void Restore(Scene scene)
        {
            base.Restore(scene);
            InitCfg();
#if SERVER
#else
            ClientRestore();
#endif
        }

        public override void ClearFocus(int planeId)
        {
            base.ClearFocus(planeId);
            if (planeId == fireTargetId)
            {
                // 失去焦点
                fireTargetId = -1;
                fireTarget = null;
            }
        }

        public override void Update0(int ticks)
        {
            if (fireCDTicks > ticks)
            {
                xyIncMoveScaling = cfg.plane_Laser.atkMoveSpdScaling;
            }
            else xyIncMoveScaling = 0;

            base.Update0(ticks);
        }

        public override void Update2(int ticks)
        {
            if (skillSwitches[0] && fireCDTicks <= ticks)
            {
                // 开始前摇( 顺道把整个开火流程的 timer 的值都设起 )
                fireBeforeTicks = ticks + cfg.plane_Laser.startDelayTicks;
                fireAfterTicks = fireBeforeTicks + cfg.plane_Laser.endDelayTicks;
                fireCDTicks = fireAfterTicks + cfg.plane_Laser.cdTicks;
                fireTargetId = targetId;
                fireTarget = target;
                fireCancel = false;
#if SERVER
#else
                if (fireTarget != null)
                {
                    fireAngle = (float)XY.Angle(xy, fireTarget.xy);
                }
                else
                {
                    fireAngle = angle;
                }
                DrawStartFire(fireAngle);
#endif
            }
            else if (fireCDTicks > ticks && !fireCancel)
            {
                // 判断前摇是否刚结束( 这一帧发射 )
                if (fireBeforeTicks == ticks)
                {
                    // 如果当前有焦点目标, 就对它发射( 飞机朝向瞬间变化, 发射完后还原 )
                    // 焦点的作用只是决定开火瞬间的飞机朝向
                    var bak_angle = angle;
                    var bak_xyIncBase = xyIncBase;
                    if (target != null)
                    {
                        angle = (float)XY.Angle(xy, target.xy);
                        xyIncBase = XY.Forward(angle);   // 填充帧xy步进值( FindTarget 里要用到 )
                    }
#if SERVER
#else
                    fireAngle = angle;
#endif

                    var t = scene.FindTarget(this, cfg.plane_Laser.range);
                    fireXY1 = t.p1;
                    fireXY2 = t.p2;
                    var tar = t.tar;
#if SERVER
#else
                    DrawBullet(t);
#endif
                    if (tar != null)
                    {
                        if (tar.bornShieldLifespanTicks < ticks)
                        {
                            var res = tar.Hurt(ticks, cfg.plane_Laser.damage);
                            Stat(tar);
#if SERVER
#else
                            tar.DrawDamage(res);
#endif
                        }
                    }

                    if (target != null)
                    {
                        angle = bak_angle;
                        xyIncBase = bak_xyIncBase;
                    }
                }
            }
        }

    }
}
