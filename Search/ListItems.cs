using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string VideoId { get; set; }

        /// <summary>
        /// A list item object.
        /// </summary>
        /// <param name="videoTitle">The video title of the item.</param>
        /// <param name="channelTitle">The channel title of the item.</param>
        /// <param name="videoId">The video ID of the item.</param>
        public ListItem(string videoTitle, string channelTitle, string videoId)
        {
            this.VideoTitle = videoTitle;
            this.ChannelTitle = channelTitle;
            this.VideoId = videoId;
        }
    }
}
