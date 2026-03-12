using System.Xml.Serialization;

namespace RestWithAspNet10_Scaffold.Hypermedia
{
    public class HypermediaLink
    {
        [XmlAttribute]
        public string Rel { get; set; } = string.Empty;

        [XmlAttribute]
        public string Href { get; set; } = string.Empty;

        [XmlAttribute]
        public string Type { get; set; } = "application/json";

        [XmlAttribute]
        public string Action { get; set; } = string.Empty;

        public HypermediaLink(string rel, string href, string type = "application/json", string action = "")
        {
            Rel = rel;
            Href = href;
            Type = type;
            Action = action;
        }

        public HypermediaLink() { }

    }
}
