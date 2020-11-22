using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YoutubeGameBarWidget.Pages
{
    /// <summary>
    /// The resources for the Main Page.
    /// Its values are intended to be set with the content on the user language's XML.
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
    }
}
