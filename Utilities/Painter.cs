using System;
using System.Threading;
using Windows.UI;

namespace YTGameBarWidget.Utilities
{
    public class Painter
    {
        /// <summary>
        /// Asynchronously runs an UI updated defined by the given method using the UIUpdate thread.
        /// </summary>
        /// <param name="uiMethod">The UI update method to be executed.</param>
        public static void RunUIUpdateByMethod(Action uiMethod)
        {
            Thread UIThread = new Thread(new ThreadStart(uiMethod));
            UIThread.Start();
        }

        /// <summary>
        /// Gets the theme resources based on the user preferences.
        /// </summary>
        /// <returns></returns>
        public static ThemeResources GetTheme()
        {
            ThemeResources Theme = new ThemeResources();
            Theme.AccentColor = (string)Utils.GetSettingValue(Constants.Settings.AccentColors["varname"]);
            Theme.SecondaryColor = (string)Utils.GetSettingValue(Constants.Settings.SecondaryColors["varname"]);
            Theme.AuxiliaryColor = (string)Utils.GetSettingValue(Constants.Settings.AuxiliaryColors["varname"]);

            return Theme;
        }

        /// <summary>
        /// Gets a Color from a Hex value converting it to RGBA to do so.
        /// </summary>
        /// <param name="hex">The Hex string value of the color.</param>
        /// <returns></returns>
        public static Color GetFromHex(string hex)
        {
            hex = hex.Replace("#", Constants.Common.EmptyString);
            byte a = 255;
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));

            return Color.FromArgb(a, r, g, b);
        }
    }
}
