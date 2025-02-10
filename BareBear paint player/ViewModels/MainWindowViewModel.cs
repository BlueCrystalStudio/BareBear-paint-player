using System.Collections.ObjectModel;

namespace BareBear_paint_player.ViewModels;

public sealed class MainWindowViewModel() : ViewModelBase
{
    private string[] repozitories = ["Main"];
    private string currentRepozitory = "Main";

    public string[] Repozitories 
    { 
        get => repozitories; 
        set { 
            repozitories = value; 
            OnPropertyChanged(); 
        }
    }

    public string CurrentRepozitory
    { 
        get => currentRepozitory; 
        set
        {
            currentRepozitory = value;
            OnPropertyChanged();
        }
    }
}
