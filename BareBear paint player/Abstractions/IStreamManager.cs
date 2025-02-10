using System.Windows.Controls;

namespace BareBear_paint_player.Logic.Serialization;

public interface IStreamManager
{
    public uint Save(Canvas children);
    public Canvas Load(uint index);
    uint[] GetIndexes();
}