
using Microsoft.Extensions.Caching.Memory;
using SurveyBasket.Api.Contract.Answer;
using SurveyBasket.Api.Contract.Question;
using SurveyBasket.Api.Persistence;
using System.Collections.Generic;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.Api.Contract.Common;
using System.Linq.Dynamic.Core;
using MailKit.Search;

namespace SurveyBasket.Api.Services;


public class QuestionServices(ApplicationDbContext context , HybridCache hybridCache ) : IQuestionServices
{
    private readonly ApplicationDbContext _context = context;
    private readonly HybridCache _hybridCache = hybridCache;

    private const string _cachePrefix = "avalibaleQuestion";
    //private readonly ICacheServices _cacheServices = cacheServices;
    //private readonly IOutputCacheStore _outputCacheStore = outputCacheStore;
    //private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<Resault<PaginatedList<ResponseQuestion>>> GetAllAsync(int pollId,RequestFilter filter , CancellationToken cancellationToken = default)
    {
        var isExistPoll = await _context.Polls.AnyAsync(c => c.Id == pollId, cancellationToken);

        if (!isExistPoll)
            return Resault.Faliure<PaginatedList<ResponseQuestion>>(PollErrors.NotFound);

        var query = _context.Questions.Where(c => c.PollId == pollId );

        if(!string.IsNullOrEmpty(filter.SearchValue))
        {
            query = query.Where(c => c.Content.Contains(filter.SearchValue));
        }
        if(!string.IsNullOrEmpty(filter.SortColumn))
        {
            query = query.OrderBy($"{filter.SortColumn} {filter.SortDirection}");
        }
           
           var source = query.Include(c => c.Answer)
            //.Select(c => new ResponseQuestion
            //(
            //    c.Id,
            //    c.Content,
            //    c.Answer.Select(q => new ResponseAnswer
            //    (
            //        q.Id,
            //        q.Content
            //     ))))
            .ProjectToType<ResponseQuestion>()
                .AsNoTracking();
        var question = await PaginatedList<ResponseQuestion>.CreateAsync(source, filter.PageNumber, filter.PageSize, cancellationToken);
        return Resault.Success<PaginatedList<ResponseQuestion>>(question);
    }

    public async Task<Resault<IEnumerable<ResponseQuestion>>> GetAvalibaleAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(c => c.PollId == pollId && c.UserId == userId, cancellationToken);
        if (hasVote)
            return Resault.Faliure<IEnumerable<ResponseQuestion>>(VoteErrors.DuplicateVote);

        var isExistPoll = await _context.Polls
            .AnyAsync(c => c.Id == pollId && c.IsPublished && c.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
            && c.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        if (!isExistPoll)
            return Resault.Faliure<IEnumerable<ResponseQuestion>>(PollErrors.NotFound);

        // Add Unique Key To Check Key if key Inside Memory Or New Key By GetOrCreateAsync
        var cacheKey = $"{_cachePrefix}-{pollId}";

        //Add Hybrid Cache 
        var question = await _hybridCache.GetOrCreateAsync<IEnumerable<ResponseQuestion>>(
            cacheKey,
           async cacheEntry => await _context.Questions
             .Where(c => c.PollId == pollId && c.Active)
             .Include(c => c.Answer)
             .Select(c => new ResponseQuestion(
                 c.Id,
                 c.Content,
                 c.Answer.Where(c => c.Active)
                 .Select(c => new ResponseAnswer(
                     c.Id,
                     c.Content
                 ))))
             .AsNoTracking()
             .ToListAsync(cancellationToken)
             //,
           //Add Expire Minutes To Hybrid Memory
           //new HybridCacheEntryOptions
           //{
           //    Expiration = TimeSpan.FromMinutes(50)
           //}
            );


        // Get Data From Cache Services
        // var cacheQuestion = await _cacheServices.GetAsync<IEnumerable<ResponseQuestion>>(cacheKey, cancellationToken);

        // IEnumerable<ResponseQuestion> question = [];
        // If Cache Null Get Data From DataBase And Set Data To Cache Services
        //if ( cacheQuestion is null)
        //{


        //    question = await _context.Questions
        //     .Where(c => c.PollId == pollId && c.Active)
        //     .Include(c => c.Answer)
        //     .Select(c => new ResponseQuestion(
        //         c.Id,
        //         c.Content,
        //         c.Answer.Where(c => c.Active)
        //         .Select(c => new ResponseAnswer(
        //             c.Id,
        //             c.Content
        //         ))))
        //     .AsNoTracking()
        //     .ToListAsync(cancellationToken);

        //    await _cacheServices.SetAsync(cacheKey, question, cancellationToken);
        //}
        //else
        //{


        //    question = cacheQuestion;
        //}

        //var question = await _memoryCache.GetOrCreateAsync(
        //    cacheKey ,
        //    cacheEntry =>
        //    {
        //        //Add Expire Minutes To Cache Memory
        //        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(50);
        //        return _context.Questions
        //     .Where(c => c.PollId == pollId && c.Active)
        //     .Include(c => c.Answer)
        //     .Select(c => new ResponseQuestion(
        //         c.Id,
        //         c.Content,
        //         c.Answer.Where(c => c.Active)
        //         .Select(c => new ResponseAnswer(
        //             c.Id,
        //             c.Content
        //         ))))
        //     .AsNoTracking()
        //     .ToListAsync(cancellationToken);
        // });


        return Resault.Success<IEnumerable<ResponseQuestion>>(question!);
                 
    }

    public async Task<Resault<ResponseQuestion>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {

        var question = await _context.Questions
            
            .Where(c => c.PollId == pollId && c.Id == id)
            .Include(c => c.Answer)
            .ProjectToType<ResponseQuestion>()
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);
        if(question == null)
            return Resault.Faliure<ResponseQuestion>(QuestionErrors.NotFound);
            return Resault.Success(question);
    }
    public async Task<Resault<ResponseQuestion>> CreateAsync(int pollId, RequestQuestion request, CancellationToken cancellationToken = default)
    {
        var isExistPoll = await _context.Polls.AnyAsync(c => c.Id == pollId ,cancellationToken);

        if (!isExistPoll)
            return Resault.Faliure<ResponseQuestion>(PollErrors.NotFound);
        var isExistQuestion = await _context.Questions.AnyAsync(c => c.Content == request.Content && c.PollId == pollId,cancellationToken);
        if(isExistQuestion)
            return Resault.Faliure<ResponseQuestion>(QuestionErrors.DuplicateQuestion);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        //Add Answer With Mapster
        //foreach (var answer in request.Answers)
        //{
        //    question.Answer.Add(new Answer { Content = answer });
        //}

        await _context.Questions.AddAsync(question ,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        // So When Do I say The OutPut Cache Has Been Deleted
        //await _outputCacheStore.EvictByTagAsync("avalibaleQuestion", cancellationToken);
        // So When Do I say The Memory Cache Has Been Deleted
        //_memoryCache.Remove($"{_cachePrefix}={pollId}");

        // So When Do I say The Distrabute Cache Has Been Deleted
        //await _cacheServices.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        // So When Do I say The hybridCache Cache Has Been Deleted
        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        var response = question.Adapt<ResponseQuestion>();
        return Resault.Success(response);
    }


    public async Task<Resault> UpdateAsync(int pollId, int id, RequestQuestion request , CancellationToken cancellationToken = default)
    {
        var isExistquestion = await _context.Questions.AnyAsync(
            c => c.Id != id &&
            c.PollId == pollId &&
            c.Content == request.Content,cancellationToken);
        if(isExistquestion)
            return Resault.Faliure(QuestionErrors.DuplicateQuestion);

        var question = await _context.Questions.Include(c => c.Answer).SingleOrDefaultAsync(
            c => c.Id == id &&
            c.PollId == pollId ,cancellationToken
            );
        if(question is null)
            return Resault.Faliure(QuestionErrors.NotFound);

        question.Content = request.Content;

        // Current Answer  in db and Change Type to List String
        var currentAnswer = question.Answer.Select(c =>  c.Content).ToList();

        // Check answer in db or in request 
        //new Answer
        var newAnswer = request.Answer.Except(currentAnswer).ToList();
        newAnswer.ForEach(answer =>
        question.Answer.Add(new Answer { Content = answer }));

        question.Answer.ToList().ForEach(answer =>
        answer.Active = request.Answer.Contains(answer.Content));
            await _context.SaveChangesAsync(cancellationToken);
        // So When Do I say The Cache Has Been Deleted
        //await _outputCacheStore.EvictByTagAsync("avalibaleQuestion", cancellationToken);

        // So When Do I say The Memory Cache Has Been Deleted
        //_memoryCache.Remove($"{_cachePrefix}={pollId}");
        // So When Do I say The Distrabute Cache Has Been Deleted
        //await _cacheServices.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        // So When Do I say The hybridCache Cache Has Been Deleted
        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);
        return Resault.Success();

    }
    public async Task<Resault> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var currentQuestion = await _context.Questions.SingleOrDefaultAsync(c => c.Id == id && c.PollId == pollId, cancellationToken);
        if (currentQuestion is null)
            return Resault.Faliure(QuestionErrors.NotFound);

        currentQuestion.Active = !currentQuestion.Active;
        await _context.SaveChangesAsync(cancellationToken);

        // So When Do I say The Cache Has Been Deleted
        //await _outputCacheStore.EvictByTagAsync("avalibaleQuestion", cancellationToken);

        // So When Do I say The Memory Cache Has Been Deleted
        // _memoryCache.Remove($"{_cachePrefix}={pollId}");

        // So When Do I say The Distrabute Cache Has Been Deleted
        //await _cacheServices.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        // So When Do I say The hybridCache Cache Has Been Deleted
        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Resault.Success();
    }

   
}
