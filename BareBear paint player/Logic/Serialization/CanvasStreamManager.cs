using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using Path = System.IO.Path;
using XamlWriter = System.Windows.Markup.XamlWriter;

namespace BareBear_paint_player.Logic.Serialization;

class CanvasStreamManager : IStreamManager
{
    uint filesCount;
    const string SavePath = @"C:\PaintPlayer\CanvasStorage\";
    string FileName => $"Canvas{filesCount}.xaml";

    public CanvasStreamManager() 
    {
        Directory.CreateDirectory(SavePath);    // This only create directory if it does not exist yet

        filesCount = GetFilesCount();
    }

    /// <summary>
    /// Saves <see cref=":children"/> to <see cref=":SavePath"/>
    /// </summary>
    /// <param name="canvas">Saved object</param>
    /// <returns>Index of saved collection</returns>
    public uint Save(Canvas canvas)
    {
        string xaml = XamlWriter.Save(canvas);

        filesCount++;
        File.WriteAllText(Path.Combine(SavePath, FileName), xaml);

        return filesCount;
    }

    public Canvas Load(uint index)
    {
        FileStream stream = File.Open(Path.Combine(SavePath, GetFileName(index)), FileMode.Open, FileAccess.Read);
        var loadedCanvas = XamlReader.Load(stream) as Canvas;

        if(loadedCanvas is null)
            throw new FileLoadException("Failed to load canvas");

        return loadedCanvas;
    }

    private string GetFileName(uint index) => $"Canvas{index}.xaml";
    private uint GetFilesCount() => (uint)Directory.GetFiles(SavePath, "*", SearchOption.AllDirectories).Length;
}
