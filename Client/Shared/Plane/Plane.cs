using System.Collections.Generic;
using xxlib;


/***************************************************************************************/
// 配置附加部分
/***************************************************************************************/

namespace Cfg
{
    partial class Plane
    {
        // 针对各种 speed, interval 要换算成 每帧的数据, 各种 id 要指针关联

        public int shieldRecoverIntervalTicks;
        public int shieldBeginRecoverIntervalTicks;
        public int moveSpeedPerFrame;
        public List<Skill> skills = new List<Skill>();
        public List<Plane> evolutions = new List<Plane>();
        public List<Enhance> enhances = new List<Enhance>();
        public int bornShieldLifespanTicks;
        public int bornIntervalTicks;
        public float angleLimitPerFrame;

        // 特化配置区
        public Plane_Laser plane_Laser;
        public Plane0 plane0;
        public Plane1 plane1;
        public Plane2 plane2;
        public Plane3 plane3;
        // todo: more


        public void Init(Cfgs cfgs)
        {
            shieldRecoverIntervalTicks = shieldRecoverInterval.ToTicks();
            shieldBeginRecoverIntervalTicks = shieldBeginRecoverInterval.ToTicks();
            moveSpeedPerFrame = moveSpeed.PerFrame();
            foreach (var id in skillIds) skills.Add(cfgs.skills.Find(a => a.id == id));
            foreach (var id in evolutionIds) evolutions.Add(cfgs.planes.Find(a => a.id == id));
            foreach (var id in enhanceIds) enhances.Add(cfgs.enhances.Find(a => a.id == id));
            bornShieldLifespanTicks = bornShieldLifespan.ToTicks();
            bornIntervalTicks = bornInterval.ToTicks();
            angleLimitPerFrame = (float)angleLimit / 180.0f / (float)Cfgs.logicFrameTicksPerSeconds;
        }
    }

    // laser 特化配置
    public class Plane_Laser
    {
        public int startDelayTicks;
        public int endDelayTicks;
        public int cdTicks;
        public int range;
        public int damage;
        public int atkMoveSpdScaling;
    }
    // plane0 特化配置
    public class Plane0
    {
        public int enhance_extraHp;
        public int enhance_extraShield;
        public int enhance_extraArmor;
    }
    public class Plane1 : Plane0
    {
        public int cdTicks2;
    }
    public class Plane2 : Plane1
    {
        public int damage2;
    }
    public class Plane3 : Plane1
    {
        public int numTotal;
        public int intervalTicks;
    }
}

/***************************************************************************************/
// 原型附加部分( 这些变量在 FullSync 步骤需要填充 )
/***************************************************************************************/

namespace PT
{
    partial class Plane
    {
        /// <summary>
        /// 指向配置项
        /// </summary>
        public Cfg.Plane cfg;

        /// <summary>
        /// 指向玩家
        /// </summary>
        public Player player;

        /// <summary>
        /// 指向场景
        /// </summary>
        public Scene scene;

        /// <summary>
        /// 附加一个位于 Scene.planes / bullets 中的 index, 以便快速删除( 和最后一个交换 )
        /// </summary>
        public int index_in_scene = -1;

        /// <summary>
        /// 附加一个 与 角度 对应 的每帧 xy 增量( 与速度无关 )
        /// </summary>
        public XY xyIncBase;

        /// <summary>
        /// 附加一个 与 角度 对应 的每帧 xy 增量 for move( xyIncBase * moveSpeedPerFrame )
        /// </summary>
        public XY xyIncMove;

        /// <summary>
        /// 附加一个移动速度调整系数
        /// </summary>
        public int xyIncMoveScaling;

        /// <summary>
        /// 与 targetId 同步, 方便使用
        /// </summary>
        public Plane target;

        void FillPartials()
        {
            if (targetId == -1) target = null;
            else target = scene.planes.Find(a => a.id == targetId);
            xyIncBase = XY.Forward(angleTarget);        // 填充帧xy步进值
            xyIncMove = xyIncBase * cfg.moveSpeedPerFrame;      // 填充帧xy步进值 for move
            xyIncMoveScaling = 1;
        }

        /// <summary>
        /// new 出来之后 在与 player, scene bind 好之后, 调用以填充各种 bind 读配置啥的
        /// </summary>
        public virtual void Init(Scene scene, Player player, float a = 0)
        {
            this.id = player.id;
            this.playerId = player.id;
            this.player = player;
            this.scene = scene;
            scene.AddPlane(this);   // will fill index_in_scene

            angle = angleTarget = a;
            bornShieldLifespanTicks = scene.ticks + cfg.bornShieldLifespanTicks;
            moving = false;
            skillSwitches.Clear();
            for (int i = 0; i < cfg.skillIds.Count; i++)
            {
                skillSwitches.Add(false);
            }
            targetId = -1;

            FillPartials();

#if SERVER
#else
            ClientInit();
#endif
        }

        /// <summary>
        /// 用于 FullSync 时还原状态
        /// </summary>
        public virtual void Restore(Scene scene)
        {
            this.scene = scene;
            cfg = scene.cfgs.planes.Find(a => a.id == cfgId);
            player = scene.players.Find(a => a.id == playerId);
            player.plane = this;
            index_in_scene = scene.planes.FindIndex(a => a.id == id);

            FillPartials();

#if SERVER
#else
            ClientRestore();
#endif
        }

        // 三个 update 模拟了三个层级的生命周期事件。可理解为处理优先级排序

        /// <summary>
        /// 主要处理移动逻辑
        /// </summary>
        public virtual void Update0(int ticks)
        {
            // todo: 当有任意攻击锁定目标的技能 cast 时，策划希望 当目标未在射程内时， 飞机往目标飞

            if (angleTarget != angle)
            {
                XY.ChangeAngle(ref angle, angleTarget, cfg.angleLimitPerFrame);
            }
            if (moving)
            {
                xy.Add(xyIncMove * (1 + xyIncMoveScaling.PercentToFloat()));                    // 移动速度受系数影响
                xy.Limit(scene.map.cfg.size.width, scene.map.cfg.size.height);                  // 修正坐标, 不可以飞出 map 边界
            }
        }

        /// <summary>
        /// 主要处理增益逻辑
        /// </summary>
        public virtual void Update1(int ticks)
        {
            // todo: 回血？护盾回复？
            if (cfg.shield > 0)                     // 配有 shield 参数才启用该功能。
            {
                if (shieldBeginRecoverIntervalTicks >= ticks)
                {
                    if (shieldRecoverIntervalTicks >= ticks)
                    {
                        shieldRecoverIntervalTicks = ticks + cfg.shieldRecoverIntervalTicks;
                        if (shield < cfg.shield)
                        {
                            ++shield;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 主要处理减益逻辑( 一般攻击行为都是在派生类中 )
        /// </summary>
        public virtual void Update2(int ticks) { }

        /// <summary>
        /// 在派生类中填充 如果射击瞬间有保存焦点, 也需要清
        /// </summary>
        public virtual void ClearFocus(int planeId)
        {
            if (targetId == planeId)
            {
                targetId = -1;
                target = null;
            }
        }


        // 伤害计算。对于有增强 armor 的子飞机， 必须重写本函数
        public struct HurtResult
        {
            public int hp, shield;
        }

        public virtual HurtResult Hurt(int ticks, int val)
        {
            HurtResult result;
            // 扣甲 扣血. 优先扣甲。 甲没了 接着扣血。 扣血时要用 armor 值来免一部分伤。免不完。至少剩 1 
            if (val > shield)
            {
                val -= shield;
                result.shield = shield;
                shield = 0;

                if (cfg.armor > 0) val -= cfg.armor;
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

        public void Stat(Plane tar)
        {
            if (tar.hp <= 0)
            {
                player.numKills++;
                tar.player.numDeads++;
            }
        }


        /****************************************************************************************************************/
        // 下面是一些被 player 调用以作 消息转发的函数
        /****************************************************************************************************************/


        // 用于 强化数据变化时的 通知. 具体怎么解读强化数据, 派生类说了算
        public virtual void Enhance() { }


        public void Rotate(float a)
        {
            if (angleTarget == a) return;
            angleTarget = a;
            xyIncBase = XY.Forward(angleTarget);        // 填充帧xy步进值
            xyIncMove = xyIncBase * cfg.moveSpeedPerFrame;      // 填充帧xy步进值 for move
#if SERVER
            scene.events.rotates.Add(new Pkg.SC.Events.Rotate
            {
                playerId = playerId,
                angle = a,
                xyIncBase = xyIncBase               // 先下发 server 的计算结果
            });
#endif
        }

        // 客户端专用
        public void SimRotate(float a, XY xyIncBase)
        {
            if (angleTarget == a) return;
            angleTarget = a;
            this.xyIncBase = xyIncBase;
            xyIncMove = xyIncBase * cfg.moveSpeedPerFrame;      // 填充帧xy步进值 for move
        }

        public void Move()
        {
            if (moving == true) return;
            moving = true;
#if SERVER
            scene.events.moves.Add(playerId);
#endif
        }

        public void StopMove()
        {
            if (moving == false) return;
            moving = false;
#if SERVER
            scene.events.stopMoves.Add(playerId);
#endif
        }

        public void Cast(int idx)
        {
            if (idx < 0 || idx >= skillSwitches.Count) return;
            if (skillSwitches[idx] == true) return;
            skillSwitches[idx] = true;
#if SERVER
            scene.events.casts.Add(new Pkg.SC.Events.PlayerAction { playerId = playerId, index = idx });
#endif
        }

        public void StopCast(int idx)
        {
            if (idx < 0 || idx >= skillSwitches.Count) return;
            if (skillSwitches[idx] == false) return;
            skillSwitches[idx] = false;
#if SERVER
            scene.events.stopCasts.Add(new Pkg.SC.Events.PlayerAction { playerId = playerId, index = idx });
#else
            DrawStopFire(angle);
#endif
        }

        public void Aim(int planeId)
        {
            if (planeId == -1)
            {
                CancelAim();
                return;
            }
            if (id != planeId)
            {
                // todo: 阵营, 距离校验
                targetId = planeId;
                target = scene.planes.Find(p => p.id == planeId);
#if SERVER
                scene.events.aims.Add(new Pkg.SC.Events.Aim { playerId = playerId, planeId = planeId });
#else
                target.DrawLocked(playerId, true);
#endif
            }
        }

        public void CancelAim()
        {
            if (targetId != -1)
            {
#if SERVER
                scene.events.aims.Add(new Pkg.SC.Events.Aim { playerId = playerId, planeId = -1 });
#else
                target.DrawLocked(playerId, false);
#endif
                targetId = -1;
                target = null;
            }
        }
    }
}
