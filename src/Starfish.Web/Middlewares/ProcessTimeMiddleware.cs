using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;

namespace Starfish.Web.Middlewares;

public class ProcessTimeMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = new Stopwatch();
        sw.Start();
        
        await next.Invoke(context);
        sw.Stop();
        
        //TODO: Collect this info in a no-sql database
        Debug.Write($$"""
            {
                "method": "{{context.Request.Method}}"
                "url": "{{context.Request.GetEncodedUrl()}}"
                "processTime": {{sw.ElapsedMilliseconds}}
                "clientId": "{{GetClientId(context.Request.Headers)}}"
                "utcDate": {{DateTime.UtcNow}}
            }
            """);
    }

    private static string GetClientId(IHeaderDictionary headers)
    {
        var clientId = headers["ClientId"];
        return (clientId.Count == 0 ? "unknown" : clientId)!;
    }
}