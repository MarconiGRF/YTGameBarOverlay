using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YTGameBarWidget.Utilities
{
    /// <summary>
    /// A class for all validations and regular expressions common to the application.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Regular Expression pattern strings common to the app.
        /// </summary>
        public static class RegexPatterns
        {
            public const string YTBaseExpected = @"https?:\/\/(www\.)?youtu(\.be)?(be)?\.[a-zA-Z]{1,6}\b([-a-zA-Z.\/]*)";
            public const string HTTPBaseExpected = @"https?:\/\/";
            public const string PathFormat = @"/[?](.+)";
        }

        /// <summary>
        /// Checks if the MediaURL is a valid video, live or playlist URL.
        /// </summary>
        public static bool IsMediaURLValid(string mediaURL)
        {
            List<String> invalidWords = new List<string>();
            invalidWords.Add("/user/");
            invalidWords.Add("/channel/");
            invalidWords.Add("redirect");
            invalidWords.Add("share");
            invalidWords.Add("login");
            invalidWords.Add("event=");

            foreach (string word in invalidWords)
            {
                if (mediaURL.Contains(word))
                {
                    return false;
                }
            }

            if (mediaURL.Length <= 32)
            {
                if (Regex.IsMatch(mediaURL, RegexPatterns.YTBaseExpected))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string ytBaseURLInput = mediaURL.Substring(0, 24);

                if (Regex.IsMatch(ytBaseURLInput, RegexPatterns.YTBaseExpected))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
