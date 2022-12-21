using System.Net;
using Microsoft.AspNetCore.Mvc;
using Starfish.Core.Resources;

namespace Starfish.Web.Controllers;

[ApiController]
[Route("change-tokens")]
public class ChangeTrackerController : ControllerBase
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


