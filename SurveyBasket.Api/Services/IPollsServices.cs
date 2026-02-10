namespace SurveyBasket.Api.Services;

public interface IPollsServices
{

   
    public Task<IEnumerable<ResponsePoll>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<ResponsePoll>> GetCurrentAsyncV1(CancellationToken cancellationToken = default);
    public Task<IEnumerable<ResponsePollV2>> GetCurrentAsyncV2(CancellationToken cancellationToken = default);
    public Task<Resault<ResponsePoll>> GetAsync (int id , CancellationToken cancellationToken = default);
    public Task<Resault<ResponsePoll>>CreateAsync(RequestPoll poll, CancellationToken cancellationToken = default);
    public Task<Resault> UpdateAsync(int id, RequestPoll poll, CancellationToken cancellationToken = default);
    public Task<Resault> DeleteAsync(int id , CancellationToken cancellationToken = default);

    public Task<Resault> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
}
