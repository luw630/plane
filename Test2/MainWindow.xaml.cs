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

namespace Test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scene scene = new Scene();

        DispatcherTimer t;
        int ticks = 0;
        public long lastMS = GetCurrentMS();
        public long accumulatMS = 0;

        private void T_Tick(object sender, EventArgs e)
        {
            var nowMS = GetCurrentMS();
            var durationMS = nowMS - lastMS;
            lastMS = nowMS;
            if (durationMS > Cfgs.logicFrameMS) durationMS = Cfgs.logicFrameMS;
            accumulatMS += durationMS;
            //bool executed = false;
            while (accumulatMS >= Cfgs.logicFrameMS)
            {
                //executed = true;
                accumulatMS -= Cfgs.logicFrameMS;

                scene.Update(ticks);

                ++ticks;
            }
            //if (!executed) Thread.Sleep(1);
        }

        public MainWindow()
        {
            InitializeComponent();

            t = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            t.Tick += T_Tick;
            t.Interval = new TimeSpan(0, 0, 0, 0, 1);
            t.Start();


            scene.Init(root, ticks);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
