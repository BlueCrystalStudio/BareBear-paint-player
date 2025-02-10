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
using static BareBear_paint_player.Abstractions.Delegates;
using static BareBear_paint_player.MainWindow;

namespace BareBear_paint_player.Controls;

/// <summary>
/// Interaction logic for CanvasCapture.xaml
/// </summary>
public partial class CanvasCapture : UserControl
{
    LoadDrawingDelegate callback;
    uint index;

    public CanvasCapture(
        uint index,
        MainWindow mainWindowRefference,
        LoadDrawingDelegate onClickCallback,
        Image? image = null)
    {
        InitializeComponent();

        this.index = index;
        callback = onClickCallback;
        CaptureButton.Content = index.ToString();
    }

    private void CaptureButton_Click(object sender, RoutedEventArgs e) => callback.Invoke(index);
}
