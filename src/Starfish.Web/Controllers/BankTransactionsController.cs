using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Models;

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
    public ActionResult<IEnumerable<BankTransaction>> Get()
    {
        return Ok(Enumerable.Empty<BankTransaction>());
    }
}