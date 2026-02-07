using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Contract.Role;

namespace SurveyBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleServices roleServices) : ControllerBase
{
    private readonly IRoleServices _roleServices = roleServices;

    [HttpGet("")]
    [HasPermission(Permission.GetRoles)]


    public async Task<IActionResult> GetAll([FromQuery] bool includeDelete, CancellationToken cancellationToken)
    {
        var resault = await _roleServices.GetAllAsync(includeDelete, cancellationToken);
        return Ok(resault);
    }
    [HttpGet("{id}")]
    [HasPermission(Permission.GetRoles)]


    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var resault = await _roleServices.GetAsync(id);
        return 
            resault.IsSuccess 
            ? Ok(resault.Value)
            :resault.ToProblem();
    }
    [HttpPost("")]
    [HasPermission(Permission.AddPRoles)]

    public async Task<IActionResult> Create([FromBody] RequestRole request)
    {
        var resault = await _roleServices.CreateAsync(request);
        return
            resault.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = resault.Value.Id }, resault.Value) 
            : resault.ToProblem();
    }
[HasPermission(Permission.UpdateRoles)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id ,[FromBody] RequestRole request)
    {
        var resault = await _roleServices.UpdateAsync(id ,request);
        return 
            resault.IsSuccess 
            ?NoContent()
            :resault.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id )
    {
        var resault = await _roleServices.ToggleStatusAsync(id);
        return 
            resault.IsSuccess 
            ?NoContent()
            :resault.ToProblem();
    }
}
