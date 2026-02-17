using RestWithAspNet10_Scaffold.Hypermedia;
using System.Xml.Serialization;

namespace RestWithAspNet10_Scaffold.DTOs.Common
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<T> Items { get; set; } = new();

        // Links HATEOAS
        [XmlIgnore]
        public List<HypermediaLink> Links { get; set; } = new();

    }
}
