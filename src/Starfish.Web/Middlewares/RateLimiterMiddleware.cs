using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Starfish.Web.Services;

namespace Starfish.Web.Middlewares;

public class RateLimiterMiddleware : IMiddleware
{
    private readonly IRateLimiterService _rateLimiterService;

    public RateLimiterMiddleware(IRateLimiterService rateLimiterService)
    {
        _rateLimiterService = rateLimiterService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var rateLimiterResult = await _rateLimiterService.Acquire(GetClientId(context.Request.Headers));

        if (rateLimiterResult.IsAcquired)
        {
            await next.Invoke(context);    
        }
        else
        {
            context.Response.StatusCode = 429; // Too many requests;
            context.Response.Headers.Add("Content-Type", "application/json");
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
            {
                Title = "Too many requests",
                Status = 429,
                Detail = rateLimiterResult.Message
            }));
        }
    }
    
    private static string GetClientId(IHeaderDictionary headers)
    {
        var clientId = headers["ClientId"];
        return (clientId.Count == 0 ? "unknown" : clientId!);
    }
}