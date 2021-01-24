using System;
using System.Xml.Serialization;

namespace YoutubeGameBarWidget
{
    [XmlRoot("SupportMeResources")]
    public class SupportMeResources
    {
        [XmlElement("Title")]
        public String Title { get; set; }

        [XmlElement("Donate")]
        public String Donate { get; set; }

        [XmlElement("Message")]
        public String Message { get; set; }

        [XmlElement("ThankYou")]
        public String ThankYou { get; set; }
    }
}
