
namespace SurveyBasket.Api.Services;

public class PollsServices : IPollsServices 
{
    private readonly List<Poll> _polls = [
        new Poll
        {
            Id = 1 ,
            Title = "Poll 1" ,
            Description = "My First Poll"
        } ,
        new Poll
        {
           
            Id = 2 ,
            Title = "Poll 2" ,
            Description = "My Second Poll"
        }];
   
     // new() Or [] Or new List<Poll>();
    public IEnumerable<Poll> GetAll() => _polls;
   
    
    public Poll? Get(int id) => _polls.SingleOrDefault(c => c.Id == id);

    public Poll Create(Poll poll)
    {
        poll.Id = _polls.Count + 1;
         _polls.Add(poll);
        return poll;

    }

    public bool Update(int id, Poll poll)
    {
        var currentPoll = Get(id);
        if (currentPoll is null)
            return false;
        currentPoll.Title = poll.Title;
        currentPoll.Description = poll.Description;

        return true;

    }

    public bool Delete(int id)
    {
        var currentPol = Get(id);
        if (currentPol is null)
            return false;
        _polls.Remove(currentPol);
        return true;
    }
}
