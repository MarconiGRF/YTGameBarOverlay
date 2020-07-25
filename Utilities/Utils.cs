using System;
using System.Linq;

namespace YoutubeGameBarWidget.Utilities
{
    /// <summary>
    /// A class with useful methods common to the code.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Gets and returns an Environment Variable by the given variable name.
        /// </summary>
        /// <param name="varName">The variable name to be retrieved.</param>
        /// <returns>A string containing the environment variable content.</returns>
        public static string GetVar(string varName)
        {
            return Environment.GetEnvironmentVariable(varName);
        }

        /// <summary>
        /// Gets the proper URI following Youtube Game Bar's VideoUI format.
        /// Supported formats are described at https://github.com/MarconiGRF/YoutubeGameBarVideoUI
        /// </summary>
        /// <returns>A compatible VideoUI URI.</returns>
        public static Uri GetProperVideoUIUri(string _mediaUrl)
        {
            Uri properVideoUIURI;
            string baseAddress = Constants.VideoUI.BaseAddress;
            string ytgbwsPort = Utils.GetVar(Constants.Vars.WSPort);
            string videoQS = Constants.VideoUI.VideoQueryString;
            string playlistQS = Constants.VideoUI.PlaylistQueryString;

            string mediaId = GetMediaId(_mediaUrl);
            if (mediaId.Length > 11)
            {
                properVideoUIURI = new Uri(baseAddress + ytgbwsPort + playlistQS + mediaId);
            }
            else
            {
                properVideoUIURI = new Uri(baseAddress + ytgbwsPort + videoQS + mediaId);
            }

            return properVideoUIURI;
        }

        /// <summary>
        /// Parses the Video/Live ID from the given Media URL.
        /// </summary>
        /// <param name="url">The URL to parse from.</param>
        /// <returns>The parsed media ID.</returns>
        public static string GetMediaId(string url)
        {
            char qsSeparator = Constants.URL.QueryStringSeparator;
            char dashSeparator = Constants.URL.DashSeparator;
            string videoIdSeparator = Constants.URL.VideoIdSeparator;
            string listIdSeparator = Constants.URL.ListIdSeparator;

            try
            {
                return url.Split(listIdSeparator)[1].Split(qsSeparator).First();
            }
            catch (IndexOutOfRangeException)
            {
                try
                {
                    return url.Split(videoIdSeparator)[1].Substring(0, 11);
                }
                catch (IndexOutOfRangeException)
                {
                    return url.Split(dashSeparator).Last();
                }
            }
        }
    }
}