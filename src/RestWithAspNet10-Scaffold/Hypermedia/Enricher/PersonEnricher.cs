using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using RestWithAspNet10_Scaffold.Hypermedia.Constants;

namespace RestWithAspNet10_Scaffold.Hypermedia.Enricher
{
    public class PersonEnricher : ContentResponseEnricher<PersonResponseDTO>
    {
        protected override Task EnrichModel(
            PersonResponseDTO content, 
            IUrlHelper urlHelper)
        {
            var request = urlHelper.ActionContext.HttpContext.Request;

            var baseUrl = $"{request.Scheme}://" +
                $"{request.Host.ToUriComponent()}" +
                $"{request.PathBase.ToUriComponent()}/api/v1/person";

            content.Links.AddRange(GenerateLinks(content.Id, baseUrl));

            return Task.CompletedTask;
        }

        private IEnumerable<HypermediaLink> GenerateLinks(long id, string baseUrl)
        {
            //return new List<HypermediaLink>
            return
            [
                // This new HypermediaLink is equal to new()
                new()
                {
                    Rel = RelationType.COLLECTION,
                    Href = $"{baseUrl}",
                    Type = ResponseTypeFormat.DefaultGet,
                    Action = HttpActionVerb.GET
                },
                new()
                {
                    Rel = RelationType.SELF,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.DefaultGet,
                    Action = HttpActionVerb.GET
                },
                new()
                {
                    Rel = RelationType.CREATE,
                    Href = $"{baseUrl}",
                    Type = ResponseTypeFormat.DefaultPost,
                    Action = HttpActionVerb.POST
                },
                new()
                {
                    Rel = RelationType.UPDATE,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.DefaultPut,
                    Action = HttpActionVerb.PUT
                },
                new()
                {
                    Rel = RelationType.PATCH,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.DefaultPatch,
                    Action = HttpActionVerb.PATCH
                },
                new()
                {
                    Rel = RelationType.DELETE,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.DefaultDelete,
                    Action = HttpActionVerb.DELETE
                },
            ];
        }

    }
}
