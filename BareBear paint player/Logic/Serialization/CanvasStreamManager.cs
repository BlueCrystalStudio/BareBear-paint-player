using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using Path = System.IO.Path;
using XamlWriter = System.Windows.Markup.XamlWriter;

namespace BareBear_paint_player.Logic.Serialization;

class CanvasStreamManager : IStreamManager
{
    int filesCount;
    const string SavePath = @"C:\PaintPlayer\CanvasStorage\";
    string FileName => $"Canvas{filesCount}.xaml";

    private Dictionary<uint, UIElementCollection> CanvasDrawingCache = new();


    public CanvasStreamManager() 
    {
        Directory.CreateDirectory(SavePath);    // This only create directory if it does not exist yet

        filesCount = GetFilesCount();
    }


    public void Save(UIElementCollection children)
    {
        string xaml = XamlWriter.Save(children);

        filesCount++;
        File.WriteAllText(Path.Combine(SavePath, FileName), xaml);
    }

    public UIElementCollection Load(uint index)
    {
        // Load from file and Cache drawings
        if (!CanvasDrawingCache.ContainsKey(index))
        {
            string xaml = File.ReadAllText(Path.Combine(SavePath, GetFileName(index)));
            UIElementCollection children = (UIElementCollection)XamlReader.Parse(xaml);
            CanvasDrawingCache.Add(index, children);
        }

        return CanvasDrawingCache[index];
    }

    private string GetFileName(uint index) => $"Canvas{index}.xaml";
    private int GetFilesCount() => Directory.GetFiles(SavePath, "*", SearchOption.AllDirectories).Length;

}
