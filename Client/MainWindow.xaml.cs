using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using xxlib;
using static Shared;

namespace Client
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FSM fsm;
        int ticks;
        DispatcherTimer t;

        public MainWindow()
        {
            InitializeComponent();

            fsm = new FSM(this);
            fsm.Enter(ticks);

            t = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            t.Tick += T_Tick;
            t.Interval = new TimeSpan(0, 0, 0, 0, 1);
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (!fsm.Update(ticks++))
            {
                Application.Current.Shutdown();
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Left:
                case System.Windows.Input.Key.A:
                    fsm.inputer.SetKeyState(Inputer.Keys.Left, true);
                    break;
                case System.Windows.Input.Key.Up:
                case System.Windows.Input.Key.W:
                    fsm.inputer.SetKeyState(Inputer.Keys.Up, true);
                    break;
                case System.Windows.Input.Key.Right:
                case System.Windows.Input.Key.D:
                    fsm.inputer.SetKeyState(Inputer.Keys.Right, true);
                    break;
                case System.Windows.Input.Key.Down:
                case System.Windows.Input.Key.S:
                    fsm.inputer.SetKeyState(Inputer.Keys.Down, true);
                    break;
                case System.Windows.Input.Key.Space:
                    fsm.inputer.SetKeyState(Inputer.Keys.Space, true);
                    break;
                case System.Windows.Input.Key.Escape:
                    fsm.inputer.SetKeyState(Inputer.Keys.Escape, true);
                    break;
                case System.Windows.Input.Key.Enter:
                    fsm.inputer.SetKeyState(Inputer.Keys.Enter, true);
                    break;
                default:
                    break;
            }
            //Log("key down: " + e.Key.ToString());
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Left:
                case System.Windows.Input.Key.A:
                    fsm.inputer.SetKeyState(Inputer.Keys.Left, false);
                    break;
                case System.Windows.Input.Key.Up:
                case System.Windows.Input.Key.W:
                    fsm.inputer.SetKeyState(Inputer.Keys.Up, false);
                    break;
                case System.Windows.Input.Key.Right:
                case System.Windows.Input.Key.D:
                    fsm.inputer.SetKeyState(Inputer.Keys.Right, false);
                    break;
                case System.Windows.Input.Key.Down:
                case System.Windows.Input.Key.S:
                    fsm.inputer.SetKeyState(Inputer.Keys.Down, false);
                    break;
                case System.Windows.Input.Key.Space:
                    fsm.inputer.SetKeyState(Inputer.Keys.Space, false);
                    break;
                case System.Windows.Input.Key.Escape:
                    fsm.inputer.SetKeyState(Inputer.Keys.Escape, false);
                    break;
                case System.Windows.Input.Key.Enter:
                    fsm.inputer.SetKeyState(Inputer.Keys.Enter, false);
                    break;
                default:
                    break;
            }
            //Log("key up: " + e.Key.ToString());
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                fsm.inputer.SetKeyState(Inputer.Keys.MouseLeft, true);
            }
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            {
                fsm.inputer.SetKeyState(Inputer.Keys.MouseRight, true);
            }
            if (e.ChangedButton == System.Windows.Input.MouseButton.Middle)
            {
                fsm.inputer.SetKeyState(Inputer.Keys.MouseMiddle, true);
            }
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var p = e.GetPosition(root);
            fsm.inputer.mousePos.x = (float)p.X;
            fsm.inputer.mousePos.y = (float)p.Y;
        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                fsm.inputer.SetKeyState(Inputer.Keys.MouseLeft, false);
            }
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            {
                fsm.inputer.SetKeyState(Inputer.Keys.MouseRight, false);
            }
            if (e.ChangedButton == System.Windows.Input.MouseButton.Middle)
            {
                fsm.inputer.SetKeyState(Inputer.Keys.MouseMiddle, false);
            }
        }
    }
}
