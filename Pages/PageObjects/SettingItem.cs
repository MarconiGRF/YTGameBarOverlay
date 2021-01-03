using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// A class to hold the values of each setting. 
    /// It has a "Display Value" to be shown to user and a "Raw Value" to be processed.
    /// </summary>
    public class SettingItem
    {
        public string DisplayValue { get; set; }

        public string RawValue { get; set; }

        public ThemeResources ColorResources { get; set; }

        public SettingItem(string DisplayValue, string RawValue)
        {
            this.DisplayValue = DisplayValue;
            this.RawValue = RawValue;
            this.ColorResources = Painter.GetTheme();
        }
    }
}
