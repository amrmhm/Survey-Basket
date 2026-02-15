using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.Api.Contract.Authentication;

public class JwtOption
{

    public static string SectionName = "Jwt";



    [Required]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Inalid ExpireMinutes")]
    public int ExpireMinutes { get; init; }
}
