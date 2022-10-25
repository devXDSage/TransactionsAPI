using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TransactionAPIApplication.Filters
{
    public class ExampleFilter : ActionFilterAttribute
    {
        private readonly ILogger<ExampleFilter> _logger;

        public ExampleFilter(ILogger<ExampleFilter> logger)
        {
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Before");
           

            await next();
            _logger.LogInformation("After");
        }
    }
}
