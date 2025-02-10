using BareBear_paint_player.Controls;
using BareBear_paint_player.Logic;
using BareBear_paint_player.Logic.Drawing;
using BareBear_paint_player.Logic.LocalRepozitories;
using BareBear_paint_player.Logic.Serialization;
using BareBear_paint_player.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static BareBear_paint_player.Abstractions.Delegates;

namespace BareBear_paint_player;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    bool isAnimationPlaying;
    string currentRepozitory;
    Point mousePosition = new Point();
    DrawingMode drawingMode = DrawingMode.Line;

    IStreamManager streamManager;
    ApplicationStyleManager styleManager;
    LoadDrawingDelegate loadDrawingDelegate;
    RepozitoryManager repozitoryManager;
    Dictionary<string, List<uint>> brancheImageIndexesMap;

    public MainWindow(
        IStreamManager streamManager,
        MainWindowViewModel viewModel,
        RepozitoryManager repozitoryManager)
    {
        styleManager = new(Resources);
        brancheImageIndexesMap = new();
        this.streamManager = streamManager;
        this.repozitoryManager = repozitoryManager;
        loadDrawingDelegate += LoadDrawing;

        currentRepozitory = repozitoryManager.CurrentRepozitory;

        InitializeComponent();
        DataContext = viewModel;

        AddFramesForCurrentRepo();
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

    private void AddFrameButton_Click(object sender, RoutedEventArgs e)
    {
        var canvasIndex = streamManager.Save(DrawingCanvas);
        var capture = new CanvasCapture(canvasIndex, this, loadDrawingDelegate);

        HistoryStackPanel.Children.Add(capture);

        if (!brancheImageIndexesMap.ContainsKey(currentRepozitory))
            brancheImageIndexesMap.Add(currentRepozitory, new List<uint>());

        brancheImageIndexesMap[currentRepozitory].Add(canvasIndex);
    }

    public void AddFramesForCurrentRepo()
    {
        HistoryStackPanel.Children.Clear();

        if (!brancheImageIndexesMap.ContainsKey(currentRepozitory))
            brancheImageIndexesMap.Add(currentRepozitory, new List<uint>());

        var canvasIndexes = streamManager.GetIndexes();
        foreach (var index in canvasIndexes)
        {
            var capture = new CanvasCapture(index, this, loadDrawingDelegate);

            HistoryStackPanel.Children.Add(capture);
            brancheImageIndexesMap[currentRepozitory].Add(index);
        }
    }

    public void LoadDrawing(uint index)
    {
        var loadedCanvas = streamManager.Load(index);
        var childrenList = loadedCanvas.Children.Cast<UIElement>().ToArray();
        DrawingCanvas.Children.Clear();

        foreach (var children in childrenList)
        {
            loadedCanvas.Children.Remove(children);
            DrawingCanvas.Children.Add(children);
        }
    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if(brancheImageIndexesMap[currentRepozitory].Count is 0)
        {
            Logger.Log("No drawings to play!");
            return;
        }

        
        // Switch state
        isAnimationPlaying = !isAnimationPlaying;

        // Change icon - TODO: Create & Use convertor
        PlayButtonImage.Source =
            isAnimationPlaying ?
            new BitmapImage(new Uri(@"Images/stop-button.png", UriKind.Relative)) :
            new BitmapImage(new Uri(@"Images/PlayIcon.png", UriKind.Relative));

        if (!isAnimationPlaying)
            return;

        var framerate = 1000 / (Framerate.Value ??= 1);

        await PlayDrawings(framerate);
    }

    private async Task PlayDrawings(double delay)
    {
        while (isAnimationPlaying)
        {
            foreach (var canvasIndex in brancheImageIndexesMap[currentRepozitory])
            {
                LoadDrawing(canvasIndex);
                await Task.Delay((int)delay);

                if (!isAnimationPlaying)
                    break;
            }
        }
    }

    private void NewRepozitoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NewRepozitoryNameTextBox.Text))
        {
            Logger.Log("Repozitory name cannot be empty!");
            return;
        }

        repozitoryManager.CreateRepozitory(NewRepozitoryNameTextBox.Text);
    }

    private void Repozitories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var selectedRepozitory = (string)Repozitories.SelectedItem;
        if (repozitoryManager.ChangeRepozitory(selectedRepozitory) is false)
            return;

        currentRepozitory = selectedRepozitory;
        AddFramesForCurrentRepo();

        DrawingCanvas.Children.Clear();
    }
}