using Starfish.Core.Models;

namespace Starfish.Core;

public interface IFraudCheckerService
{
    public FraudCheckResult Check(BankTransaction transaction);
}