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
        Directory.CreateDirectory(ApplicationPaths.SavePath + CurrentRepozitory);

        repozitories = CheckAllRepozitories();
        mainWindowViewModel.Repozitories = CreateDisplayableRepozitories(repozitories);
        this.mainWindowViewModel = mainWindowViewModel;
    }

    public bool CreateRepozitory(string name)
    {
        if (Directory.Exists(ApplicationPaths.SavePath + name))
        {
            Logger.Log("Repozitory with this name already exists!");
            return false;
        }

        Directory.CreateDirectory(ApplicationPaths.SavePath + name);
        repozitories = [.. repozitories, ApplicationPaths.SavePath + name];
        CurrentRepozitory = name;
        mainWindowViewModel.CurrentRepozitory = name;
        mainWindowViewModel.Repozitories = CreateDisplayableRepozitories(repozitories);

        return true;
    }

    /// <summary>
    /// Attempts to change current repozitory to the one with <see cref="name"/>
    /// </summary>
    /// <param name="name"></param>
    /// <returns>True if Repozitory with <see cref="name"/> exists</returns>
    public bool ChangeRepozitory(string name)
    {
        if (!repozitories.Contains(ApplicationPaths.SavePath + name))
        {
            Logger.Log("Repozitory with this name does not exist!");
            return false;
        }

        CurrentRepozitory = name;
        mainWindowViewModel.CurrentRepozitory = name;
        return true;
    }

    private string[] CreateDisplayableRepozitories(string[] repozitoriesPath)
    {
        var displayableRepos = repozitoriesPath.Select(x => x.Split('\\').Last()).ToArray();
        return displayableRepos;
    }

    private string[] CheckAllRepozitories() => Directory.GetDirectories(ApplicationPaths.SavePath);
}