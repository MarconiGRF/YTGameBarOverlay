using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// A history entry class aimed to store information for a video and displayed on history page.
    /// </summary>
    public class HistoryEntry
    {
        public String Title { get; set; }
        public String Channel { get; set; }
        public String ThumbnailURL { get; set; }
        public Constants.MediaTypes Type { get; set; }
        public String IconGlyph { get; set; }
        public String Timestamp { get; set; }
        public ThemeResources ColorResources { get; set; }

        public HistoryEntry(String title, String channel, String thumbnailURL, Constants.MediaTypes type, String timestamp)
        {
            this.Title = title;
            this.Channel = channel;
            this.ThumbnailURL = thumbnailURL;
            this.Timestamp = timestamp;
            this.Type = type;

            if (this.Type == Constants.MediaTypes.video)
            {
                this.IconGlyph = "\xF5B0";
            }
            else
            {
                this.IconGlyph = "\xE8FD";
            }
        }

        public string ToStorable()
        {
            return "(NULL,'" + Title + "','" + Channel + "','" + ThumbnailURL + "','" + Type + "','" + Timestamp + "')";
        }

        public static HistoryEntry OfRaw(String rawEntry)
        {
            string[] entryData = rawEntry.Split(',');
            return new HistoryEntry(entryData[1], entryData[2], entryData[3], Enum.Parse<Constants.MediaTypes>(entryData[4]), entryData[5]);
        }
    }
}
