//using Serilog;
//using Serilog.Events;
//using Microsoft.AspNetCore.Http;

//namespace Financials.API.Middlewares
//{
//    public class LoggingMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public LoggingMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            Log.Information("Requisição HTTP iniciada: {Method} {Path}", context.Request.Method, context.Request.Path);

//            try
//            {
//                await _next(context);
//            }
//            finally
//            {
//                var statusCode = context.Response.StatusCode;
//                var logLevel = statusCode >= 400 ? LogEventLevel.Error : LogEventLevel.Information;

//                Log.Write(logLevel, "Requisição HTTP completada: {Method} {Path} com status {StatusCode}, IP: {IP},Body:{Body}",
//                    context.Request.Method,
//                    context.Request.Path,
//                    statusCode,
//                    context.Connection.RemoteIpAddress.ToString(),
//                    context.Response.Body);
//            }
//        }
//    }
//}


using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

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
            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/index.html"))
            {
                await _next(context);
                return;
            }
            Log.Information("Requisição HTTP iniciada: {Method} {Path}", context.Request.Method, context.Request.Path);

            try
            {
                //await _next(context);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var logLevel = statusCode >= 400 ? LogEventLevel.Error : LogEventLevel.Information;

                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                

                Log.Write(logLevel, "Requisição HTTP completada: {Method} {Path} com status {StatusCode}, IP: {IP},Body:{Body}",
                context.Request.Method,
                context.Request.Path,
                statusCode,
                context.Connection.RemoteIpAddress.ToString(),
                text);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

            }
        }
    }
}
