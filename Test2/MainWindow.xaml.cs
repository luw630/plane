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

namespace Test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Polygon polygon;

        public MainWindow()
        {
            InitializeComponent();


            polygon = new Polygon { StrokeThickness = 1, Fill = Brushes.DarkGray };
            //polygon.Points.Add(new Point(CfgsPlane.radius,0));
            //polygon.Points.Add(new Point(-CfgsPlane.radius, -CfgsPlane.radius));
            //polygon.Points.Add(new Point(-CfgsPlane.radius, CfgsPlane.radius));
            polygon.RenderTransform = new RotateTransform(0.7f * 180.0);
            polygon.SetPositon(400, 300);
            root.Children.Add(polygon);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(root);
            Log(p.X + "," + p.Y);
            var a = xxlib.XY.Angle(new xxlib.XY { x = 400, y = 300 }, new xxlib.XY { x = (float)p.X, y = (float)p.Y });
            Log(a.ToString());
            ((RotateTransform)polygon.RenderTransform).Angle = -a * 180.0;
        }
    }
}

public static class DrawUtils
{
    public static void SetPositon(this UIElement o, double x, double y)
    {
        Canvas.SetLeft(o, x);
        Canvas.SetTop(o, y);
    }

}