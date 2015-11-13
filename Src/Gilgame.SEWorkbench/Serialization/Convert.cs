using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Serialization
{
    public static class Convert
    {
        public static string ToSerialized(object o)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(
                    new[] { XmlQualifiedName.Empty }
                );

                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true
                };

                StringBuilder result = new StringBuilder();
                using (var writer = XmlWriter.Create(result, settings))
                {
                    serializer.Serialize(writer, o, namespaces);
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                // log error

                return String.Empty;
            }
        }

        public static object ToObject(string serialized)
        {
            try
            {
                Type type = GetType(serialized);
                XmlSerializer serializer = new XmlSerializer(type);
                using (var reader = new StringReader(serialized))
                {
                    return serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                //log error

                return new Object();
            }
        }

        private static Type GetType(string serialized)
        {
            XDocument document = XDocument.Parse(serialized);

            string root = document.Root.Name.ToString();
            switch (root)
            {
                case "ProjectFile": return typeof(ProjectFile);

                default: return typeof(Object);
            }
        }
    }
}
