using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;

namespace Starfish.Web.Middlewares;

public class ProcessTimeMiddleware : IMiddleware
{
    private readonly RequestLogger.RequestLoggerClient _requestLoggerClient;

    public ProcessTimeMiddleware(RequestLogger.RequestLoggerClient requestLoggerClient)
    {
        _requestLoggerClient = requestLoggerClient;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next.Invoke(context);
        sw.Stop();

        var requestDetails = Map(context.Request, Convert.ToInt32(sw.ElapsedMilliseconds));
        Task.Run(() => _requestLoggerClient.LogAsync(requestDetails));
    }

    private static RequestDetailsRequest Map(HttpRequest request, int elapsedMs)
    {
        return new RequestDetailsRequest
        {
            Method = request.Method,
            Url = request.GetEncodedUrl(),
            ProcessTime = elapsedMs,
            ClientId = GetClientId(request.Headers),
            UtcDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
        };
    }

    private static string GetClientId(IHeaderDictionary headers)
    {
        var clientId = headers["ClientId"];
        return (clientId.Count == 0 ? "unknown" : clientId)!;
    }
}