using RestWithAspNet10_Scaffold.Hypermedia.Abstract;

namespace RestWithAspNet10_Scaffold.Hypermedia.Filters
{
    public class HypermediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = [];
    }
}
