using SurveyBasket.Api.Contract.Votes;

namespace SurveyBasket.Api.Services;

public interface IVoteServices
{
    public Task<Resault> AddAsync (int pollId , string userId , RequestVotes request , CancellationToken cancellationToken = default);
}
