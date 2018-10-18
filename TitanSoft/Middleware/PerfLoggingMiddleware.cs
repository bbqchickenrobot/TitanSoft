using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TitanSoft.Api.Middleware
{
    public class PerfLoggingMiddleware
    {
        ILogger log;
        private readonly RequestDelegate _next;

        public PerfLoggingMiddleware(ILogger logger) => log = logger;

        public PerfLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            log = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = new Stopwatch();
            sw.Start();
            // Call the next delegate/middleware in the pipeline
            await _next(context);
            sw.Stop();
            log.LogInformation($"{context.Request.Protocol} request for {context.Request.Path} took {sw.ElapsedMilliseconds} ms");
        }
    }

    public static class PerfLoggingMiddlewareRegistration{
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, ILogger log) =>
            app.UseMiddleware<PerfLoggingMiddleware>();
    }
}
