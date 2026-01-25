using SurveyBasket.Api.Contract.Answer;
using SurveyBasket.Api.Contract.Question;
using SurveyBasket.Api.Persistence;
using System.Collections.Generic;

namespace SurveyBasket.Api.Services;


public class QuestionServices(ApplicationDbContext context) : IQuestionServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Resault<IEnumerable<ResponseQuestion>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var isExistPoll = await _context.Polls.AnyAsync(c => c.Id == pollId, cancellationToken);

        if (!isExistPoll)
            return Resault.Faliure<IEnumerable<ResponseQuestion>>(PollErrors.NotFound);

        var question = await _context.Questions.
            Where(c => c.PollId == pollId)
            .Include(c => c.Answer)
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
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        return Resault.Success<IEnumerable<ResponseQuestion>>(question);
    }

    public async Task<Resault<IEnumerable<ResponseQuestion>>> GetAvalibaleAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(c => c.PollId == pollId && c.UserId == userId, cancellationToken);
        if (hasVote)
            return Resault.Faliure<IEnumerable<ResponseQuestion>>(VoteErrors.DuplicateVote);

        var isExistPoll = await _context.Polls
            .AnyAsync(c => c.Id == pollId && c.IsPublished && c.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
            && c.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

         if(isExistPoll)
            return Resault.Faliure<IEnumerable<ResponseQuestion>>(PollErrors.NotFound);

        var question = await _context.Questions
             .Where(c => c.PollId == pollId && c.Active)
             .Include(c => c.Answer)
             .Select(c => new ResponseQuestion(
                 c.Id ,
                 c.Content,
                 c.Answer.Where(c => c.Active)
                 .Select(c => new ResponseAnswer(
                     c.Id ,
                     c.Content
                 ))))
             .AsNoTracking()
             .ToListAsync(cancellationToken);
        return Resault.Success<IEnumerable<ResponseQuestion>>(question);
                 
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
        return Resault.Success();

    }
    public async Task<Resault> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var currentQuestion = await _context.Questions.SingleOrDefaultAsync(c => c.Id == id && c.PollId == pollId, cancellationToken);
        if (currentQuestion is null)
            return Resault.Faliure(QuestionErrors.NotFound);

        currentQuestion.Active = !currentQuestion.Active;
        await _context.SaveChangesAsync(cancellationToken);
        return Resault.Success();
    }

   
}
