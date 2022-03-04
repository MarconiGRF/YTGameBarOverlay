using System;
using System.Xml.Serialization;

namespace YTGameBarWidget.Pages
{
    /// <summary>
    /// The resources for the Changelog Page.
    /// Its values are intended to be set with the content on the user language's XML.
    /// </summary>
    [XmlRoot("ChangelogResources")]
    public class ChangelogResources
    {
        [XmlElement("Title")]
        public String Title { get; set; }

        [XmlElement("VersionDisplayer")]
        public String VersionDisplayer { get; set; }

        [XmlElement("AmazingError")]
        public String AmazingError { get; set; }

        [XmlElement("GithubCommunicationError")]
        public String GithubCommunicationError { get; set; }
    }
}
