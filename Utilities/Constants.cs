namespace YoutubeGameBarWidget.Utilities
{
    /// <summary>
    /// A class with contant values common to the code.
    /// The constants are alphabetically ordered to facilitate search.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Common constants.
        /// </summary>
        public static class Common
        {
            public const string AuthorSignature = "Author: ";
            public const string EmptyString = "";
        }

        /// <summary>
        /// Constant endpoints.
        /// </summary>
        public static class Endpoints
        {
            public const string FSBase = "http://{0}:{1}/feedback/";
            public const string Github = "https://api.github.com/repos/MarconiGRF/YoutubeGameBarOverlay/releases";
            public const string SSBase = "http://{0}:{1}/current/search/";
        }

        /// <summary>
        /// Header constants for requests.
        /// </summary>
        public static class Headers
        {
            public const string CommonUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
            public const string GithubJson = "application/vnd.github.v3+json";
            public const string Json = "application/json";
        }

        /// <summary>
        /// URL parsing constants.
        /// </summary>
        public static class URL
        {
            public const char DashSeparator = '/';
            public const string ListIdSeparator = "list=";
            public const char QueryStringSeparator = '&';
            public const string VideoIdSeparator = "v=";

        }

        /// <summary>
        /// Environment variables names constants.
        /// </summary>
        public static class Vars
        {
            public const string FSAddress = "YTGBFS_ADDRESS";
            public const string FSPort = "YTGBFS_PORT";
            public const string SSAddress = "YTGBSS_ADDRESS";
            public const string SSPort = "YTGBSS_PORT";
            public const string WSAddress = "YTGBWS_ADDRESS";
            public const string WSPort = "YTGBWS_PORT";
        }

        /// <summary>
        /// VideoUI-related constants.
        /// </summary>
        public static class VideoUI
        {
            public const string BaseAddress = "http://localhost:";
            public const string PlaylistQueryString = "/?listId=";
            public const string VideoQueryString = "/?videoId=";
        }

        /// <summary>
        /// Common Constants to WebServer Class.
        /// </summary>
        public static class WebServer
        {
            public const string ContentCSS = "Content-Type: text/css; charset=\"utf-8\"";
            public const string ContentHTML = "Content-Type: text/html";
            public const string ContentImage = "Content-Type: image/png";
            public const string ContentJavascript = "Content-Type: text/javascript";
            public const string GETMethod = "GET";
            public const string IndexHTML = "index.html";
            public const string NotFoundHeader = "HTTP/1.1 404 Not Found\r\n" +
                                                 "Content-Length:0\r\n" +
                                                 "Connection: close\r\n\r\n";
            public const string OkHeader = "HTTP/1.1 200 OK\r\n" +
                                               "Content-Length: {0}\r\n" +
                                               "{1}\r\n" +
                                               "Connection: close\r\n\r\n";
            public const string POSTMethod = "POST";
            public const string RequestNotSupportedException = "Request Method not supported: ";
            public const string VideoUIFilePath = "VideoUI\\";
        }
    }
}
