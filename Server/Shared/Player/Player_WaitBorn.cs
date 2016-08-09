using xxlib;
using CS = Pkg.CS;

namespace PT
{
    partial class Player
    {
        public State_WaitBorn_t State_WaitBorn;
        public class State_WaitBorn_t : IState
        {
            Player player;
            public State_WaitBorn_t(Player player)
            {
                this.player = player;
            }

            public void Enter(int ticks) { }
            public void Leave() { }

            public bool Update(int ticks)
            {
                var scene = player.scene;
                IBBWriter o;
                while (player.msgs.TryDequeue(out o))               // 这里忽略掉先前玩家发送的其它类型包( 客户端认为没死故会产生操作数据包 )
                {
                    switch (o.PackageId)                            // 只认 Quit 包
                    {
                        case CS.Quit.packageId:
                            {
                                return player.Quit();
                            }
                        default:
                            break;
                    }
                }

                // 自动重生时间到( 刚 join 的玩家会立即重生 )
                if (player.bornCDTicks <= ticks)
                {
                    // 创建飞机
                    var p = player.CreatePlane();

                    // 设置地图随机位置
                    p.xy.x = scene.rnd.Next() % (scene.map.cfg.size.width / 2) + scene.map.cfg.size.width / 4;
                    p.xy.y = scene.rnd.Next() % (scene.map.cfg.size.height / 2) + scene.map.cfg.size.height / 4;

                    // 切状态
                    return player.ChangeState(player.State_Alive);
                }
                return true;
            }
        }
    }
}
