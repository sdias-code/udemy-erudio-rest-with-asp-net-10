using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithAspNet10_Scaffold.Hypermedia.Filters
{
    public class HypermediaFilter : ResultFilterAttribute
    {
        private readonly HypermediaFilterOptions _hypermediaFilterOptions;

        public HypermediaFilter(HypermediaFilterOptions hypermediaFilterOptions)
        {
            _hypermediaFilterOptions = hypermediaFilterOptions;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Ensure the async enrichment runs to completion before proceeding.
            TryEnrichResult(context).GetAwaiter().GetResult();
            base.OnResultExecuting(context);
        }

        private async Task TryEnrichResult(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult  
                && objectResult.StatusCode is >= 200 and < 300 
                && objectResult.Value != null)
            {
                var enricher = _hypermediaFilterOptions
                    ?.ContentResponseEnricherList
                    ?.FirstOrDefault(option => option.CanEnrich(context));

                if (enricher != null)
                {
                    await enricher.Enrich(context);
                }
            }
        }
    }
}
