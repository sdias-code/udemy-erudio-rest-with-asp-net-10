using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithAspNet10_Scaffold.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        bool CanEnrich(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}
