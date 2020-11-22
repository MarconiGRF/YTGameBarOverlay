using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YoutubeGameBarWidget.Pages
{
    /// <summary>
    /// The resources for the WebPage.
    /// Its values are intended to be set with the content on the user language's XML.
    /// </summary>
    [XmlRoot("WebPageResources")]
    public class WebPageResources
    {
        [XmlElement("PinTip")]
        public String PinTip { get; set; }

        [XmlElement("BackTip")]
        public String BackTip { get; set; }

        [XmlElement("InvalidURLWarn")]
        public String InvalidURLWarn { get; set; }

        [XmlElement("LoadingWarn")]
        public String LoadingWarn { get; set; }
    }
}
