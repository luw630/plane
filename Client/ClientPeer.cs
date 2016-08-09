using System;
using ExitGames.Client.Photon;
using static Shared;
using xxlib;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Shapes;
using System.Windows.Controls;
using Pkg;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace Client
{
    /// <summary>
    /// 客户网络端上下文, 专注于收发包, 有收包队列
    /// </summary>
    public class Peer :  IPhotonPeerListener
    {
        public static long numRecvedBytes = 0;
        public static long numSendBytes = 0;

        public Peer()
        {
            peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        }

        public void Update()
        {
            peer.Service();
        }

        public bool Connect(string serverAddress)
        {
            return peer.Connect(serverAddress, "");
        }
        public void Disconnect()
        {
            peer.Disconnect();
        }

        // ***********************************************************************
        // 收包队列 与 发送函数
        // ***********************************************************************

        public PhotonPeer peer;
        public ByteBuffer bb = new ByteBuffer();
        Dictionary<byte, object> oo = new Dictionary<byte, object>();

        public bool Send(byte[] buf)
        {
            numSendBytes += buf.Length;
            oo[0] = buf;
            return peer.OpCustom(0, oo, true);
        }

        public bool Send(ByteBuffer bb)
        {
            return Send(bb.DumpData());
        }

        public bool Send(params IBBWriter[] os)
        {
            bb.Clear();
            foreach (var o in os)
            {
                bb.Write(o.PackageId);
                bb.Write(o);
            }
            return Send(bb);
        }


        /// <summary>
        /// client peer 的 OnOperationRequest 收到的的数据( 因为是在接收数据的线程中操作，故 concurrent )
        /// </summary>
        public Queue<IBBWriter> msgs = new Queue<IBBWriter>();


        // 针对同一 peer 的数据会确保不会多线同时投递, 故这些可以复用
        public ByteBuffer bb_read = new ByteBuffer();                               // for recv

        /// <summary>
        /// msg[0] -> bytebuffer -> do{ decode -> object => msgs } while offset < dataLen
        /// </summary>
        void PushMsgByPeer(byte[] buf)
        {
            bb_read.Assign(buf, buf.Length);
            while (bb_read.offset < bb_read.dataLen)
            {
                var o = Pkg.Pkg2Obj.Convert(bb_read);
                if (o != null) msgs.Enqueue(o);
                else
                {
                    msgs.Enqueue(Pkg.Sim.Disconnect.DefaultInstance);
                    break;
                }
            }
        }

        // ***********************************************************************
        // IPhotonPeerListener
        // ***********************************************************************

        /// <summary>
        /// 接收 server 端用 OpCustom 发送的数据. 塞入 msgs
        /// </summary>
        void IPhotonPeerListener.OnOperationResponse(OperationResponse or)
        {
            var buf = (byte[])or.Parameters[0];
            numRecvedBytes += buf.Length;
            PushMsgByPeer(buf);
        }

        /// <summary>
        /// 连接 , 断开/出错 状态变化通知. 塞入 msgs
        /// </summary>
        void IPhotonPeerListener.OnStatusChanged(StatusCode sc)
        {
            Log("OnStatusChanged " + sc.ToString());
            PushMsgByPeer(sc == StatusCode.Connect
                ? Pkg.Sim.Connect.DefaultPackageData
                : Pkg.Sim.Disconnect.DefaultPackageData);
        }

        // unused
        void IPhotonPeerListener.DebugReturn(DebugLevel level, string message) { }
        void IPhotonPeerListener.OnEvent(EventData eventData) { }

    }
}
