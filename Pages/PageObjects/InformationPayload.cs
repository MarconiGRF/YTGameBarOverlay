using System;
using YoutubeGameBarOverlay;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// An Information payload, intended to carry information between MainPage and Webpage.
    /// </summary>
    class InformationPayload
    {
        public Uri VideoURI { get; set; }
        public MainPage MainPage { get; set; }

        public InformationPayload(Uri videoUri, MainPage mainPage)
        {
            this.VideoURI = videoUri;
            this.MainPage = mainPage;
        }
    }
}
