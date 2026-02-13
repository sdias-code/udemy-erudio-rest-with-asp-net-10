using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RestWithAspNet10_Scaffold.Hypermedia.Abstract;

namespace RestWithAspNet10_Scaffold.Hypermedia
{
    public abstract class ContentResponseEnricher<T>
        : IResponseEnricher where T : ISupportsHypermedia
    {
        /*
        PSEUDOCODE / PLAN (detailed):
        - Problem: CS8602 arises because okObjectResult.Value may be null and we call GetType() on it.
        - Fix: Before calling GetType(), ensure Value is not null.
        - Implementation steps:
          1. In IResponseEnricher.CanEnrich(ResultExecutingContext), check if response.Result is OkObjectResult.
          2. If it is, capture okObjectResult.Value into a local variable.
          3. If the local value is null, return false (cannot enrich a null payload).
          4. Otherwise call CanEnrich(value.GetType()) and return its result.
        - Keep other logic unchanged: Enrich method uses pattern matching which is null-safe.
        - This change avoids dereferencing a possibly-null reference and preserves previous behavior.
        - No other code is modified.
        */

        public virtual bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T)
                || contentType == typeof(List<T>);
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

            if (response.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                if (objectResult.Value is T model)
                {
                    await EnrichModel(model, urlHelper);
                }
                else if (objectResult.Value is IEnumerable<T> collection)
                {
                    foreach (var element in collection)
                    {
                        await EnrichModel(element, urlHelper);
                    }
                }
            }
        }

    }
}

