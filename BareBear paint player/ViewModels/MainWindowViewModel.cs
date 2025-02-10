using BareBear_paint_player.Logic.LocalRepozitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareBear_paint_player.ViewModels;

public sealed class MainWindowViewModel() : ViewModelBase
{
    public string[] Repozitories { get; set; } = ["Main"];
    public string CurrentRepozitory { get; set; } = "Main";
}
