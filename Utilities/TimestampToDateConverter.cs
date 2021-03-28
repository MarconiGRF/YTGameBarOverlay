using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace YoutubeGameBarWidget.Utilities
{
    public sealed class TimestampToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string dateString = (string)value;
            CultureInfo currentCulture = new CultureInfo(CultureInfo.InstalledUICulture.Name);
            return DateTime.ParseExact(dateString, Constants.Common.StorableDateFormat, CultureInfo.InvariantCulture).ToString(currentCulture.DateTimeFormat.ShortDatePattern);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string dateString = (string)value;
            return DateTime.ParseExact(dateString, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture).ToString(Constants.Common.StorableDateFormat);
        }
    }
}
