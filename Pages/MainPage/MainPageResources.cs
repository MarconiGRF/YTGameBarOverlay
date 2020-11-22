using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YoutubeGameBarWidget.Pages
{
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
