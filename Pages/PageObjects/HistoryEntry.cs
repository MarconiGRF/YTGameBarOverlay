using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTGameBarWidget.Utilities;

namespace YTGameBarWidget.Pages.PageObjects
{
    /// <summary>
    /// A history entry class aimed to store information for a video and displayed on history page.
    /// </summary>
    public class HistoryEntry
    {
        public long Id { get; set; }
        public String Title { get; set; }
        public String Channel { get; set; }
        public String ThumbnailURL { get; set; }
        public String MediaURL { get; set; }
        public Constants.MediaTypes Type { get; set; }
        public String IconGlyph { get; set; }
        public String Timestamp { get; set; }
        public ThemeResources ColorResources { get; set; }

        public HistoryEntry(String title, String channel, String mediaURL, String thumbnailURL, Constants.MediaTypes type, String timestamp)
        {
            this.Title = title;
            this.Channel = channel;
            this.MediaURL = mediaURL;
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
            return "(NULL,'" + Title + "','" + Channel + "','" + MediaURL + "','" + ThumbnailURL + "','" + Type + "','" + Timestamp + "')";
        }

        public static HistoryEntry OfRaw(String rawEntry)
        {
            string[] entryData = rawEntry.Split(',');
            HistoryEntry storedEntry = new HistoryEntry(
                entryData[1],
                entryData[2],
                entryData[3],
                entryData[4],
                Enum.Parse<Constants.MediaTypes>(entryData[5]), entryData[6]);
            storedEntry.Id = long.Parse(entryData[0]);
            return storedEntry;
        }
    }
}
