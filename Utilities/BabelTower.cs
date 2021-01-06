using System.IO;

namespace YoutubeGameBarWidget.Utilities
{
    /// <summary>
    /// A translator aimed to parse XMLs and return filled language objects.
    /// </summary>
    public static class BabelTower
    {
        /// <summary>
        /// Gets the translated resources for the given class based on the user preffered language.
        /// In case of the user language is not yet translated en-US will be set as a failsase source language.
        /// </summary>
        /// <typeparam name="T">The class type to get resources of.</typeparam>
        /// <returns>A resource object filled with the information parsed from XML.</returns>
        public static T getTranslatedResources<T>()
        {
            string currentLanguage = (string) Utils.GetSettingValue(Constants.Settings.Languages["varname"]);
            string requestedClass = typeof(T).Name;
            string filePath = "Resources\\" + requestedClass + "\\" + currentLanguage + ".xml";

            if (!File.Exists(filePath))
            {
                Utils.setSettingValue(Constants.Settings.Languages["varname"], Constants.Settings.DefaultPreferredLanguage);
                currentLanguage = Constants.Settings.DefaultPreferredLanguage;
                filePath = "Resources\\" + requestedClass + "\\" + currentLanguage + ".xml";
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                object result = serializer.Deserialize(reader);
                return (T) result;
            }
        }
    }
}
