using System.Net;
using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Models;
using Starfish.Core.Services;

namespace Starfish.Web.BankAccounts;

[ApiController]
[Route("[controller]")]
public class BankAccountsController : ControllerBase
{
    private readonly IBankAccountsService _bankAccountsService;
    private readonly ILogger<BankAccountsController> _logger;

    public BankAccountsController(IBankAccountsService bankAccountsService, ILogger<BankAccountsController> logger)
    {
        _bankAccountsService = bankAccountsService;
        _logger = logger;
    }

    /// <summary>
    /// Get all account list
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet(Name = "GetBankAccounts")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetAll(CancellationToken ctx)
    {
        var accounts = await _bankAccountsService.GetAll(ctx);
        return Ok(accounts);
    }
    
    /// <summary>
    /// Get account by id
    /// <param name="id">Account id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet(Name = "GetBankAccount")]
    [Route("/{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> Get(Guid id, CancellationToken ctx)
    {
        var account = await _bankAccountsService.Get(id, ctx);
        return Ok(account);
    }
    
    /// <summary>
    /// Add new account
    /// <param name="account">Account details</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpPost(Name = "AddBankAccount")]
    public async Task<ActionResult> Add(BankAccount account, CancellationToken ctx)
    {
        await _bankAccountsService.Add(account, ctx);
        return StatusCode((int)HttpStatusCode.Created);
    }
    
    /// <summary>
    /// Add new accounts
    /// <param name="accounts">List of bank accounts</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpPost(Name = "AddBankAccounts")]
    public async Task<ActionResult> Add(List<BankAccount> accounts, CancellationToken ctx)
    {
        await _bankAccountsService.Add(accounts, ctx);
        return StatusCode((int)HttpStatusCode.Created);
    }
    
    /// <summary>
    /// Delete account by id
    /// <param name="id">Account id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpDelete(Name = "DeleteBankAccount")]
    [Route("/{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> Delete(Guid id, CancellationToken ctx)
    {
        await _bankAccountsService.Delete(id, ctx);
        return Ok();
    }
    
    /// <summary>
    /// Delete account by id
    /// <param name="ids">List of account ids</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpDelete(Name = "DeleteBankAccounts")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> Delete(List<Guid> ids, CancellationToken ctx)
    {
        await _bankAccountsService.Delete(ids, ctx);
        return Ok();
    }
}