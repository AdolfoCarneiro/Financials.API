using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Http;

namespace Financials.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information("Requisição HTTP iniciada: {Method} {Path}", context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var logLevel = statusCode >= 400 ? LogEventLevel.Error : LogEventLevel.Information;

                Log.Write(logLevel, "Requisição HTTP completada: {Method} {Path} com status {StatusCode}, IP: {IP}",
                    context.Request.Method,
                    context.Request.Path,
                    statusCode,
                    context.Connection.RemoteIpAddress.ToString());
            }
        }
    }
}
