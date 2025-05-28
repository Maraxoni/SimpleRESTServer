using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleRESTServer
{

    public class CustomHeaderFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("Header", "Test");
        }
    }
}
