using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System.Collections.Generic;
using static Shared;

namespace Server
{
    /// <summary>
    /// 与客户端建立的连接对象( 理论上讲可能存在很多种, 位于不同容器啥的 )
    /// </summary>
    public class Peer : PeerBase
    {
        /// <summary>
        /// 指向客户端上下文( 初始为 GuestContext, 确认身份后方指向具体 ClientContext )
        /// </summary>
        public PT.Player player;
        public static long numRecvedBytes = 0;
        public static long numSendBytes = 0;

        /// <summary>
        /// 实现必须的构造函数, 转发参数, 以及初始化变量
        /// </summary>
        public Peer(InitRequest ir) : base(ir) { }

        /// <summary>
        /// 断线事件 投递
        /// </summary>
        protected override void OnDisconnect(DisconnectReason r, string d)
        {
            player?.PushMsgByPeer(Pkg.Sim.Disconnect.DefaultPackageData);
        }


        /// <summary>
        /// 收到数据 投递( 当前忽略了 SendParameters 处理。一般没这个需求 )
        /// </summary>
        protected override void OnOperationRequest(OperationRequest cor, SendParameters csp)
        {
            var buf = (byte[])cor.Parameters[0];
            System.Threading.Interlocked.Add(ref numRecvedBytes, buf.Length);
            player?.PushMsgByPeer(buf);
        }

        /// <summary>
        /// 默认的发送用参数，这里选用稳定传输
        /// </summary>
        private static readonly SendParameters sp = new SendParameters { Unreliable = false };

        /// <summary>
        /// Send 操作复用( 本代码业务逻辑确保了单线程写 )
        /// </summary>
        public Dictionary<byte, object> oo = new Dictionary<byte, object>();

        /// <summary>
        /// 发送一段 buf. 成功的将数据交给系统发送则返回 true ( 本函数于本应用中只可能单线程被调 )
        /// </summary>
        public bool Send(byte[] buf)
        {
            numSendBytes += buf.Length;
            oo[0] = buf;
            return SendOperationResponse(new OperationResponse(0, oo), sp) == SendResult.Ok;
        }
    }

}
