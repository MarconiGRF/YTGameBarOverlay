using System;
using System.Xml.Serialization;

namespace YTGameBarWidget
{
    /// <summary>
    /// The resources for the SupportMe Page.
    /// Its values are intended to be set with the content on the user language's XML and with user preferences.
    /// </summary>
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
