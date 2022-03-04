using System.Collections.Generic;

namespace YTGameBarWidget.Utilities
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
            public const string StorableDateFormat = "yyyyMMdd";
        }

        /// <summary>
        /// Enumarator for the media types.
        /// </summary>
        public enum MediaTypes
        {
            video,
            playlist
        }

        /// <summary>
        /// Constant endpoints.
        /// </summary>
        public static class Endpoints
        {
            public const string FSBase = "http://{0}:{1}/feedback/";
            public const string Github = "https://api.github.com/repos/MarconiGRF/YTGameBarOverlay/releases";
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

        /// <summary>
        /// Common Constants to Local Settings key names.
        /// </summary>
        public static class Settings
        {
            public static readonly Dictionary<string, string> ShowTips =
                new Dictionary<string, string>
                {
                    { "varname", "showTips" },
                    { "True", "true" },
                    { "False", "false" }
                };

            public static readonly Dictionary<string, string> ShowThumbnails =
                new Dictionary<string, string>
                {
                    { "varname", "showThumbnails" },
                    { "True", "true" },
                    { "False", "false" }
                };

            public static string[] LanguagesNames = { "English", "Portuguese", "Spanish" };
            public const string DefaultPreferredLanguage = "en-US";
            public static readonly Dictionary<string, string> Languages =
                new Dictionary<string, string>
                {
                    { "varname", "preferredLanguage" },
                    { "English", "en-US" },
                    { "Portuguese", "pt-BR" },
                    { "Spanish", "es-ES" }
                };

            public static string[] ColorNames = { "Red", "Yellow", "Green", "Blue", "Black" };
            public static string[] AuxiliaryNames = { "Black", "White" };

            public static readonly Dictionary<string, string> AccentColors =
                new Dictionary<string, string>
                {
                    { "varname", "accentColor" },
                    { "Red", "#C92306" },
                    { "Yellow", "#FFBA08" },
                    { "Green", "#81BC06" },
                    { "Blue", "#05A6F0" },
                    { "Black", "#000000" }
                };

            public static readonly Dictionary<string, string> SecondaryColors =
                new Dictionary<string, string>
                {
                    { "varname", "secondaryColor" },
                    { "Red", "#7D1B0A" },
                    { "Yellow", "#E6A607" },
                    { "Green", "#71A305" },
                    { "Blue", "#0494D6" },
                    { "Black", "#4D4D4D" }
                };

            public static readonly Dictionary<string, string> AuxiliaryColors =
                new Dictionary<string, string>
                {
                    { "varname", "auxiliarColor" },
                    { "Black", "#000000" },
                    { "White", "#FFFFFF" }
                };
        }

        public static string DonationURL = "https://www.paypal.com/donate?hosted_button_id=CDGQF2GN2J6KL&source=url";
    }
}
