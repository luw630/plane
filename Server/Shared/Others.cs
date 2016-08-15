using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Navigation;
using System.Windows.Shapes;
using static Shared;
using xxlib;

using System.Diagnostics;


// public static class DrawUtils
// {
//     public static void SetPositon(this UIElement o, XY xy)
//     {
//         Canvas.SetLeft(o, xy.x);
//         Canvas.SetTop(o, xy.y);
//     }
//     public static XY GetPosition(this UIElement o)
//     {
//         XY rtv;
//         rtv.x = (float)Canvas.GetLeft(o);
//         rtv.y = (float)Canvas.GetTop(o);
//         return rtv;
//     }
// }

public static class ListXYExt
{
    public static XY CircleAt(this List<XY> list, int idx)
    {
        if (idx < 0) return list[list.Count + idx];
        return list[idx % list.Count];
    }
}

public struct Size
{
    public int width, height;
    public Size(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}


