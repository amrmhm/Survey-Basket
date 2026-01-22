
using OneOf;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Contract.Authentication;
using SurveyBasket.Api.Errors;
using System.Security.Cryptography;

namespace SurveyBasket.Api.Services;

public class AuthServices(UserManager<ApplicationUser> userManager , IJwtProvider jwtProvider) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
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

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidCredintial);
        }

        //Check Password

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!IsValidPassword)
        {
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidCredintial); ;
        }


        //Generate JWT Token
        var (token, expireIn) = _jwtProvider.GenrateToken(user);

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



    public async Task<Resault<ResponseAuth>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if(userId == null) 
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(userId);
        if(user == null)
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidJwtToken); ;
       var userRefreshToken = user.RefreshToken.SingleOrDefault(c => c.Token == refreshToken && c.IsActive);
        if(userRefreshToken == null) 
            return Resault.Faliure<ResponseAuth>(UserErrors.InvalidRefreshToken); ;
        userRefreshToken.RevokedOn = DateTime.UtcNow;


        //Generate JWT Token
        var (newToken, expireIn) = _jwtProvider.GenrateToken(user);

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

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
