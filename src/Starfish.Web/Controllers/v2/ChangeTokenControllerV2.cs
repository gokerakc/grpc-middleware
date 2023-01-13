using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Starfish.Core.Resources;

namespace Starfish.Web.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Tags("Change Token")]
[Route("change-token", Name = "ChangeTokenName")]
[EnableRateLimiting("strict")]
public class ChangeTokenControllerV2 : ControllerBase
{
    /// <summary>
    /// Just an experimental endpoint to try the Change tokens feature.
    /// <param name="fullName">Full name of the new guest</param>
    /// </summary>
    [HttpPost]
    public ActionResult Add([FromBody]string fullName)
    {
        GuestListSource.Add(fullName);
        return StatusCode((int)HttpStatusCode.Created);
    }
}


