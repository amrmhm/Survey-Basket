namespace SurveyBasket.Api.Entites;

public class AuditableEntity
{

    public string CreateById { get; set; } = string.Empty;
    public DateTime CreateOn { get; set; } = DateTime.UtcNow;
    public string? UpdateById { get; set; }
    public DateTime? UpdateOn { get; set; }
    public ApplicationUser CreateBy { get; set; } = default!;
    public ApplicationUser? UpdateBy { get; set; } = default!;
}
