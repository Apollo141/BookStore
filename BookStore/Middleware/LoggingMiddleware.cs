namespace BookStore.Middleware
{
    public class LoggingMiddleare
    {
        
    private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleare> _logger;

        public LoggingMiddleare(
            RequestDelegate next,
            ILogger<LoggingMiddleare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log incoming request
            _logger.LogInformation("Incoming Request: {method} {url}",
                context.Request.Method, context.Request.Path); 

            // Copy original response stream
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Log outgoing response
            _logger.LogInformation("Outgoing Response: {statusCode}",
                context.Response.StatusCode);  

            // Copy the contents back to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

}
