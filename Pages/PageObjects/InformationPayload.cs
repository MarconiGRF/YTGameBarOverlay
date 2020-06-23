using System;
using YoutubeGameBarOverlay;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
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
