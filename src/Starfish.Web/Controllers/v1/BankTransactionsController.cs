using System.Net;
using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Models;
using Starfish.Core.Services;

namespace Starfish.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Tags("Bank Transactions")]
[Route("bank-transactions")]
public class BankTransactionsController : ControllerBase
{
    private readonly IBankTransactionsService _bankTransactionsService;
    private readonly ILogger<BankTransactionsController> _logger;

    public BankTransactionsController(IBankTransactionsService bankTransactionsService, ILogger<BankTransactionsController> logger)
    {
        _bankTransactionsService = bankTransactionsService;
        _logger = logger;
    }

    /// <summary>
    /// Get all bank transaction list
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> GetAll(CancellationToken ctx)
    {
        var transactions = await _bankTransactionsService.GetAllAsync(ctx);
        return Ok(transactions);
    }
    
    /// <summary>
    /// Get bank transaction by id
    /// <param name="id">Bank transaction id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> Get(Guid id, CancellationToken ctx)
    {
        var transaction = await _bankTransactionsService.GetAsync(id, ctx);
        return Ok(transaction);
    }
    
    /// <summary>
    /// Add new bank transaction
    /// <param name="transaction">Bank transaction details</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Add(BankTransaction transaction, CancellationToken ctx)
    {
        await _bankTransactionsService.AddAsync(transaction, ctx);
        return StatusCode((int)HttpStatusCode.Created);
    }
    
    /// <summary>
    /// Delete bank transaction by id
    /// <param name="id">Bank transaction id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> Delete(Guid id, CancellationToken ctx)
    {
        await _bankTransactionsService.DeleteAsync(id, ctx);
        return Ok();
    }
}