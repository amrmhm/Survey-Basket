using System.Diagnostics.Contracts;

namespace SurveyBasket.Api.Entites;

public class Vote
{
    public int Id { get; set; }

    public int PollId { get; set; }

    public string UserId { get; set; } =string.Empty;

    public DateTime SubmitOn { get; set; } = DateTime.UtcNow;



    public ApplicationUser User { get; set; } = default!;
    public Poll Poll { get; set; } = default!;

    public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];
}
