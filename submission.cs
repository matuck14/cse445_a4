using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://matuck14.github.io/cse445_a4/Hotels.xml";
        public static string xmlErrorURL = "https://matuck14.github.io/cse445_a4/HotelsErrors.xml";
        public static string xsdURL = "https://matuck14.github.io/cse445_a4/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdUrl);
                settings.ValidationType = ValidationType.Schema;

                string message = "No errors are found";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    message = e.Message;
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }

                return message;
            }
            catch (XmlException ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);
            // validation: must be deserializable
            var _ = JsonConvert.DeserializeXmlNode(jsonText);

            return jsonText;
        }
    }
}
