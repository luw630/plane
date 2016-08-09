using xxlib;
using static Shared;
using CS = Pkg.CS;
using SC = Pkg.SC;

namespace PT
{
    partial class Player
    {
        public State_WaitJoin_t State_WaitJoin;
        public class State_WaitJoin_t : IState
        {
            Player player;

            public State_WaitJoin_t(Player player)
            {
                this.player = player;
            }

#if SERVER
            /// <summary>
            /// 用于判断 建立网络连接 后  接收 Join 的超时
            /// </summary>
            int timeout;

            public void Enter(int ticks)
            {
                timeout = ticks + Cfgs.logicFrameTicksPerSeconds * 5;               // 5 秒钟内收不到 join 指令就断线
                //Log("Server State_WaitJoin Enter. ticks = " + ticks + ". timeout = " + timeout + ". Cfgs.logicFrameTicksPerSeconds = " + Cfgs.logicFrameTicksPerSeconds);
            }
#else
            public void Enter(int ticks) { }
#endif

            public void Leave() { }

            // 逻辑：建立网络连接 之后，客户端应该第一时间发 Join 指令过来. 
            // 收到后, 做相应初始化, 发出 join 的回应(返回服务器分配的id), 进入 WaitBorn 状态( 初始不等 cd )
            // 如果超时就断开.
            public bool Update(int ticks)
            {
#if SERVER
                if (ticks > timeout)
                {
                    Log("Server WaitJoin Update. before Disconnect. ticks = " + ticks + ". timeout = " + timeout);
                    player.Disconnect();
                    player.nextState = null;
                    return false;
                }
#endif

                IBBWriter o;
                while (player.msgs.TryDequeue(out o))
                {
                    if (o.PackageId == CS.Join.packageId)               // 只认 Join 包
                    {
                        var scene = player.scene;
                        player.id = scene.currentPlayerId++;            // ID 自增填充

                        // 阵营选择: 统计两边的人数, 选人少的
                        int count = 0;
                        scene.players.ForEach(p => { if (p.camp == PT.Camps.Alliance) ++count; });
                        player.camp = count * 2 > scene.players.Count ? PT.Camps.Horde : PT.Camps.Alliance;

#if SERVER
                        // 优先发送 自己的 id ( 于完整同步包之前 )
                        var pkg = SC.Join.DefaultInstance;
                        pkg.selfId = player.id;
                        player.Send(pkg);

                        // 通知外循环转正
                        player.isGuest = false;
#endif

                        player.planeCfgId = 0;                          // 刚开始都是基本型
                        player.bornCDTicks = ticks;                     // 立即重生( 这里 return 出去之后会立即再执行一次 Update )
                        return player.ChangeState(player.State_WaitBorn);
                    }
                }
                return true;
            }
        }
    }
}
