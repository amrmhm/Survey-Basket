




using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
///using Swashbuckle.AspNetCore.Annotations;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("api/[controller]")]
[ApiController]
[Authorize]
//[DisableCors] //Or[EnableCors] Can Use On Controle / Action
public class PollsController(IPollsServices pollsServices) : ControllerBase
{
    private readonly IPollsServices _pollsServices = pollsServices;



    [HttpGet("")]
    [HasPermission(Permission.GetPolls)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {

        return Ok(await _pollsServices.GetAllAsync(cancellationToken));

    }
    [MapToApiVersion(1)]
    [Authorize(Roles = DefaultRole.Member)]
    [HttpGet("current")]
    [EnableRateLimiting(RateLimit.UserLimit)]
    // [SwaggerIgnore]
    public async Task<IActionResult> GetCurrentV1(CancellationToken cancellationToken)
    {

        return Ok(await _pollsServices.GetCurrentAsyncV1(cancellationToken));

    }
    [MapToApiVersion(2)]
    [Authorize(Roles = DefaultRole.Member)]
    [HttpGet("current")]
    [EnableRateLimiting(RateLimit.UserLimit)]
    public async Task<IActionResult> GetCurrentV2(CancellationToken cancellationToken)
    {

        return Ok(await _pollsServices.GetCurrentAsyncV2(cancellationToken));

    }

    [HttpGet("{id}")]
    [HasPermission(Permission.GetPolls)]


    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var resault = await _pollsServices.GetAsync(id, cancellationToken);

        return resault.IsSuccess
            ? Ok(resault.Value)
            : resault.ToProblem(); ;

    }

    [HttpPost("")]
    [HasPermission(Permission.AddPolls)]


    public async Task<IActionResult> Create([FromBody] RequestPoll request, CancellationToken cancellationToken)
    {

        var resault = await _pollsServices.CreateAsync(request, cancellationToken);

        return resault.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = resault.Value.Id }, resault.Value)
            : resault.ToProblem();

    }
    [HttpPut("{id}")]
    [HasPermission(Permission.UpdatePolls)]


    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RequestPoll request, CancellationToken cancellationToken)
    {
        var resault = await _pollsServices.UpdateAsync(id, request, cancellationToken);
        return resault.IsSuccess
             ? NoContent()
             : resault.ToProblem();

    }
    [HttpDelete("{id}")]
    [HasPermission(Permission.DeletePolls)]

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var resault = await _pollsServices.DeleteAsync(id, cancellationToken);
        return resault.IsFaliure
             ? resault.ToProblem()
             : NoContent();
    }
    [HttpPut("{id}/toggle-publish")]
    [HasPermission(Permission.UpdatePolls)]

    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var resault = await _pollsServices.TogglePublishStatusAsync(id, cancellationToken);
        return resault.IsFaliure
      ? resault.ToProblem()
      : NoContent();

    }
}
