using Serilog;
using Serilog.Events;

namespace Financials.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        static Random random = new Random();

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
                

                Log.Write(logLevel, "Requisição HTTP completada: {Method} {Path} com status {StatusCode}, IP: {IP},Body:{Body},User: {User}",
                context.Request.Method,
                context.Request.Path,
                statusCode,
                context.Connection.RemoteIpAddress.ToString(),
                text,
                context.User?.Identity?.Name ?? GetRandomName());

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

            }
        }
        public static string GetRandomName()
        {
            // Gera um número aleatório entre 0 e 99
            int randomNumber = random.Next(100);

            // Condições baseadas nas probabilidades
            if (randomNumber < 70) // 0-69 corresponde a 70%
            {
                return "Anonymous";
            }
            else if (randomNumber < 80) // 70-79 corresponde a 10%
            {
                return "Adolfo";
            }
            else if (randomNumber < 90) // 80-89 corresponde a 10%
            {
                return "Davi";
            }
            else // 90-99 corresponde a 10%
            {
                return "Lucas";
            }
        }
    }
}
