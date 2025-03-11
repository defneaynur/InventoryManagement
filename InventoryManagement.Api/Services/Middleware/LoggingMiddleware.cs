using Serilog;

namespace InventoryManagement.Api.Services.Middleware
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
            var startTime = DateTime.UtcNow;
            var request = context.Request;
            var methodName = request.Path; 
            var requestBody = string.Empty;

            if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put || request.Method == HttpMethods.Patch || request.Method == HttpMethods.Get)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, encoding: System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            Log.Information("Gelen İstek - Metod: {Method}, Endpoint: {Path}, Gövde: {Body}, Başlangıç: {StartTime}",
                request.Method, methodName, requestBody, startTime);

            try
            {
                await _next(context);

                var responseStatusCode = context.Response.StatusCode;
                var duration = DateTime.UtcNow - startTime; 

                Log.Information("Yanıt - Endpoint: {Path}, Durum Kodu: {StatusCode}, Süre: {Duration}ms",
                    methodName, responseStatusCode, duration.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var duration = DateTime.UtcNow - startTime; 
                Log.Error(ex, "Hata - Endpoint: {Path}, Süre: {Duration}ms, Hata Mesajı: {Message}",
                    methodName, duration.TotalMilliseconds, ex.Message);

       
                throw;
            }
        }
    }



}
