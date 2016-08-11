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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Shared;
using xxlib;
using System.Windows.Threading;
using System.Diagnostics;


public class Scene
{
    Fortrees fortrees1 = new Fortrees();
    Fortrees fortrees2 = new Fortrees();

    public void Init(Canvas root, int ticks)
    {
        fortrees1.Init(fortrees2, root, ticks, 0);
        fortrees2.Init(fortrees1, root, ticks);
    }
    public void Update(int ticks)
    {
        fortrees1.Update(ticks);
        fortrees2.Update(ticks);
    }
}
