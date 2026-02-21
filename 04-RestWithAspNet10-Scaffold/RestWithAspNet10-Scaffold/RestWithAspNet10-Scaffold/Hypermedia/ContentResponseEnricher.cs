using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.Hypermedia.Abstract;

namespace RestWithAspNet10_Scaffold.Hypermedia
{
    public abstract class ContentResponseEnricher<T>
        : IResponseEnricher where T : ISupportsHypermedia
    {   

        public virtual bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T)
                || contentType == typeof(List<T>)
                || contentType == typeof(PagedResponse<T>);
        }

        protected abstract Task EnrichModel(
            T content, IUrlHelper urlHelper);

        bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
        {
            if (response.Result is ObjectResult objectResult 
                && objectResult.StatusCode is null or >= 200 and < 300)
            {
                var value = objectResult.Value;

                if (value == null) return false;
                
                return CanEnrich(value.GetType());
            }

            return false;
        }
        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory()
                .GetUrlHelper(response);

            if (response.Result is ObjectResult objectResult
                && objectResult.Value != null)
            {
                switch (objectResult.Value)
                {
                    case T model:
                        await EnrichModel(model, urlHelper);
                        break;

                    case IEnumerable<T> collection:
                        foreach (var item in collection)
                        {
                            await EnrichModel(item, urlHelper);
                        }
                        break;

                    case PagedResponse<T> paged:
                        foreach (var element in paged.Items)
                        {
                            await EnrichModel(element, urlHelper);
                        }
                        break;
                }
            }
        }

    }
}

