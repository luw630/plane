using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using xxlib;
using static Shared;
using Pkg;
using CS = Pkg.CS;
using SC = Pkg.SC;
using Sim = Pkg.Sim;
using System.Runtime.InteropServices;
using PT;

namespace Server
{
    /// <summary>
    /// 大循环. 不可以有任何阻塞或长时间( 另1帧超时 )运行的代码. 除非服务作用本身就是计算
    /// </summary>
    public static class Looper
    {
        public static bool running = true;
        public static bool stoped = false;
        public static int ticks = 0;

        // 用来改 Sleep(1) 的精度到 1-2ms
        [DllImport("winmm")]
        static extern void timeBeginPeriod(int t);
        [DllImport("winmm")]
        static extern void timeEndPeriod(int t);


        /// <summary>
        /// 用来限制输入发包的上次执行时间点
        /// </summary>
        public static long lastMS = GetCurrentMS();
        /// <summary>
        /// 上次执行时间到这次的经历时间的蓄水池
        /// </summary>
        public static long accumulatMS = 0;


        /// <summary>
        /// 所有玩家均在内，包含掉线但未超时的
        /// </summary>
        public static Scene scene = new Scene();

        /// <summary>
        /// 还没有发  进入游戏  过来的. 收到进入游戏包后，将被移至 Scene
        /// </summary>
        public static List<Player> guests = new List<Player>();

        /// <summary>
        /// 发送复用
        /// </summary>
        public static ByteBuffer bb = new ByteBuffer();

        /// <summary>
        /// 刚 Accept 的临时容器, 将通过 Loop() 转到 guests 容器
        /// </summary>
        public static ConcurrentQueue<Player> accepts = new ConcurrentQueue<Player>();

        /// <summary>
        /// 新用户接入，创建配套的上下文( Entry 专用 )
        /// </summary>
        public static Peer Accept(Peer peer)
        {
            var player = new Player(peer);
            accepts.Enqueue(player);
            return peer;
        }

        /// <summary>
        /// 大循环函数( clients update, scene update )
        /// </summary>
        public static void Loop()
        {
            timeBeginPeriod(1);
            scene.Init(ticks);
            while (running)
            {
                var nowMS = GetCurrentMS();
                var durationMS = nowMS - lastMS;
                lastMS = nowMS;
                if (durationMS > Cfgs.logicFrameMS) durationMS = Cfgs.logicFrameMS;
                accumulatMS += durationMS;
                bool executed = false;
                while (accumulatMS >= Cfgs.logicFrameMS)
                {
                    executed = true;
                    accumulatMS -= Cfgs.logicFrameMS;

                    //Log("Server ticks = " + ticks);

                    // accepts -> guests
                    Player a;
                    while (accepts.TryDequeue(out a))
                    {
                        guests.Add(a);
                        a.Init(scene);
                    }


                    // 处理 guests 看有没有发 join 的( 倒循环以 for fast remove )
                    for (int i = guests.Count - 1; i >= 0; --i)
                    {
                        var g = guests[i];
                        if (!g.Update(ticks) || !g.isGuest)             // 可能是断开或被移至 players & joins 了
                        {
                            guests.FastRemoveAt(i);
                            if (!g.isGuest)                             // 转正
                            {
                                scene.joins.Add(g);                     // 记录到临时容器. 以便发完整同步包
                                scene.AddPlayer(g);
                                Log("Server scene.AddPlayer(g)");
                            }
                        }
                    }

                    //更新场景
                    if (!scene.Update(ticks)) goto TheEnd;

                    // 递增逻辑时间
                    ++ticks;
                }

                // 输出一些统计信息 for debug
                Stat();

                // 省点 cpu
                if (!executed) Thread.Sleep(1);
            }
            TheEnd:
            Thread.MemoryBarrier();
            stoped = true;
            timeEndPeriod(1);
        }

        public static void Start()
        {
            new Thread(Loop).Start();
        }

        /// <summary>
        /// 通知 Loop 结束
        /// </summary>
        public static void Stop()
        {
            running = false;
            while (!stoped) Thread.Sleep(100);   // todo: 超时强行返回?
        }

        public static long statMS = DateTime.MinValue.Ticks / 10000;
        public static long beginMS = DateTime.MinValue.Ticks / 10000;
        /// <summary>
        /// 每秒钟输出一行 流量统计 到 vs 的 output 区
        /// </summary>
        public static void Stat()
        {
            // 每秒钟输出一行 流量统计 到 vs 的 output 区
            var nowMS = DateTime.Now.Ticks / 10000;
            if (nowMS - statMS > 10000)
            {
                statMS = nowMS;
                var elapsedSecs = (nowMS - beginMS) / 10000;
                var sb = (double)Peer.numSendBytes / elapsedSecs;
                var rb = (double)Peer.numRecvedBytes / elapsedSecs;
                Log("total TX RX = " + Peer.numSendBytes + ", " + Peer.numRecvedBytes + ", sec avg TX RX = " + sb + ", " + rb);
            }
        }
    }
}
