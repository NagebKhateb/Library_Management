using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace LibraryManagement.Filters
{
    public class LibraryActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LibraryActionFilter> _logger;

        public LibraryActionFilter(ILogger<LibraryActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // معلومات عن الطلب
            var actionName = context.ActionDescriptor.DisplayName;
            var httpMethod = context.HttpContext.Request.Method;
            var timestamp = DateTime.Now;

            _logger.LogInformation($"Starting {httpMethod} request to {actionName} at {timestamp}");

            var stopwatch = Stopwatch.StartNew();

            await next(); // تنفيذ الإجراء

            stopwatch.Stop();

            _logger.LogInformation($"Completed {actionName} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
