using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace RestWithAspNet10_Scaffold.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogTestController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Log.Information("LogTestController accessed at " + System.DateTime.Now);
            //Log.Debug("This is a debug message from LogTestController.");
            //Log.Warning("This is a warning message from LogTestController.");
            //Log.Error("This is an error message from LogTestController.");
            //Log.Fatal("This is a fatal message from LogTestController.");


            //Log.Logger.Verbose("This is a verbose message from LogTestController.");
            //Log.Logger.Information("LogTestController finished processing at " + System.DateTime.Now);
            //Log.Logger.Debug("LogTestController finished processing at " + System.DateTime.Now);
            //Log.Logger.Warning("LogTestController finished processing at " + System.DateTime.Now);
            //Log.Logger.Error("LogTestController finished processing at " + System.DateTime.Now);
            //Log.Logger.Fatal("LogTestController finished processing at " + System.DateTime.Now);



            return Ok("LogTestController is working!");
        }
    }
}
