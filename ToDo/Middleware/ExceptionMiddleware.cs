using Newtonsoft.Json.Linq;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace ToDoDemo.Middleware
{
    public class ExceptionMiddleware
    {
        //STEP 1 => Store the next middleware in pipeline
        private readonly RequestDelegate _next;

        //STEP 2 => Constructor receives next middleware
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // STEP 3 => Invoke method runs for every request
        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                // STEP 4: Continue request to next middleware
                await _next(context);
            }
            catch (Exception ex) 
            {
                // STEP 5 => Handle exception globally
                await HandleExceptionAsync(context, ex);
            }
        }

        // STEP 6 => Centralized exception handling method
        private static Task HandleExceptionAsync(HttpContext context ,Exception ex) 
        {
            // STEP 1: Set status code
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // STEP 2: Redirect to custom error page
            context.Response.Redirect("/Home/Error");

            return Task.CompletedTask;

            
        }
    }
}
