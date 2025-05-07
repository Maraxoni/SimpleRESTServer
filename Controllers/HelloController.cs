using Microsoft.AspNetCore.Mvc;

namespace SimpleRESTServer.Controllers
{
    [ApiController]
    [Route("api/hello")]
    public class HelloController : ControllerBase
    {
        // Ćwiczenie 1
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Hello from REST API", "text/plain");
        }

        // Ćwiczenie 2
        [HttpGet("echo")]
        public IActionResult Echo()
        {
            return Content("Witaj Echo", "text/plain");
        }

        // Ćwiczenie 3
        [HttpGet("echo2/{param}")]
        public IActionResult Echo2(string param)
        {
            return Ok($"Witaj {param}");
        }
    }
}
