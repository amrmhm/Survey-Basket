namespace SurveyBasket.Api.Services;

public interface IResualtServices
{
    public Task<Resault<ResponsePollVotes>> GetPollVoteAsync(int pollid, CancellationToken cancellationToken = default);
    public Task<Resault<IEnumerable<ResponseVotePerDay>>> GetVotePerDayAsync(int pollid, CancellationToken cancellationToken = default);
    public Task<Resault<IEnumerable<ResponseVotePerQuestion>>> GetVotePerQuestionAsync(int pollid, CancellationToken cancellationToken = default);
}
