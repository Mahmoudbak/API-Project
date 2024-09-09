namespace ProjectApi.MiddelWares
{
    public class APIKeyValidationMiddleWare
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER_NAME = "X-Api-Key";

        public APIKeyValidationMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            if (!IsValidApiKey(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }

        private bool IsValidApiKey(string extractedApiKey)
        {
            return extractedApiKey == "Bakr";
        }
    }

}

