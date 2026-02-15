using Asp.Versioning;
using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserServices userServices) : ControllerBase
{
    private readonly IUserServices _userServices = userServices;

    [HttpGet("")]
    [HasPermission(Permission.GetUsers)]

    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var resault = await _userServices.GetAllAsync(cancellationToken);
        return Ok(resault);
    }
    [HttpGet("{id}")]
    [HasPermission(Permission.GetUsers)]

    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var resault = await _userServices.GetAsync(id);
        return resault.IsSuccess
            ? Ok(resault.Value)
            : resault.ToProblem();
    }
    [HttpPost("")]
    [HasPermission(Permission.AddUsers)]

    public async Task<IActionResult> Create(RequestCreateUser request, CancellationToken cancellationToken)
    {
        var resault = await _userServices.CreateAsync(request, cancellationToken);
        return resault.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = resault.Value.Id }, resault.Value)
            : resault.ToProblem();
    }
    [HttpPut("{id}")]
    [HasPermission(Permission.UpdateUsers)]
    public async Task<IActionResult> Update([FromRoute] string id, RequestUpdateUser request, CancellationToken cancellationToken)
    {
        var resault = await _userServices.UpdateAsync(id, request, cancellationToken);
        return resault.IsSuccess
            ? NoContent()
            : resault.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permission.UpdateUsers)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var resault = await _userServices.ToggleStatusAsync(id);
        return resault.IsSuccess
            ? NoContent()
            : resault.ToProblem();
    }
    [HttpPut("{id}/unlock")]
    [HasPermission(Permission.UpdateUsers)]
    public async Task<IActionResult> UnLockUser([FromRoute] string id)
    {
        var resault = await _userServices.UnLockUserAsync(id);
        return resault.IsSuccess
            ? NoContent()
            : resault.ToProblem();
    }
}
