namespace SurveyBasket.Api.Entites;

public sealed class Question :AuditableEntity 
{
    public int Id { get; set; } 
    public string Content { get; set; } =string.Empty;

    public int PollId { get; set; }

    public bool Active { get; set; } = true;

    public Poll Poll { get; set; } = default!;

    public ICollection<Answer> Answer { get; set; } = [];
}
