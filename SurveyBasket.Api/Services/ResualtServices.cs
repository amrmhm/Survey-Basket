
using SurveyBasket.Api.Persistence;
using System.Collections.Generic;

namespace SurveyBasket.Api.Services;

public class ResualtServices(ApplicationDbContext context) : IResualtServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Resault<ResponsePollVotes>> GetPollVoteAsync(int pollid, CancellationToken cancellationToken = default)
    {

        var pollVote = await _context.Polls.
            Where(c => c.Id == pollid)
            .Select(c => new ResponsePollVotes(
                c.Title,
                c.Vote.Select(c => new ResponseVote(
                    $"{c.User.FirstName} {c.User.LastName}",
                    c.SubmitOn,
                    c.VoteAnswers.Select(c => new ResponseQuestionAnswer(
                        c.Question.Content,
                        c.Answer.Content))))))
            .SingleOrDefaultAsync(cancellationToken);
        if (pollVote is null)
            return Resault.Faliure<ResponsePollVotes>(PollErrors.NotFound);
        return Resault.Success(pollVote);
    }

    public async Task<Resault<IEnumerable<ResponseVotePerDay>>> GetVotePerDayAsync(int pollid, CancellationToken cancellationToken = default)
    {
        var pollIsExist = await _context.Polls.AnyAsync(c => c.Id == pollid, cancellationToken);
        if (!pollIsExist)
            return Resault.Faliure<IEnumerable<ResponseVotePerDay>>(PollErrors.NotFound);

        var votePerDay = await _context.Votes
            .Where(c => c.PollId == pollid)
            .GroupBy(c => new
            {
                Date = DateOnly.FromDateTime(c.SubmitOn)
            }).Select(c => new ResponseVotePerDay(
                c.Key.Date,
                c.Count()
                ))
            .ToListAsync(cancellationToken);
        return Resault.Success<IEnumerable<ResponseVotePerDay>>(votePerDay);
    }

    public async Task<Resault<IEnumerable<ResponseVotePerQuestion>>> GetVotePerQuestionAsync(int pollid, CancellationToken cancellationToken = default)
    {
        var pollIsExist = await _context.Polls.AnyAsync(c => c.Id == pollid, cancellationToken);
        if (!pollIsExist)
            return Resault.Faliure<IEnumerable<ResponseVotePerQuestion>>(PollErrors.NotFound);


        var votePerQuestion =await _context.VoteAnswers
            .Where(c => c.Vote.PollId == pollid)
            .Select ( c => new ResponseVotePerQuestion(
                c.Question.Content,
                c.Question.VoteAnswer
                .GroupBy(
                    c => new { AnswerId = c.AnswerId , AnswerContent = c.Answer.Content })
                .Select(c => new ResponseVotePerAnswer(
                   c.Key.AnswerContent ,
                   c.Count()))))
            .ToListAsync(cancellationToken);

        return Resault.Success<IEnumerable<ResponseVotePerQuestion>>(votePerQuestion);
    }
}