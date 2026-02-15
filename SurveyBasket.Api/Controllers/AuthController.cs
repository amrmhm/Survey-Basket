using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("[controller]")]
[ApiController]
[EnableRateLimiting(RateLimit.UserLimit)]
public class AuthController(IAuthServices authServices, IOptions<JwtOption> jwtOption) : ControllerBase
{
    private readonly IAuthServices _authServices = authServices;
    private readonly JwtOption _jwtOption = jwtOption.Value;

    //[HttpPost("")]
    //public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    //{
    //    var authResualt = await _authServices.GetTokenAsync(request.Email, request.Password, cancellationToken);


    //    return authResualt.Match(
    //        authResponse => Ok(authResponse),
    //        error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description));



    //}




    [HttpPost("")]

    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {

        var authResualt = await _authServices.GetTokenAsync(request.Email, request.Password, cancellationToken);

        var problem = new ProblemDetails();

        return authResualt.IsSuccess
            ? Ok(authResualt.Value)
            : authResualt.ToProblem();



    }



    [HttpGet("refresh")]
    public async Task<IActionResult> GetRefresh([FromBody] RequestRefreshToken request, CancellationToken cancellationToken)
    {
        var authResault = await _authServices.GetRefreshTokenAsync(request.token, request.refreshToken);

        return authResault.IsSuccess
            ? Ok(authResault.Value)
            : authResault.ToProblem();
    }
    [HttpPut("revoke-refresh-roken")]
    public async Task<IActionResult> RevokeRefresh([FromBody] RequestRefreshToken request, CancellationToken cancellationToken)
    {
        var authResault = await _authServices.RevokeRefreshTokenAsync(request.token, request.refreshToken);


        return authResault.IsSuccess
        ? Ok()
        : authResault.ToProblem(); ;

    }

    [HttpPost("register")]
    [DisableRateLimiting]
    public async Task<IActionResult> GetRefresh([FromBody] RequestRegister request, CancellationToken cancellationToken)
    {
        var authResault = await _authServices.RegisterAsync(request, cancellationToken);

        return authResault.IsSuccess
            ? Ok()
            : authResault.ToProblem();
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] RequestConfirmEmail request)
    {
        var authResault = await _authServices.ConfirmEmailAsync(request);

        return authResault.IsSuccess
            ? Ok()
            : authResault.ToProblem();
    }
    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmEmail([FromBody] RequestResendConfirmEmail request)
    {
        var authResault = await _authServices.ResendConfirmEmailAsync(request);

        return authResault.IsSuccess
            ? Ok()
            : authResault.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] RequestForgetPassword request)
    {
        var authResault = await _authServices.SendResetPasswordAsync(request.Email);

        return authResault.IsSuccess
            ? Ok()
            : authResault.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] RequestResetPassword request)
    {
        var authResault = await _authServices.ResetPasswordAsync(request);

        return authResault.IsSuccess
            ? Ok()
            : authResault.ToProblem();
    }



}
