namespace Starfish.Web.Services;

public class RateLimiterService : IRateLimiterService
{
    private readonly RateLimiter.RateLimiterClient _client;

    public RateLimiterService(RateLimiter.RateLimiterClient client)
    {
        _client = client;
    }
    
    public async Task<RateLimiterResult> Acquire(string clientId)
    {
        var rateLimiterResult = await _client.AcquireAsync(new RateLimiterRequest{ ClientId = clientId });

        if (rateLimiterResult == null)
        {
            throw new NullReferenceException("Rate limiter result can not be null");
        }

        return rateLimiterResult;
    }
}