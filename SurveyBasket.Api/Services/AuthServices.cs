
using Hangfire;
using Microsoft.AspNetCore.WebUtilities;
using OneOf;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Contract.Authentication;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Helpers;
using SurveyBasket.Api.Persistence;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.Api.Services;

public class AuthServices(UserManager<ApplicationUser> userManager ,
    IJwtProvider jwtProvider ,
    SignInManager<ApplicationUser> signInManager ,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthServices> logger,
    ApplicationDbContext context) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<AuthServices> _logger = logger;
    private readonly ApplicationDbContext _context = context;
    private readonly int _ExpirationRefreshToken = 14;

    //public async Task<OneOf<ResponseAuth, Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    //{
    //    //Check User

    //    var user = await _userManager.FindByEmailAsync(email);
    //    if (user == null)
    //    {
    //        return UserErrors.InvalidCredintial;
    //    }

    //    //Check Password

    //    var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

    //    if (!IsValidPassword)
    //    {
    //        return UserErrors.InvalidCredintial; ;
    //    }


    //    //Generate JWT Token
    //    var (token, expireIn) = _jwtProvider.GenrateToken(user);

    //    //Genrate Refresh Token 
    //    var refreshToken = GenerateRefreshToken();
    //    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_ExpirationRefreshToken);

    //    //Add Refresh Token TO Save in DataBase

    //    user.RefreshToken.Add(new RefreshToken
    //    {
    //        Token = refreshToken,
    //        ExpireOn = refreshTokenExpiration
    //    });
    //    await _userManager.UpdateAsync(user);


    //    var response = new ResponseAuth(

    //        Id: Guid.NewGuid().ToString(),
    //        Email: user.Email,
    //        FirstName: user.FirstName,
    //        LastName: user.LastName,
    //        ExpireIn: expireIn,
    //        Token: token,
    //        RefreshTokenExpiration: refreshTokenExpiration,
    //        RefreshToken: refreshToken


    //        );

    //    return response;


    //}

    public async Task<Resault<ResponseAuth>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        //Check User

        //var user = await _userManager.FindByEmailAsync(email);
        //if (user == null)
        //{
        //    return Resault.Faliure<ResponseAuth>(UserErrors.InvalidCredintial);
        //}

        // Or Use Check User 
        if(await _userManager.FindByEmailAsync(email) is not { } user)
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidCredintial);
        if(user.IsDisabled)
            return Resault.Faliure<ResponseAuth>(UserErrors.DisabledUser);



        //Check Password

        //var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        //if (!IsValidPassword)
        //{
        //    return Resault.Faliure<ResponseAuth>(UserErrors.InvalidCredintial); ;
        //}

        // Or Use PasswordSignInAsync In SignInManager to Check Confirm Email

        var resault = await _signInManager.PasswordSignInAsync(user, password, false, true);

        if(resault.Succeeded)
        {

            //Get Roles And Permissions

            var (userRoles , permissions) = await GetRolesAndPermissions( user , cancellationToken);

            //Generate JWT Token
            var (token, expireIn) = _jwtProvider.GenrateToken(user , userRoles ,permissions);

            //Genrate Refresh Token 
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_ExpirationRefreshToken);

            //Add Refresh Token TO Save in DataBase

            user.RefreshToken.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpireOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);


            var response = new ResponseAuth(

                Id: Guid.NewGuid().ToString(),
                Email: user.Email,
                FirstName: user.FirstName,
                LastName: user.LastName,
                ExpireIn: expireIn,
                Token: token,
                RefreshTokenExpiration: refreshTokenExpiration,
                RefreshToken: refreshToken


                );

            return Resault.Success(response);

        }
        // Check If First  If User Is Locked Out Or EmailNotConfirmed To Sign In Else This User Is InvalidCredintial  And  Return Error Message
        var error = resault.IsLockedOut
            ? UserErrors.LockedUser
            : resault.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : UserErrors.InvalidCredintial;
        return Resault.Faliure<ResponseAuth>(error);




 
    }

    

    public async Task<Resault<ResponseAuth>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if(userId == null) 
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(userId);
        if(user == null)
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidJwtToken);
        // Check If User Is Disabled And Check User Is Locked Out By SignInManager 
        if (user.IsDisabled)
            return Resault.Faliure<ResponseAuth>(UserErrors.DisabledUser);

        if(user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            return Resault.Faliure<ResponseAuth>(UserErrors.LockedUser);

        var userRefreshToken = user.RefreshToken.SingleOrDefault(c => c.Token == refreshToken && c.IsActive);
        if(userRefreshToken == null) 
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidRefreshToken); ;
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        //Get Roles And Permissions

        var (userRoles, permissions) = await GetRolesAndPermissions(user, cancellationToken);

        //Generate JWT Token
        var (newToken, expireIn) = _jwtProvider.GenrateToken(user , userRoles, permissions);

        //Genrate Refresh Token 
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_ExpirationRefreshToken);

        //Add Refresh Token TO Save in DataBase

        user.RefreshToken.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpireOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);


        var response = new ResponseAuth(

            Id: Guid.NewGuid().ToString(),
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            ExpireIn: expireIn,
            Token: newRefreshToken,
            RefreshTokenExpiration: refreshTokenExpiration,
            RefreshToken: newRefreshToken


            );
        return Resault.Success(response);
    }
    public async Task<Resault> ConfirmEmailAsync(RequestConfirmEmail request)
    {
        if(await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Resault.Faliure(UserErrors.InvalidCode);

        if(user.EmailConfirmed)
            return Resault.Faliure(UserErrors.DuplicatedConfirmation);

        var code = request.Code;
        try
        {
             code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {

            return Resault.Faliure(UserErrors.InvalidCode);
        }

        var resault = await _userManager.ConfirmEmailAsync(user, code);
        if(resault.Succeeded)
        {
            await _userManager.AddToRoleAsync(user , DefaultRole.Member);
            return Resault.Success();
        }

             var error = resault.Errors.First();
            return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Resault> ResendConfirmEmailAsync(RequestResendConfirmEmail request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Resault.Success();
        if(user.EmailConfirmed)
            return Resault.Faliure(UserErrors.DuplicatedConfirmation);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Confiremation Email {code} :", code);

        //TODO Send Confirm Email

        await SendConfirmationEmail(user, code);

        return Resault.Success();
    }

    public async Task<Resault> SendResetPasswordAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Resault.Success();

        if (!user.EmailConfirmed)
            return Resault.Faliure(UserErrors.EmailNotConfirmed);
        // Genarate Code from GeneratePasswordResetTokenAsync Methods
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Forget Password Email {code} :", code);
        //TODO Send Forget Password Email
        await SendResetPasswordEmail( user , code);
        return Resault.Success();
    }

    public async Task<Resault> ResetPasswordAsync (RequestResetPassword request)
    {
        var user = await _userManager.FindByEmailAsync (request.Email);
        if(user is null && !user!.EmailConfirmed)
            return Resault.Faliure(UserErrors.InvalidCode);
        IdentityResult resault;
        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            resault = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {

            // return Resault.Faliure(UserErrors.InvalidCode);
            //Or
          resault =  IdentityResult.Failed( _userManager.ErrorDescriber.InvalidToken());
        }

        if(resault.Succeeded)
            return Resault.Success();
        var error = resault.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }
    public async Task<Resault> RegisterAsync(RequestRegister request, CancellationToken cancellationToken = default)
    {
        var emailIsExist = await _userManager.Users.AnyAsync(c => c.Email == request.Email , cancellationToken);
        if(emailIsExist)
            return Resault.Faliure(UserErrors.DuplicateEmail);

        var user = request.Adapt<ApplicationUser>();

        var resualt = await _userManager.CreateAsync(user, request.Password);

        if(resualt.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confiremation Email {code} :", code);

            //TODO Send Confirm Email

            

            await SendConfirmationEmail(user ,code);


            return Resault.Success();

        }

            var error = resualt.Errors.First();
            return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        
    }

    public async Task<Resault> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
            return Resault.Faliure(UserErrors.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Resault.Faliure(UserErrors.InvalidJwtToken);
        var userRefreshToken = user.RefreshToken.SingleOrDefault(c => c.Token == refreshToken && c.IsActive);
        if (userRefreshToken == null)
            return Resault.Faliure(UserErrors.InvalidRefreshToken);
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return Resault.Success(); ;

    }


    private async Task <ResponseAuth> GenarateTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        //Get Roles And Permissions

        var (userRoles, permissions) = await GetRolesAndPermissions(user, cancellationToken);
        var (token, expireIn) = _jwtProvider.GenrateToken(user , userRoles , permissions);

        //Genrate Refresh Token 

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_ExpirationRefreshToken);

        //Add Refresh Token TO Save in DataBase

        user.RefreshToken.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpireOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);


        var response = new ResponseAuth(

            Id: Guid.NewGuid().ToString(),
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            ExpireIn: expireIn,
            Token: token,
            RefreshTokenExpiration: refreshTokenExpiration,
            RefreshToken: refreshToken


            );
        return response;
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
    private async Task SendConfirmationEmail(ApplicationUser user , string code)
    {

        //Origin => https://localhost.....
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
        var bodyBuilder = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                    { "{{name}}" ,user.FirstName } ,
                    { "{{action_url}}" ,$"{origin}/auth/email-confirm?userId={user.Id}&code={code}" }
            }
            );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "SurveyBasket - Email Confiremation", bodyBuilder));
        
        await Task.CompletedTask;
    }
    private async Task SendResetPasswordEmail(ApplicationUser user , string code)
    {

        //Origin => https://localhost.....
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
        var bodyBuilder = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            new Dictionary<string, string>
            {
                    { "{{name}}" ,user.FirstName } ,
                    { "{{action_url}}" ,$"{origin}/auth/forget-password?userId={user.Email}&code={code}" }
            }
            );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "SurveyBasket - Forget Password", bodyBuilder));
        
        await Task.CompletedTask;
    }

    private async Task<(IEnumerable<string>,IEnumerable<string>)> GetRolesAndPermissions(ApplicationUser user , CancellationToken cancellationToken = default)
    {
        //Get Roles
        var userRoles = await _userManager.GetRolesAsync(user);

        // Get Claims 
        //   var permissions = await _context.Roles
        //.Join(_context.RoleClaims,
        //role => role.Id,
        //claim => claim.RoleId,
        //(roles, claims) => new { roles, claims })
        //.Where(c => userRoles.Contains(c.roles.Name!))
        //.Select(c => c.claims.ClaimValue!)
        //.Distinct()
        //.ToListAsync(cancellationToken);

        //Or
        var permissions = await (from r in _context.Roles
                                 join p in _context.RoleClaims
                                 on r.Id equals p.RoleId
                                 where userRoles.Contains(r.Name!)
                                 select p.ClaimValue)
                                 .Distinct()
                                 .ToListAsync(cancellationToken);

     return (userRoles , permissions);
    }


}
