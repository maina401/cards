using Newtonsoft.Json.Linq;

namespace Cards.Core.Util;

public class ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        var response = new JObject
        {
            ["error"] = exception.Message
        };

        // Only include the stack trace if the app is in development
        if (env.IsDevelopment())
        {
            response["stackTrace"] = exception.StackTrace;
        }

        return context.Response.WriteAsync(response.ToString());
    }
}