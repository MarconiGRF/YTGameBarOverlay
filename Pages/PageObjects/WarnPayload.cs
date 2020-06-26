using System;
using YoutubeGameBarWidget.Pages;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// An Warn payload, intended to carry information between Webpage and WarnPage .
    /// </summary>
    class WarnPayload
    {
        public string Message { get; set; }

        public WarnPayload(string message)
        {
            this.Message = message;
        }
    }
}
