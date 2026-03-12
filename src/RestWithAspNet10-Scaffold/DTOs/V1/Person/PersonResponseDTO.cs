using RestWithAspNet10_Scaffold.Hypermedia;
using RestWithAspNet10_Scaffold.Hypermedia.Abstract;

namespace RestWithAspNet10_Scaffold.DTOs.V1.Person
{
    public class PersonResponseDTO : ISupportsHypermedia
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        public string? Address { get; set; }
        
        public string? Gender { get; set; }
        public bool Enabled { get; set; }
        public List<HypermediaLink> Links { get ; set ; } = [];
    }
}
