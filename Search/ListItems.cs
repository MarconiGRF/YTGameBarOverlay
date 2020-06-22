using System.Collections.ObjectModel;

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
        public string VideoTitle { get; set; }
        public string ChannelTitle { get; set; }
        public string MediaUrl { get; set; }

        /// <summary>
        /// A list item object.
        /// </summary>
        /// <param name="videoTitle">The video title of the item.</param>
        /// <param name="channelTitle">The channel title of the item.</param>
        /// <param name="mediaUrl">The Media URL of the item.</param>
        public ListItem(string videoTitle, string channelTitle, string mediaUrl)
        {
            this.VideoTitle = videoTitle;
            this.ChannelTitle = channelTitle;
            this.MediaUrl = mediaUrl;
        }
    }
}
