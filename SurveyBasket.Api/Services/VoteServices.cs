using SurveyBasket.Api.Contract.Votes;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class VoteServices(ApplicationDbContext context) : IVoteServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Resault> AddAsync(int pollId, string userId, RequestVotes request, CancellationToken cancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(c => c.PollId == pollId && c.UserId == userId, cancellationToken);
        if (hasVote)
            return Resault.Faliure(VoteErrors.DuplicateVote);

        var isExistPoll = await _context.Polls
            .AnyAsync(c => c.Id == pollId && c.IsPublished && c.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
            && c.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        if (!isExistPoll)
            return Resault.Faliure(PollErrors.NotFound);

        var avaliableQuestion = await _context.Questions
            .Where(c => c.PollId == pollId && c.Active)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);
        if (!avaliableQuestion.SequenceEqual(request.AnswerVotes.Select(c => c.QuestionId)))
            return Resault.Faliure(VoteErrors.InvalidQuestion);

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.AnswerVotes.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };

        await _context.Votes.AddAsync(vote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Resault.Success();



    }
}
