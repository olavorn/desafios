using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Filters
{
    public class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        private IHostingEnvironment _env;
        private ILogger _logger;

        public ErrorHandlerAttribute(IHostingEnvironment env, ILogger logger)
        {
            _env = env;
            _logger = logger;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!_env.IsDevelopment())
            {
                await Task.Run( () => _logger.LogError(context.Exception, "Error in the handling of the message") );
            }

            context.Result = new StatusCodeResult(500);
        }
    }
}
