using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BareBear_paint_player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point mousePosition = new Point();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton is not MouseButtonState.Pressed)
                return;

            DrawLine(mousePosition, e.GetPosition(DrawingCanvas));
            mousePosition = e.GetPosition(DrawingCanvas);
        }

        private void DrawLine(Point pointStart, Point pointEnd)
        {
            var line = new Line();

            line.Stroke = SystemColors.WindowFrameBrush;
            line.X1 = pointStart.X;
            line.Y1 = pointStart.Y;
            line.X2 = pointEnd.X;
            line.Y2 = pointEnd.Y;
            line.StrokeThickness = ThicknessSlider.Value;

            DrawingCanvas.Children.Add(line);
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState is MouseButtonState.Pressed)
                mousePosition = e.GetPosition(DrawingCanvas);
        }
    }
}