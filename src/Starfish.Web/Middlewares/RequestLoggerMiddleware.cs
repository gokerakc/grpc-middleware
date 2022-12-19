using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Starfish.Web.Options;

namespace Starfish.Web.Middlewares;

public class RequestLoggerMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggerMiddleware> _logger;
    private readonly IOptionsMonitor<StarfishLoggingOptions> _starfishLoggingOptions;

    public RequestLoggerMiddleware(ILogger<RequestLoggerMiddleware> logger, IOptionsMonitor<StarfishLoggingOptions> options)
    {
        _logger = logger;
        _starfishLoggingOptions = options;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_starfishLoggingOptions.CurrentValue.RequestLoggingEnabled == false)
        {
            await next.Invoke(context);
            return;
        }
        
        var startingTime = Stopwatch.GetTimestamp();
        await next.Invoke(context);
        var endingTime = Stopwatch.GetTimestamp();
        
       _logger.LogInformation(message: Message(context.Request, (int)Stopwatch.GetElapsedTime(startingTime, endingTime).TotalMilliseconds));
    }

    private static string Message(HttpRequest request, int elapsedMs)
    {
        return $$"""
        {
            "method": {{request.Method}}
            "url": {{request.GetEncodedUrl()}}
            "processTime": {{elapsedMs}}
            "clientId": {{GetClientId(request.Headers)}}
            "utcDate": {{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}}
        }
        """;
    }

    private static string GetClientId(IHeaderDictionary headers)
    {
        var clientId = headers["ClientId"];
        return (clientId.Count == 0 ? "unknown" : clientId)!;
    }
}