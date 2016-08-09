using xxlib;
using CS = Pkg.CS;
using Sim = Pkg.Sim;

namespace PT
{
    partial class Player
    {
        public State_Alive_t State_Alive;
        public class State_Alive_t : IState
        {
            Player player;
            public State_Alive_t(Player player)
            {
                this.player = player;
            }

            public void Enter(int ticks)
            {
#if SERVER
                Shared.Log("******* SERVER player id = " + player.id + " AliveEnter");
#else
                Shared.Log("CLIENT player id = " + player.id + " AliveEnter");
#endif
            }
            public void Leave() { }


            public bool Update(int ticks)
            {
                var scene = player.scene;
                IBBWriter o;
                if (player.msgs.TryDequeue(out o))                  // 每 frame 只处理一个包( 客户端应该也遵循这样的逻辑. 真有多种操作, 则需要合并 )
                {
                    switch (o.PackageId)                            // 只认 非 CS.Join
                    {
                        case CS.Move.packageId:
                            if (player.plane != null) player.plane.Move();
                            break;
                        case CS.StopMove.packageId:
                            if (player.plane != null) player.plane.StopMove();
                            break;
                        case CS.Cast.packageId:
                            {
                                var pkg = o as CS.Cast;
                                if (player.plane != null) player.plane.Cast(pkg.index);
                                break;
                            }
                        case CS.StopCast.packageId:
                            {
                                var pkg = o as CS.StopCast;
                                if (player.plane != null) player.plane.StopCast(pkg.index);
                                break;
                            }
                        case CS.Rotate.packageId:
                            {
                                var pkg = o as CS.Rotate;
                                if (player.plane != null) player.plane.Rotate(pkg.angle);
                                break;
                            }
                        case Sim.Rotate.packageId:
                            {
                                var pkg = o as Sim.Rotate;
                                if (player.plane != null) player.plane.SimRotate(pkg.angle, pkg.xyIncBase);
                                break;
                            }
                        case CS.Aim.packageId:
                            {
                                var pkg = o as CS.Aim;
                                if (player.plane != null) player.plane.Aim(pkg.planeId);
                                break;
                            }
                        case CS.CancelAim.packageId:
                            if (player.plane != null) player.plane.CancelAim();
                            break;

                        case CS.Enhance.packageId:
                            {
                                var pkg = o as CS.Enhance;
                                player.Enhance(pkg.index);
                                break;
                            }
                        case CS.Evolution.packageId:
                            {
                                var pkg = o as CS.Evolution;
                                player.Evolution(pkg.index);
                                break;
                            }
                        case CS.Quit.packageId:
                        case Sim.Disconnect.packageId:
                            return player.Quit();
                        default:
                            break;
                    }
                }
                return true;
            }

        }
    }
}
