using System;

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
    }
}