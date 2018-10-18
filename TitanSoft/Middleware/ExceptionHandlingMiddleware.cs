using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TitanSoft.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        readonly ILogger log;
        readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            log = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            try{
                await _next(context);
            }catch(Exception ex){
                log.LogError($"An error occurred while requesting {context.Request.Path}", ex);

            }
        }
    }

    public static class ExceptionHandlingMiddlewareRegistration
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
