using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg
{
    partial class Fortress
    {
        int moveSpeedPerFrame;
        int startMoveTimeTicks;
        int firstAttackTimeTicks;
        int atkIntervalTicks;
        int restrictionTimeTicks;

        // todo: 将速度换算成角度变化  C = 2 PI r

        // todo: 子弹飞行速度? 子弹体积? 发射点? 起始判定距离( 子弹飞出炮口之后才会碰撞. 在此之前是被炮口 遮挡的 )?
        // 子弹每秒产生多少次伤害, 每伤减多少血 ?

        public void Init(Cfgs cfgs)
        {
            moveSpeedPerFrame = moveSpeed.PerFrame();
            startMoveTimeTicks = startMoveTime.ToTicks();
            firstAttackTimeTicks = firstAttackTime.ToTicks();
            atkIntervalTicks = atkInterval.ToTicks();
            restrictionTimeTicks = restrictionTime.ToTicks();
        }
    }
}

namespace PT
{
    partial class Fortrees
    {
        public Cfg.Fortress cfg;
    }
}
