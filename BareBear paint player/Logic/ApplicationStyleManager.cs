using System.Windows.Documents;
using System.Windows;
using Wpf.Ui.Appearance;
using System.Windows.Media;

namespace BareBear_paint_player.Logic;

internal class ApplicationStyleManager
{
    public ApplicationStyleManager(ResourceDictionary resources)
    {
        // Apply Dark mode
        ApplicationAccentColorManager.Apply(
            Color.FromArgb(255, 76, 194, 255),
            ApplicationTheme.Dark,
            true
        );

        // Set Paragrams to be without Margin
        Style noSpaceStyle = new Style(typeof(Paragraph));
        noSpaceStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0)));
        resources.Add(typeof(Paragraph), noSpaceStyle);
    }
}
