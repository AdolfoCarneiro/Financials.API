using Financials.Infrastructure.Helpers;
using Financials.Services.RequestsResponses.Base;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System.Text;

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
            // Se a requisição é para a URL do Swagger ou para a página inicial, simplesmente passa pelo middleware sem interferência.
            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/index.html"))
            {
                await _next(context);
                return;
            }

            try
            {
                // Não faz nada aqui, mas você poderia usar try para capturar exceções se estivesse realizando alguma ação.
            }
            finally
            {
                // Salva o stream original do corpo da resposta para que possamos restaurá-lo mais tarde.
                var originalBodyStream = context.Response.Body;

                // Cria um novo MemoryStream que vai temporariamente armazenar a resposta enquanto ela é manipulada.
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                // Chama o próximo middleware no pipeline, que agora escreverá no nosso stream de memória em vez de escrever diretamente na resposta.
                await _next(context);

                // Depois que o próximo middleware for chamado, obtemos o status da resposta HTTP.
                var statusCode = context.Response.StatusCode;

                // Define o nível de log baseado no status code, utilizando Error para códigos 400+ e Information para outros.
                var logLevel = statusCode >= 400 ? LogEventLevel.Error : LogEventLevel.Information;

                // Retrocede ao início do stream de memória para ler o conteúdo.
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Lê o texto da resposta HTTP do stream.
                var text = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();

                // Desserializa o texto da resposta para um objeto ApplicationResponse para que possamos acessar suas propriedades.
                var responseObj = JsonConvert.DeserializeObject<ApplicationResponse<string>>(text);

                // Extrai uma mensagem de erro personalizada do objeto de resposta, se a resposta for inválida.
                var errorMessage = responseObj.Valid ? null : responseObj.Error.CustomMessage;

                // Registra o log com todas as informações relevantes, incluindo método, caminho, status code, IP, corpo da resposta, usuário e mensagem de erro.
                Log.Write(logLevel, "Requisição HTTP completada: {Method} {Path} com status {StatusCode}, IP: {IP}, Body:{Body}, User: {User}, Error: {errorMessage}",
                    context.Request.Method,
                    context.Request.Path,
                    statusCode,
                    context.Connection.RemoteIpAddress.ToString(),
                    text,
                    context.User?.Identity?.Name ?? "Anonymous",
                    errorMessage);

                // Retorna ao início do stream de memória para copiar seu conteúdo de volta para o stream de resposta original.
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);

                // Restaura o stream original do corpo da resposta.
                context.Response.Body = originalBodyStream;
            }
        }

    }
}
