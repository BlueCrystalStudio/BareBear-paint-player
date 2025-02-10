using BareBear_paint_player.ViewModels;
using System.IO;

namespace BareBear_paint_player.Logic.LocalRepozitories;

public class RepozitoryManager
{
    public string CurrentRepozitory { get; private set; } = "Main";

    private string[] repozitories;
    MainWindowViewModel mainWindowViewModel;

    public RepozitoryManager(MainWindowViewModel mainWindowViewModel)
    {
        repozitories = CheckAllRepozitories();
        mainWindowViewModel.Repozitories = repozitories;
        this.mainWindowViewModel = mainWindowViewModel;
    }

    private string[] CheckAllRepozitories() => Directory.GetDirectories(ApplicationPaths.SavePath);

    public void CreateRepozitory(string name)
    {
        if (Directory.Exists(ApplicationPaths.SavePath + name))
        {
            Logger.Log("Repozitory with this name already exists!");
            return;
        }

        Directory.CreateDirectory(ApplicationPaths.SavePath + name);
        repozitories.Append(name);
    }
}