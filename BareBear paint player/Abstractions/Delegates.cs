using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareBear_paint_player.Abstractions;

public class Delegates
{
    public delegate void LoadDrawingDelegate(uint index);
    public delegate void DisplayLogDelegate(string message);
}
