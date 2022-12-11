using System.Net;
using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Models;
using Starfish.Core.Services;

namespace Starfish.Web.BankTransactions;

[ApiController]
[Route("[controller]")]
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
    [HttpGet(Name = "GetBankTransactions")]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> GetAll(CancellationToken ctx)
    {
        var transactions = await _bankTransactionsService.GetAll(ctx);
        return Ok(transactions);
    }
    
    /// <summary>
    /// Get bank transaction by id
    /// <param name="id">Bank transaction id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet(Name = "GetBankTransaction")]
    [Route("/{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> Get(Guid id, CancellationToken ctx)
    {
        var transaction = await _bankTransactionsService.Get(id, ctx);
        return Ok(transaction);
    }
    
    /// <summary>
    /// Add new bank transaction
    /// <param name="transaction">Bank transaction details</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpPost(Name = "AddBankTransaction")]
    public async Task<ActionResult> Add(BankTransaction transaction, CancellationToken ctx)
    {
        await _bankTransactionsService.Add(transaction, ctx);
        return StatusCode((int)HttpStatusCode.Created);
    }
    
    /// <summary>
    /// Add new bank transactions
    /// <param name="transactions">List of bank bank transactions</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpPost(Name = "AddBankTransactions")]
    public async Task<ActionResult> Add(List<BankTransaction> transactions, CancellationToken ctx)
    {
        await _bankTransactionsService.Add(transactions, ctx);
        return StatusCode((int)HttpStatusCode.Created);
    }
    
    /// <summary>
    /// Delete bank transaction by id
    /// <param name="id">Bank transaction id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpDelete(Name = "DeleteBankTransaction")]
    [Route("/{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> Delete(Guid id, CancellationToken ctx)
    {
        await _bankTransactionsService.Delete(id, ctx);
        return Ok();
    }
    
    /// <summary>
    /// Delete bank transaction by id
    /// <param name="ids">List of bank transaction ids</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpDelete(Name = "DeleteBankTransactions")]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> Delete(List<Guid> ids, CancellationToken ctx)
    {
        await _bankTransactionsService.Delete(ids, ctx);
        return Ok();
    }
}