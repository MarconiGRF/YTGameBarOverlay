using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeGameBarWidget.Utilities
{
    public static class BabelTower
    {
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
