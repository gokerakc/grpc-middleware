using System.Net;
using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Models;
using Starfish.Core.Services;
using Starfish.Web.Controllers.v1;

namespace Starfish.Web.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Tags("Bank Accounts")]
[Route("bank-accounts")]
public class BankAccountsControllerV2 : ControllerBase
{
    private readonly IBankAccountsService _bankAccountsService;
    private readonly ILogger<BankAccountsController> _logger;

    public BankAccountsControllerV2(IBankAccountsService bankAccountsService, ILogger<BankAccountsController> logger)
    {
        _bankAccountsService = bankAccountsService;
        _logger = logger;
    }

    /// <summary>
    /// Get all account list
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetAll(CancellationToken ctx)
    {
        var accounts = await _bankAccountsService.GetAllAsync(ctx);
        return Ok(accounts);
    }
    
    /// <summary>
    /// Get account by id
    /// <param name="id">Account id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> Get(Guid id, CancellationToken ctx)
    {
        var account = await _bankAccountsService.GetAsync(id, ctx);
        return Ok(account);
    }
    
    /// <summary>
    /// Add new account
    /// <param name="account">Account details</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Add(BankAccount account, CancellationToken ctx)
    {
        await _bankAccountsService.AddAsync(account, ctx);
        return StatusCode((int)HttpStatusCode.Created);
    }
    
    /// <summary>
    /// Delete account by id
    /// <param name="id">Account id</param>
    /// <param name="ctx">Cancellation token</param>
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> Delete(Guid id, CancellationToken ctx)
    {
        await _bankAccountsService.DeleteAsync(id, ctx);
        return Ok();
    }
}