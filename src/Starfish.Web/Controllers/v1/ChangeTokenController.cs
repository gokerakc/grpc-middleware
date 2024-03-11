using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Resources;

namespace Starfish.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Tags("Change Token")]
[Route("change-token")]
public class ChangeTokenController : ControllerBase
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


