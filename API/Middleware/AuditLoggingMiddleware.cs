using Serilog;

namespace User_Management.Middleware
{
    public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public AuditLoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next;
        _logger =logger;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();

        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        _logger.Information("Incoming Request: {Method} {Path} | Headers: {@Headers} | Body: {Body}",
            context.Request.Method,
            context.Request.Path,
            context.Request.Headers,
            requestBody);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context); // Proceed to next middleware

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.Information("Outgoing Response: {StatusCode} | Body: {Body}",
            context.Response.StatusCode,
            responseText);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}

}
