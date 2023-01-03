using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Starfish.Core;
using Starfish.Core.Models;
using Starfish.Shared;

namespace Starfish.Infrastructure.Services;

public class FraudCheckerService : IFraudCheckerService
{
    private readonly FraudChecker.FraudCheckerClient _client;

    public FraudCheckerService(FraudChecker.FraudCheckerClient client)
    {
        _client = client;
    }
    
    public FraudCheckResult Check(BankTransaction transaction)
    {
        var result = _client.Check(Map(transaction));
        return Map(result);
    }

    private static TransactionDetailsRequest Map(BankTransaction transaction) =>
        new TransactionDetailsRequest
        {
            Id = transaction.Id.ToString(),
            SourceId = transaction.SourceId.ToString(),
            TargetId = transaction.TargetId.ToString(),
            Amount = Convert.ToDouble(transaction.Amount),
            UtcDate = Timestamp.FromDateTime(transaction.Date)
        };

    private static FraudCheckResult Map(FraudReport report)
    {
        return report.TransactionStatus switch
        {
            FraudReport.Types.StatusTypes.Unknown => FraudCheckResult.Unknown,
            FraudReport.Types.StatusTypes.Valid => FraudCheckResult.Valid,
            FraudReport.Types.StatusTypes.Suspicious => FraudCheckResult.Suspicious,
            FraudReport.Types.StatusTypes.Fraud => FraudCheckResult.Fraud,
            _ => throw new ArgumentOutOfRangeException() // Throw a custom exception
        };
    }
}