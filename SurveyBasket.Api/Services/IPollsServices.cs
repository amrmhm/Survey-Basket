namespace SurveyBasket.Api.Services;

public interface IPollsServices
{

   
    public IEnumerable<Poll> GetAll();
    public Poll? Get (int id);
    public Poll Create(Poll poll);
    public bool Update(int id, Poll poll);
    public bool Delete(int id);
}
