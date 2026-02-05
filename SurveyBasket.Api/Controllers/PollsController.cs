




using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contract.Poll;

namespace SurveyBasket.Api.Controllers;

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
    [Authorize(Roles =DefaultRole.Member)]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        
        return Ok(await _pollsServices.GetCurrentAsync(cancellationToken));

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
    [HttpPut("{id}/TogglePublish")]
    [HasPermission(Permission.UpdatePolls)]

    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var resault = await _pollsServices.TogglePublishStatusAsync(id, cancellationToken);
            return resault.IsFaliure
          ? resault.ToProblem()
          : NoContent();
       
    }
}
