using Microsoft.AspNetCore.Mvc;
using SVT.HttpsClients;
using SVT.Models;
using SVT.Core;

namespace SVT.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RobotsController : ControllerBase
{
    private readonly ILogger<RobotsController> _logger;
    private readonly RobotsClient _RobotsClient;
    public RobotsController(ILogger<RobotsController> logger, RobotsClient RobotsClient)
    {
        _logger = logger;
        _RobotsClient = RobotsClient;
    }

    [HttpPost("closest")]
    public async Task<ActionResult<ClosestResponseModel>> Closest([FromBody] ClosestInputModel payload)
    {
        var robots = await _RobotsClient.GetRobotLocations();

        var closestResponse = RobotLocator.FindClosestRobot(robots, payload.X.Value, payload.Y.Value);

        if (closestResponse == null) {
            return NotFound("Closest robot not found");
        }

        return Ok(closestResponse);
    }
}
