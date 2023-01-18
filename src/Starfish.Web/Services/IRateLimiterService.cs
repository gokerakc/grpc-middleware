namespace Starfish.Web.Services;

public interface IRateLimiterService
{
   public Task<RateLimiterResult> Acquire(string clientId);
}