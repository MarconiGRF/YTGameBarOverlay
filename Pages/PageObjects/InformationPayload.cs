using System;
using YTGameBarOverlay;

namespace YTGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// An Information payload, intended to carry information between MainPage and Webpage.
    /// </summary>
    class InformationPayload
    {
        public Uri VideoURI { get; set; }

        public InformationPayload(Uri videoUri)
        {
            this.VideoURI = videoUri;
        }
    }
}
