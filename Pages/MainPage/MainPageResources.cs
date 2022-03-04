using System;
using System.Xml.Serialization;

namespace YTGameBarWidget.Pages
{
    /// <summary>
    /// The resources for the Main Page.
    /// Its values are intended to be set with the content on the user language's XML and with user preferences.
    /// </summary>
    [XmlRoot("MainPageResources")]
    public class MainPageResources
    {
        [XmlElement("Greeting")]
        public String Greeting { get; set; }

        [XmlElement("SearchFieldPlaceholder")]
        public String SearchFieldPlaceholder { get; set; }

        [XmlElement("VideoNotSelectedError")]
        public String VideoNotSelectedError { get; set; }

        [XmlElement("URLNotValidError")]
        public String URLNotValidError { get; set; }

        [XmlElement("SearchNotAvailableError")]
        public String SearchNotAvailableError { get; set; }

        [XmlElement("HistorySaveError")]
        public String HistorySaveError { get; set; }
    }
}
