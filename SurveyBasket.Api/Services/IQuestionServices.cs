using SurveyBasket.Api.Contract.Question;

namespace SurveyBasket.Api.Services;

public interface IQuestionServices
{
    public Task<Resault<IEnumerable<ResponseQuestion>>> GetAllAsync(int pollId, CancellationToken cancellationToken);
    public Task<Resault<IEnumerable<ResponseQuestion>>> GetAvalibaleAsync(int pollId,string userId, CancellationToken cancellationToken);
    public Task<Resault<ResponseQuestion>> GetAsync(int pollId, int id , CancellationToken cancellationToken);
    public Task<Resault<ResponseQuestion>> CreateAsync (int pollId ,RequestQuestion request ,CancellationToken cancellationToken);

    public Task<Resault> UpdateAsync(int pollId, int id, RequestQuestion request , CancellationToken cancellationToken = default);
    public Task<Resault> ToggleStatusAsync(int pollId ,int id, CancellationToken cancellationToken = default);
}
