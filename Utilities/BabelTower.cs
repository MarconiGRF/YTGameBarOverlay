using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeGameBarWidget.Utilities
{
    /// <summary>
    /// A translator aimed to parse XMLs and return filled language objects.
    /// </summary>
    public static class BabelTower
    {
        /// <summary>
        /// Gets the translated resources for the given class based on the user system language.
        /// In case of the user language is not yet translated en-US will be set as a failsase source language.
        /// </summary>
        /// <typeparam name="T">The class type to get resources of.</typeparam>
        /// <returns>A resource object filled with the information parsed from XML.</returns>
        public static T getTranslatedResources<T>()
        {
            string currentLanguage = CultureInfo.InstalledUICulture.Name;
            string requestedClass = typeof(T).Name;
            string filePath = "Resources\\" + requestedClass + "\\" + currentLanguage + ".xml";

            if (!File.Exists(filePath))
            {
                currentLanguage = "en-US";
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
