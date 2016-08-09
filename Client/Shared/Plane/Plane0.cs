using xxlib;

namespace PT
{
    partial class Plane0
    {
        void InitCfg()
        {
            if (cfg.plane0 == null)
            {
                cfg.plane0 = new Cfg.Plane0();
                cfg.plane0.enhance_extraHp = cfg.enhances[0].GetValue(Cfg.PropertyTypes.extraHp);
                cfg.plane0.enhance_extraArmor = cfg.enhances[1].GetValue(Cfg.PropertyTypes.extraArmor);
                cfg.plane0.enhance_extraShield = cfg.enhances[2].GetValue(Cfg.PropertyTypes.extraShield);
            }
        }

        public override void Init(Scene scene, Player player, float a = 0)
        {
            cfgId = 0;
            cfg = scene.cfgs.planes.Find(p => p.id == cfgId);
            base.Init(scene, player, a);
            InitCfg();

            hp = cfg.hp + cfg.plane0.enhance_extraHp * player.enhanceLevels[0];
            shield = cfg.shield + cfg.plane0.enhance_extraShield * player.enhanceLevels[2];
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
                if (cfg.armor > 0) val -= cfg.armor + cfg.plane0.enhance_extraArmor * player.enhanceLevels[1];
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
            base.Update2(ticks);
        }
    }
}
