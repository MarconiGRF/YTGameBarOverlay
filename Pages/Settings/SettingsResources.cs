using System;
using System.Xml.Serialization;

namespace YoutubeGameBarWidget.Pages 
{
    /// <summary>
    /// The resources for the Main Page.
    /// Its values are intended to be set with the content on the user language's XML and with user preferences.
    /// </summary>
    [XmlRoot("SettingsResources")]
    public class SettingsResources
    {
        [XmlElement("Title")]
        public String Title { get; set; }

        [XmlElement("AccentColor")]
        public String AccentColor { get; set; }

        [XmlElement("SecondaryColor")]
        public String SecondaryColor { get; set; }

        [XmlElement("TextIconColor")]
        public String TextIconColor { get; set; }

        [XmlElement("Save")]
        public String Save { get; set; }

        [XmlElement("Black")]
        public String Black { get; set; }

        [XmlElement("White")]
        public String White { get; set; }

        [XmlElement("Blue")]
        public String Blue { get; set; }

        [XmlElement("Red")]
        public String Red { get; set; }

        [XmlElement("Yellow")]
        public String Yellow { get; set; }

        [XmlElement("Green")]
        public String Green { get; set; }

        [XmlElement("PreferredLanguage")]
        public String PreferredLanguage { get; set; }

        [XmlElement("EnglishUS")]
        public String EnglishUS { get; set; }

        [XmlElement("PortugueseBR")]
        public String PortugueseBR { get; set; }

        [XmlElement("RestartMessage")]
        public String RestartMessage { get; set; }
    }
}
