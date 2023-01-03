using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Starfish.Shared;

namespace Starfish.Web.Middlewares;

public class PerformanceMonitorMiddleware : IMiddleware
{
    private const int DesiredMaxResponseTime = 5000;
    
    private readonly ILogger<PerformanceMonitorMiddleware> _logger;
    private readonly IOptionsMonitor<StarfishOptions> _starfishLoggingOptions;

    public PerformanceMonitorMiddleware(ILogger<PerformanceMonitorMiddleware> logger, IOptionsMonitor<StarfishOptions> options)
    {
        _logger = logger;
        _starfishLoggingOptions = options;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_starfishLoggingOptions.CurrentValue.PerformanceMonitorEnabled == false)
        {
            await next.Invoke(context);
            return;
        }
        
        var startingTime = Stopwatch.GetTimestamp();
        await next.Invoke(context);
        var endingTime = Stopwatch.GetTimestamp();

        var elapsed = Stopwatch.GetElapsedTime(startingTime, endingTime).TotalMilliseconds;
        if ( elapsed > DesiredMaxResponseTime)
        {
            _logger.LogWarning($"Request took more than 5 seconds to process. Details : {Details(context.Request, (int)elapsed)}");
        }
    }
    
    private static string Details(HttpRequest request, int elapsedMs)
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
        return (clientId.Count == 0 ? "unknown" : clientId!);
    }
}