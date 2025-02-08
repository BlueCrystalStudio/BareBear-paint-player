using System.Windows.Controls;

namespace BareBear_paint_player.Logic.Serialization;

public interface IStreamManager
{
    public void Save(UIElementCollection children);
}