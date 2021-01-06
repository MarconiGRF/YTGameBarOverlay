using System;
using Windows.UI.Xaml.Controls;
using YoutubeGameBarWidget.Pages;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// An Warn payload, intended to carry information between Webpage and WarnPage.
    /// </summary>
    class WarnPayload
    {
        public string Message { get; set; }
        public Type DestinationPage { get; set; }
        public Int32 BackoffTime { get; set; }

        public WarnPayload(string message, Type destinationPage, Int32 backoffTime)
        {
            this.Message = message;
            this.DestinationPage = destinationPage;
            this.BackoffTime = backoffTime;
        }
    }
}
