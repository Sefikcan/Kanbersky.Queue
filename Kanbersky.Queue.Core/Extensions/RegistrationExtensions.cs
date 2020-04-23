using Kanbersky.Queue.Core.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Kanbersky.Queue.Core.Extensions
{
    public static class RegistrationExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
