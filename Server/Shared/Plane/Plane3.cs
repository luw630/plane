using xxlib;

namespace PT
{
    partial class Plane3
    {
        void InitCfg()
        {
            if (cfg.plane3 == null)
            {
                cfg.plane3 = new Cfg.Plane3();
                cfg.plane3.enhance_extraHp = cfg.enhances[0].GetValue(Cfg.PropertyTypes.extraHp);
                cfg.plane3.enhance_extraArmor = cfg.enhances[1].GetValue(Cfg.PropertyTypes.extraArmor);
                cfg.plane3.enhance_extraShield = cfg.enhances[2].GetValue(Cfg.PropertyTypes.extraShield);
                cfg.plane3.cdTicks2 = cfg.skills[1].GetValue(Cfg.PropertyTypes.cd).ToTicks();
                cfg.plane3.numTotal = cfg.skills[1].GetValue(Cfg.PropertyTypes.numTotal);
                cfg.plane3.intervalTicks = cfg.skills[1].GetValue(Cfg.PropertyTypes.interval).ToTicks();
            }
        }

        public override void Init(Scene scene, Player player, float a = 0)
        {
            cfgId = 3;
            cfg = scene.cfgs.planes.Find(p => p.id == cfgId);
            base.Init(scene, player, a);
            InitCfg();

            hp = cfg.hp + cfg.plane3.enhance_extraHp * player.enhanceLevels[0];
            shield = cfg.shield + cfg.plane3.enhance_extraShield * player.enhanceLevels[2];
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
                if (cfg.armor > 0) val -= cfg.armor + cfg.plane3.enhance_extraArmor * player.enhanceLevels[1];
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
                castLeftFireTimes = cfg.plane3.numTotal;
                castCDTicks = ticks + (cfg.plane_Laser.startDelayTicks + cfg.plane_Laser.endDelayTicks + cfg.plane3.intervalTicks) * cfg.plane3.numTotal + cfg.plane3.cdTicks2;
                fireCancel = true;
            }
            else if (castCDTicks > ticks)
            {

                // 复制小改
                if (fireCDTicks <= ticks && castLeftFireTimes > 0)
                {
                    fireBeforeTicks = ticks + cfg.plane_Laser.startDelayTicks;
                    fireAfterTicks = fireBeforeTicks + cfg.plane_Laser.endDelayTicks;
                    fireCDTicks = fireAfterTicks + cfg.plane3.intervalTicks;
                    fireTargetId = targetId;
                    fireTarget = target;
                    castLeftFireTimes--;
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
                else if (fireCDTicks > ticks)
                {
                    if (fireBeforeTicks == ticks)
                    {
                        var bak_angle = angle;
                        var bak_xyIncBase = xyIncBase;
                        if (target != null)
                        {
                            angle = (float)XY.Angle(xy, target.xy);
                            xyIncBase = XY.Forward(angle);
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
            else
            {
                base.Update2(ticks);
            }
        }
    }
}
