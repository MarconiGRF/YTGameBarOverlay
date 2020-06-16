using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Threading.Tasks;

namespace YoutubeGameBarWidget.Search
{
    /// <summary>
    /// Implements a Data Retriever using Google's Youtube API.
    /// 
    /// For more details, see: https://developers.google.com/youtube/v3/docs/search/list
    /// </summary>
    class Search
    {
        private YouTubeService _youtubeService;
        private SearchResource.ListRequest _listRequest;
        private SearchListResponse _listResponse;
        public ListItems parsedResults;

        /// <summary>
        /// A simple constructor setting the common parameters for every request.
        /// </summary>
        public Search()
        {
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Environment.GetEnvironmentVariable("YT_DATA_API_KEY"),
                ApplicationName = "YoutubeGameBarWidget"
            });

            this._listRequest = this._youtubeService.Search.List("snippet");
            _listRequest.Type = "video";
            _listRequest.MaxResults = 5;
            _listRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.None;
        }

        /// <summary>
        /// Performs a search request on Data API by the given term, parsing the response into a ListItems object.
        /// </summary>
        /// <param name="term">The term to compose the request.</param>
        /// <returns></returns>
        public async Task ByTerm(string term)
        {
            this._listRequest.Q = term;
            this._listResponse = await _listRequest.ExecuteAsync();

            this.parsedResults = new ListItems();
            foreach (SearchResult resultItem in this._listResponse.Items)
            {
                switch (resultItem.Id.Kind)
                {
                    case "youtube#video":
                        this.parsedResults.Add(new ListItem(
                                resultItem.Snippet.Title, 
                                resultItem.Snippet.ChannelTitle, 
                                resultItem.Id.VideoId)
                            );
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the previously parsed result list from the last search by term.
        /// </summary>
        /// <returns></returns>
        public ListItems Retreive()
        {
            return this.parsedResults;
        }
    }
}
