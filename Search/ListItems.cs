using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace YoutubeGameBarWidget.Search
{
    /// <summary>
    /// A custom class for the items to be shown on AutoSuggestBox's suggestion list.
    /// </summary>
    public class ListItems : ObservableCollection<ListItem>
    {
        public ListItems() { }
    }

    public class ListItem
    {
        public string MediaType { get; set; }
        public string MediaTitle { get; set; }
        public string ChannelTitle { get; set; }
        public string MediaUrl { get; set; }
        public string Thumbnail { get; set; }
        public Visibility ThumbnailVisibility { get; set; }
        public ThemeResources ColorResources { get; set; }

        /// <summary>
        /// A list item object.
        /// </summary>
        /// <param name="mediaType">The Media type (video or playlist), meant to be converted to a FontIcon Glyph string.</param>
        /// <param name="videoTitle">The Media title of the item.</param>
        /// <param name="channelTitle">The Channel title of the item.</param>
        /// <param name="mediaUrl">The Media URL of the item.</param>
        /// <param name="thumbnail">The thumbnail URL of the item.</param>
        /// <param name="visibility">The thumbnail visibility for the item.</param>
        public ListItem(string mediaType, string mediaTitle, string channelTitle, string mediaUrl, string thumbnail, Visibility visibility)
        {
            _ = mediaType == "playlist" ? this.MediaType = "\xE8FD" : this.MediaType = "\xF5B0";

            this.MediaTitle = mediaTitle;
            this.ChannelTitle = channelTitle;
            this.MediaUrl = mediaUrl;
            this.Thumbnail = thumbnail;
            this.ThumbnailVisibility = visibility;
        }
    }
}
