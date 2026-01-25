using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OneOf.Types;

namespace SurveyBasket.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthServices authServices,IOptions<JwtOption> jwtOption ,ILogger<AuthServices> logger) : ControllerBase
{
    private readonly IAuthServices _authServices = authServices;
    private readonly ILogger<AuthServices> _logger = logger;
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
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request , CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logging with : {email} and {password}", request.Email, request.Password);
        var authResualt = await _authServices.GetTokenAsync(request.Email , request.Password , cancellationToken);

        var problem = new ProblemDetails();

        return authResualt.IsSuccess
            ? Ok(authResualt.Value)
            : authResualt.ToProblem();



       }

        
       
    [HttpGet ("Refresh")]
    public async Task<IActionResult> GetRefresh([FromBody] RequestRefreshToken request, CancellationToken cancellationToken)
    {
        var authResault = await _authServices.GetRefreshTokenAsync(request.token, request.refreshToken);

        return authResault.IsSuccess
            ? Ok(authResault.Value)
            : authResault.ToProblem();
    }
    [HttpPut ("Revoke-Refresh-Token")]
    public async Task<IActionResult> RevokeRefresh([FromBody] RequestRefreshToken request, CancellationToken cancellationToken)
    {
        var authResault = await _authServices.RevokeRefreshTokenAsync(request.token, request.refreshToken);

        
            return authResault.IsSuccess
            ? Ok()
            : authResault.ToProblem(); ;
       
    }
 
}
