using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxlib;

public static partial class Shared
{
    public static void Log(string s)
    {
        System.Diagnostics.Debug.WriteLine(s);
    }

    public static void LaunchDebug()
    {
        // 强制启动 vs 调试
        System.Diagnostics.Debugger.Launch();
    }

    /// <summary>
    /// 交换删除法. 如果有产生交换行为则返加 true, 没有( 删的元素就是最后一个 ) 则返回 false
    /// </summary>
    public static bool FastRemoveAt<T>(this List<T> list, int idx)
    {
        if (list.Count == idx + 1)  // 要移除的元素位于 list 尾
        {
            list.RemoveAt(idx);
            return false;
        }
        else                        // 复制最后一个到当前位置, 删最后一个
        {
            list[idx] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return true;
        }
    }

    public static long GetCurrentMS()
    {
        return (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
    }

    public static bool TryDequeue<T>(this Queue<T> q, out T v)
    {
        if (q.Count == 0)
        {
            v = default(T);
            return false;
        }
        v = q.Dequeue();
        return true;
    }

    public static float PercentToFloat(this int val)
    {
        return (float)val / 100;
    }

    public static int ToTicks(this int ms)
    {
        var rtv = ms / Cfgs.logicFrameMS;
        if (ms != 0 && rtv == 0) return 1;
        return rtv;
    }
    public static int PerFrame(this int val)
    {
        var rtv = val / Cfgs.logicFrameTicksPerSeconds;
        if (val != 0 && rtv == 0) return 1;
        return rtv;
    }
}
