using Microsoft.AspNetCore.Mvc;

namespace Starfish.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class BankTransactionsController : ControllerBase
{
    private readonly ILogger<BankTransactionsController> _logger;

    public BankTransactionsController(ILogger<BankTransactionsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBankTransactions")]
    public ActionResult<IEnumerable<object>> Get()
    {
        return Ok(Array.Empty<object>());
    }
}