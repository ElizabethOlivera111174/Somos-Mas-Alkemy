using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Middleware
{
    public class AdminRoutesMiddleware
    {
        private readonly RequestDelegate _next;
        public AdminRoutesMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            List<string> methods = new List<string>();
            methods.Add("put");
            methods.Add("post");
            methods.Add("patch");
            methods.Add("delete");
            var method = context.Request.Method;
            List<string> paths = new List<string>();
            paths.Add("/activities");
            paths.Add("/category");
            paths.Add("/comments");
            paths.Add("/news");

            string path = context.Request.Path;
            if (methods.Contains(method.ToLower()) && paths.Contains(path.ToLower()))
            {
                if (!context.User.IsInRole("Administrator"))
                {
                    context.Response.StatusCode = 401;
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}