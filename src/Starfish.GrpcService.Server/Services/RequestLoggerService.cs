using System.Diagnostics;
using System.Text.Json;
using Grpc.Core;

namespace Starfish.GrpcService.Server.Services;

public class RequestLoggerService : RequestLogger.RequestLoggerBase
{
    private readonly ILogger<RequestLoggerService> _logger;

    public RequestLoggerService(ILogger<RequestLoggerService> logger)
    {
        _logger = logger;
    }

    public override Task<StatusResponse> Log(RequestDetailsRequest request, ServerCallContext context)
    {
        //TODO: Save details to a no-sql db
        Debug.WriteLine(JsonSerializer.Serialize(request));
        
        return Task.FromResult(new StatusResponse{Success = true});
    }
}