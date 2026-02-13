using System;
using System.Collections.Generic;
using System.Text;

namespace RestWithAspNet10.IntegrationTests.Model
{
    public class HateoasLink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
        public string Action { get; set; }
    }
}
