using System;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace YoutubeGameBarWidget.Search
{
    /// <summary>
    /// Implements a Data Retriever using Youtube GameBar Search Server's service.
    /// 
    /// For more API details, see: https://github.com/MarconiGRF/YoutubeGameBarSearchServer
    /// </summary>
    class Search
    {
        private WebClient client;
        private string ytgbssEndPoint;
        public ListItems parsedResults;
        public event EventHandler FinishedFetchingResults;
        public event EventHandler FailedFetchingResults;

        /// <summary>
        /// The FinishedFetchingResults event method manager.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFinishedFetchingResults(EventArgs e)
        {
            EventHandler handler = FinishedFetchingResults;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// The FailedFetchingResults event method manager.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFailedFetchingResults(EventArgs e)
        {
            EventHandler handler = FailedFetchingResults;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// A simple constructor setting the common parameters for every search request.
        /// </summary>
        public Search()
        {
            this.ytgbssEndPoint = "http://" 
                + Environment.GetEnvironmentVariable("YTGBSS_ADDRESS") + ":" 
                + Environment.GetEnvironmentVariable("YTGBSS_PORT") + "/current/search/";

            this.client = new WebClient();
            this.client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            this.client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ParseResults);
        }

        /// <summary>
        /// Performs a search (GET) request on YTGBSS by the given term, raising events when the raw data is ready.
        /// </summary>
        /// <param name="givenTerm">The term to compose the request.</param>
        /// <returns></returns>
        public async Task ByTerm(string givenTerm)
        {
            this.client.DownloadStringAsync(new Uri(ytgbssEndPoint + givenTerm));
        }

        /// <summary>
        /// Parses the raw data into a ListItems object, raising FinishedFetchingResults event when finished.
        /// In case of failure, it FailedFetchingResults event will be raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseResults(Object sender, DownloadStringCompletedEventArgs e)
        {
            this.parsedResults = new ListItems();

            try
            {
                JsonArray jArray = JsonArray.Parse((string)e.Result);
                foreach (JsonValue jValue in jArray)
                {
                    JsonObject jObject = jValue.GetObject();
                    this.parsedResults.Add(new ListItem(
                            jObject.GetNamedString("mediaType"),
                            jObject.GetNamedString("mediaTitle"),
                            jObject.GetNamedString("channelTitle"),
                            jObject.GetNamedString("mediaUrl")));
                }

                this.OnFinishedFetchingResults(EventArgs.Empty);
            }
            catch
            {
                this.OnFailedFetchingResults(EventArgs.Empty);
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
