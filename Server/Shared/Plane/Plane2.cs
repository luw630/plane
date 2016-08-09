using xxlib;

namespace PT
{
    partial class Plane2
    {
        void InitCfg()
        {
            if (cfg.plane2 == null)
            {
                cfg.plane2 = new Cfg.Plane2();
                cfg.plane2.enhance_extraHp = cfg.enhances[0].GetValue(Cfg.PropertyTypes.extraHp);
                cfg.plane2.enhance_extraArmor = cfg.enhances[1].GetValue(Cfg.PropertyTypes.extraArmor);
                cfg.plane2.enhance_extraShield = cfg.enhances[2].GetValue(Cfg.PropertyTypes.extraShield);
                cfg.plane2.cdTicks2 = cfg.skills[1].GetValue(Cfg.PropertyTypes.cd).ToTicks();
                cfg.plane2.damage2 = cfg.skills[1].GetValue(Cfg.PropertyTypes.damage);
            }
        }

        public override void Init(Scene scene, Player player, float a = 0)
        {
            cfgId = 2;
            cfg = scene.cfgs.planes.Find(p => p.id == cfgId);
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

        public override HurtResult Hurt(int ticks, int val)
        {
            HurtResult result;
            // 扣甲 扣血. 优先扣甲。 甲没了 接着扣血。 扣血时要用 armor 值来免一部分伤。免不完。至少剩 1 
            if (val > shield)
            {
                val -= shield;
                result.shield = shield;
                shield = 0;
                if (cfg.armor > 0) val -= cfg.armor + cfg.plane2.enhance_extraArmor * player.enhanceLevels[1];
                if (val < 1) val = 1;
                hp -= val;
                result.hp = val;
            }
            else
            {
                shield -= val;
                result.hp = 0;
                result.shield = val;
            }

            // 重置 tar 的 shield 再生时间 如果它有的话
            if (cfg.shield > 0)
            {
                shieldBeginRecoverIntervalTicks = ticks + cfg.shieldBeginRecoverIntervalTicks;
                shieldRecoverIntervalTicks = ticks + cfg.shieldBeginRecoverIntervalTicks;
            }
            return result;
        }

        public override void Update2(int ticks)
        {
            // 大招的释放更优先
            if (skillSwitches[1] && castCDTicks <= ticks)
            {
                castCDTicks = ticks + (cfg.plane_Laser.startDelayTicks + cfg.plane_Laser.endDelayTicks + cfg.plane_Laser.cdTicks) + cfg.plane2.cdTicks2;
                // 开始前摇( 顺道把整个开火流程的 timer 的值都设起 )
                fireBeforeTicks = ticks + cfg.plane_Laser.startDelayTicks;
                fireAfterTicks = fireBeforeTicks + cfg.plane_Laser.endDelayTicks;
                fireCDTicks = fireAfterTicks + cfg.plane_Laser.cdTicks;
                fireTargetId = targetId;
                fireTarget = target;
                fireCancel = true;
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
            else if (castCDTicks > ticks)
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
                            var res = tar.Hurt(ticks, cfg.plane2.damage2);
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
            else
            {
                base.Update2(ticks);
            }
        }
    }
}
