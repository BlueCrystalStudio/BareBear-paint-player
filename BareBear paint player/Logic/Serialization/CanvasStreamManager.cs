using BareBear_paint_player.Logic.LocalRepozitories;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using Path = System.IO.Path;
using XamlWriter = System.Windows.Markup.XamlWriter;

namespace BareBear_paint_player.Logic.Serialization;

class CanvasStreamManager : IStreamManager
{
    uint filesInRepozitoryCount;
    
    string FileName => $"Canvas{filesInRepozitoryCount}.xaml";
    string RepozitoryPath => Path.Combine(ApplicationPaths.SavePath, repozitoryManager.CurrentRepozitory);

    RepozitoryManager repozitoryManager;

    public CanvasStreamManager(RepozitoryManager repozitoryManager) 
    {
        this.repozitoryManager = repozitoryManager;
        Directory.CreateDirectory(RepozitoryPath);    // This only create directory if it does not exist yet

        filesInRepozitoryCount = GetFilesCount();
    }

    /// <summary>
    /// Saves <see cref=":children"/> to <see cref=":SavePath"/>
    /// </summary>
    /// <param name="canvas">Saved object</param>
    /// <returns>Index of saved collection</returns>
    public uint Save(Canvas canvas)
    {
        var xaml = XamlWriter.Save(canvas);

        filesInRepozitoryCount++;
        File.WriteAllText(Path.Combine(RepozitoryPath, FileName), xaml);

        return filesInRepozitoryCount;
    }

    public Canvas Load(uint index)
    {
        var stream = File.Open(
            Path.Combine(
                RepozitoryPath,
                GetFileName(index)), FileMode.Open, FileAccess.Read);

        var loadedCanvas = XamlReader.Load(stream) as Canvas;

        if(loadedCanvas is null)
            throw new FileLoadException("Failed to load canvas");

        return loadedCanvas;
    }

    private string GetFileName(uint index) => $"Canvas{index}.xaml";
    private uint GetFilesCount() => (uint)Directory.GetFiles(RepozitoryPath, "*", SearchOption.AllDirectories).Length;
}
