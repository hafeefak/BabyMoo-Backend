using BabyMoo.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BabyMoo.Middleware
{
    public class CustomAuthorizationResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // If status code is 401 (unauthorized) and no response body has been written
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<string>(401, "Unauthorized. Please login.", null);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }

            // If status code is 403 (forbidden) and no response body has been written
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<string>(403, "Access denied. Admin only.", null);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
