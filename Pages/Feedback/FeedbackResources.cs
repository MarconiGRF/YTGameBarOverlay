using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// The resources for the Feedback Page.
    /// Its values are intended to be set with the content on the user language's XML.
    /// </summary>
    [XmlRoot("FeedbackResources")]
    public class FeedbackResources
    {
        [XmlElement("Title")]
        public String Title { get; set; }

        [XmlElement("MessagePlaceholder")]
        public String MessagePlaceholder { get; set; }

        [XmlElement("AuthorPlaceholder")]
        public String AuthorPlaceholder { get; set; }

        [XmlElement("Send")]
        public String Send { get; set; }

        [XmlElement("Sent")]
        public String Sent { get; set; }

        [XmlElement("FailedError")]
        public String FailedError { get; set; }

        [XmlElement("TryAgainWarn")]
        public String TryAgainWarn { get; set; }

        [XmlElement("NoFeedbackMessageError")]
        public String NoFeedbackMessageError { get; set; }

        [XmlElement("NoFeedbackAuthorError")]
        public String NoFeedbackAuthorError { get; set; }
    }
}
