
using Azure.Core;
using SurveyBasket.Api.Persistence;
using System.Threading;

namespace SurveyBasket.Api.Services;

public class PollsServices(ApplicationDbContext context) : IPollsServices
{
    private readonly ApplicationDbContext _context = context;
    public async Task<IEnumerable<ResponsePoll>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Polls
        .ProjectToType<ResponsePoll>()
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ResponsePoll>> GetCurrentAsync(CancellationToken cancellationToken = default) =>

     await _context.Polls
        .Where(c => c.IsPublished && c.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && c.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
        .ProjectToType<ResponsePoll>()
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    public async Task<Resault<ResponsePoll>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
       var poll =  await _context.Polls.FindAsync(id, cancellationToken);
        return  (poll is not null) 
             ? Resault.Success(poll.Adapt<ResponsePoll>())
             : Resault.Faliure<ResponsePoll>(PollErrors.NotFound);
    }

    public async Task<Resault<ResponsePoll>> CreateAsync(RequestPoll request, CancellationToken cancellationToken = default)
    {
        var isExistTitle = await _context.Polls.AnyAsync(c => c.Title == request.Title);
        if (isExistTitle)
            return Resault.Faliure<ResponsePoll>(PollErrors.DuplicatePoll);
        var polls = request.Adapt<Poll>();
       await _context.Polls.AddAsync(polls, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Resault.Success(polls.Adapt<ResponsePoll>());

    }

    public async Task<Resault> UpdateAsync(int id, RequestPoll request, CancellationToken cancellationToken = default)
    {
        var isExistTitle = await _context.Polls.AnyAsync(c => c.Title == request.Title && c.Id != id ,cancellationToken);
        if (isExistTitle)
            return Resault.Faliure(PollErrors.DuplicatePoll);

        var currentPoll = await _context.Polls.FindAsync(id ,cancellationToken);
        if (currentPoll is null)
            return Resault.Faliure(PollErrors.NotFound);

        currentPoll.Title = request.Title;
        currentPoll.Summary = request.Summary;
        currentPoll.StartsAt = request.StartsAt;
        currentPoll.EndsAt = request.EndsAt;
        await _context.SaveChangesAsync(cancellationToken);

        return Resault.Success();

    }

    public async Task<Resault> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
        if (currentPoll is null)
            return Resault.Faliure(PollErrors.NotFound);
        _context.Polls.Remove(currentPoll);
        await _context.SaveChangesAsync();
        return  Resault.Success();
    }
    public async Task<Resault> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
        if (currentPoll is null)
            return Resault.Faliure(PollErrors.NotFound);

        currentPoll.IsPublished = !currentPoll.IsPublished;
        await _context.SaveChangesAsync(cancellationToken);
        return Resault.Success();
    }
}
