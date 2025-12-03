using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GreedingController : ControllerBase
    {
        private static long _counter = 0;
        private static readonly string _template = "Hello, {0}!";
       

        [HttpGet]
        public Greeding Get([FromQuery] string name = "World")
        {
            var id = Interlocked.Increment(ref _counter);
            var content = string.Format(_template, name);

            return new Greeding(id, content);
        }
    }
}
