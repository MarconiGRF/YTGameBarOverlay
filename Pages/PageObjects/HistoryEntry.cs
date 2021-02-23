using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// A history entry class aimed to store information for a video and displayed on history page.
    /// </summary>
    public class HistoryEntry
    {
        private String Title { get; set; }

        private String ThumbnailURL { get; set; }

        private String Type { get; set; }

        private String Timestamp { get; set; }

        public HistoryEntry(String title, String thumbnailURL, String type, String timestamp)
        {
            this.Title = title;
            this.ThumbnailURL = thumbnailURL;
            this.Timestamp = timestamp;
            this.Type = type;
        }

        public string ToStorable()
        {
            return "(NULL,'" + Title + "','" + ThumbnailURL + "','" + Type + "','" + Timestamp + "')";
        }

        public static HistoryEntry ofRaw(String rawEntry)
        {
            string[] entryData = rawEntry.Split(',');
            return new HistoryEntry(entryData[1], entryData[2], entryData[3], entryData[4]);
        }
    }
}
