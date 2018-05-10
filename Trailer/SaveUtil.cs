using MSCLoader;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Trailer
{
    public class SaveUtil
    {
        public static void SerializeWriteFile<T>(T value, string path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                StreamWriter output = new StreamWriter(path);
                XmlWriter xmlWriter = XmlWriter.Create(output);
                xmlSerializer.Serialize(xmlWriter, value);
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                ModConsole.Error(ex.ToString());
            }
        }

        public static T DeserializeReadFile<T>(string path) where T : new()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                StreamReader input = new StreamReader(path);
                XmlReader xmlReader = XmlReader.Create(input);
                T result = (T)((object)xmlSerializer.Deserialize(xmlReader));
                return result;
            }
            catch (Exception ex)
            {
                ModConsole.Error(ex.ToString());
            }
            if (default(T) != null)
            {
                return default(T);
            }
            return Activator.CreateInstance<T>();
        }
    }
}
