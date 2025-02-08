using BareBear_paint_player.Controls;
using BareBear_paint_player.Logic;
using BareBear_paint_player.Logic.Drawing;
using BareBear_paint_player.Logic.Serialization;
using System.IO;
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
using System.Xaml;
using static BareBear_paint_player.Abstractions.Delegates;
using static BareBear_paint_player.MainWindow;

namespace BareBear_paint_player;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Point mousePosition = new Point();
    DrawingMode drawingMode = DrawingMode.Line;

    IStreamManager streamManager;
    ApplicationStyleManager styleManager;
    LoadDrawingDelegate loadDrawingDelegate;

    public MainWindow(IStreamManager streamManager)
    {
        styleManager = new(Resources);
        this.streamManager = streamManager;

        DataContext = this;
        loadDrawingDelegate += LoadDrawing;

        InitializeComponent();
    }

    private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton is not MouseButtonState.Pressed)
            return;

        switch(drawingMode)
        {
            case DrawingMode.Line:
                DrawLine(mousePosition, e.GetPosition(DrawingCanvas));
                break;
            case DrawingMode.Remove:
                RemoveDrawing(e.OriginalSource);
                break;
        }

        mousePosition = e.GetPosition(DrawingCanvas);
    }

    private void RemoveDrawing(object drawing)
    {
        if(drawing is Line)
        {
            DrawingCanvas.Children.Remove((Line)drawing);
        }
    }

    private void DrawLine(Point pointStart, Point pointEnd)
    {
        var line = new Line();

        line.Stroke = ColorPicker.SelectedBrush;
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

    private void RemoveButton_Checked(object sender, RoutedEventArgs e) => drawingMode = DrawingMode.Remove;
    private void DrawButton_Checked(object sender, RoutedEventArgs e) => drawingMode = DrawingMode.Line;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var canvasIndex = streamManager.Save(DrawingCanvas);
        var capture = new CanvasCapture(canvasIndex, this, loadDrawingDelegate);

        HistoryStackPanel.Children.Add(capture);
    }

    public void LoadDrawing(uint index)
    {
        DrawingCanvas.Children.Clear();

        var loadedCanvas = streamManager.Load(index);


        var childrenList = loadedCanvas.Children.Cast<UIElement>().ToArray();
        DrawingCanvas.Children.Clear();

        foreach (var c in childrenList)
        {
            loadedCanvas.Children.Remove(c);
            DrawingCanvas.Children.Add(c);
        }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {

    }
}