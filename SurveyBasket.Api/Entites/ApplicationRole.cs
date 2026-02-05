namespace SurveyBasket.Api.Entites;

public class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDelete { get; set; }
}
