using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("me")]
[ApiController]
[Authorize]
public class AccountsController(IUserServices userServices) : ControllerBase
{
    private readonly IUserServices _userServices = userServices;

    [HttpGet("")]

    public async Task<IActionResult> Info(CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var resault = await _userServices.GetProfileAsync(userId!, cancellationToken);
        return Ok(resault.Value);
    }
    [HttpPut("info")]

    public async Task<IActionResult> Info(RequestUpdateProfile request,CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var resault = await _userServices.UpdateProfileAsync(userId!,request, cancellationToken);
        return NoContent();
    }
    [HttpPut("change-password")]

    public async Task<IActionResult> ChangePassword(RequestChangePassword request,CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var resault = await _userServices.ChangePasswordAsync(userId!,request, cancellationToken);
        return resault.IsSuccess 
            ? NoContent() 
            : resault.ToProblem();
    }
}
