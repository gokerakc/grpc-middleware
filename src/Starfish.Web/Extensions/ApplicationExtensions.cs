using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;

namespace Starfish.Web.Extensions;

public static class ApplicationExtensions
{
    [Obsolete("Rate limiting logic moved to the GRPC service.")]
    public static WebApplication UseStarfishRateLimiting(this WebApplication app)
    {
        app.UseRateLimiter(new RateLimiterOptions()
            .AddConcurrencyLimiter(policyName: "strict", (limiterOptions) =>
            {
                limiterOptions.PermitLimit = 2;
                limiterOptions.QueueLimit = 2;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            })
            .AddPolicy<string>(policyName: "standard", partitioner: httpContext =>
            {
                if (!StringValues.IsNullOrEmpty(httpContext.Request.Headers["token"]))
                {
                    return RateLimitPartition.GetTokenBucketLimiter("token", _ =>
                        new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 100,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 5,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(5),
                            TokensPerPeriod = 50,
                            AutoReplenishment = true
                        }
                    );
                }

                return RateLimitPartition.Get("guest", _ => new FixedWindowRateLimiter(
                    new FixedWindowRateLimiterOptions
                    {
                        QueueLimit = 2,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        PermitLimit = 2,
                        AutoReplenishment = true,
                        Window = TimeSpan.FromSeconds(5)
                    }));
            }));
        
        return app;
    }
}