using AuthenticationTry.Data;
using AuthenticationTry.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationTry.Middlewares
{
    public class Middleware1
    {
        private readonly RequestDelegate _next;
        

        public Middleware1(RequestDelegate next )
        {
            this._next = next;
            
        }
        public async Task Invoke(HttpContext context, AuthenticationTryContext dbContext)
        {
            if (context.Request.Path.StartsWithSegments("/home"))
            {
                if (context.Request.Cookies.TryGetValue("auth", out var token)&& !token.IsNullOrEmpty())
                {
                    User user = dbContext.User.FirstOrDefault(u => u.Token == token)!;
                    if(user != null)
                    { 
                    await _next.Invoke(context);
                    return;
                    }
                }
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("access denied");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
