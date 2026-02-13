using System.Text;
using System.Xml.Serialization;

namespace RestWithAspNet10.IntegrationTests.Tools
{
    public static class XmlHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var xmlSerializer = new XmlSerializer(typeof(T));

           var ns = new XmlSerializerNamespaces();
              ns.Add(string.Empty, string.Empty);

            using var stringWriter = new Utf8StringWriter();

            xmlSerializer.Serialize(stringWriter, obj, ns);

            return new StringContent(
                stringWriter.ToString(), 
                Encoding.UTF8, 
                "application/xml")
                .ReadAsStringAsync()
                .Result;

        }

        public static async Task<T?> ReadFromXmlAsync<T>(
           HttpResponseMessage response)
        {
            var serializer = new XmlSerializer(typeof(T));

            await using var stream = await response
                .Content.ReadAsStreamAsync();

            stream.Position = 0;

            return (T?)serializer.Deserialize(stream);
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
    
}
