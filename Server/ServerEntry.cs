using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System.Collections.Generic;
using static Shared;

namespace Server
{
    /// <summary>
    /// 光子服务入口( 名称端口要配入 PhotonServer.config  )
    /// </summary>
    public class Entry : ApplicationBase
    {
        /// <summary>
        /// 服务启动事件: 做各种初始化, 数据加载...
        /// </summary>
        protected override void Setup()
        {
            Looper.Start();
        }

        /// <summary>
        /// 服务停止事件: 资源回收，通知，日志啥的？
        /// </summary>
        protected override void TearDown()
        {
            Looper.Stop();
        }

        /// <summary>
        /// 以客户端方式连接过来的 accept 操作( 可以做合法性验证，或 switch new 相应的 peer 类型 )
        /// </summary>
        protected override PeerBase CreatePeer(InitRequest ir)
        {
            Log(ir.RemoteIP + ":" + ir.RemotePort + " connected.");
            return Looper.Accept(new Peer(ir));
        }

    }
}
