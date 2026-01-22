
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider(IOptions<JwtOption> jwtOption) : IJwtProvider
{
    private readonly JwtOption _jwtOption = jwtOption.Value;

    public (string token, int expireIn) GenrateToken(ApplicationUser user)
    {
        var claims = new Claim[] {
            new Claim( JwtRegisteredClaimNames.Sub , user.Id),
             new Claim( JwtRegisteredClaimNames.Email , user.Email!),
             new Claim( JwtRegisteredClaimNames.GivenName , user.FirstName),
             new Claim( JwtRegisteredClaimNames.FamilyName , user.LastName),
             new Claim( JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey , SecurityAlgorithms.HmacSha256);
        

        var token = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOption.ExpireMinutes) ,
            signingCredentials: signingCredentials

            );

        return (token : new JwtSecurityTokenHandler().WriteToken(token) ,expireIn : _jwtOption.ExpireMinutes * 60);
    }

    public string? ValidateToken(string token)
    {
       var tokenHandler = new JwtSecurityTokenHandler();
       var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
        try
        {
            tokenHandler.ValidateToken(token,  new TokenValidationParameters
            {
                ValidateIssuer = false ,
                ValidateAudience = false ,
                ValidateIssuerSigningKey = true ,
                IssuerSigningKey = symmetricSecurityKey ,
                ClockSkew = TimeSpan.Zero
            },out SecurityToken validatedToken);
           var jwtToken = (JwtSecurityToken) validatedToken;
           return jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        }
        catch 
        {

            return null;
        }
    }
}
