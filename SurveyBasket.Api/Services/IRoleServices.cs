using SurveyBasket.Api.Contract.Role;
using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Services;

public interface IRoleServices
{
    public Task<IEnumerable<ResponseRole>> GetAllAsync(bool? includeDelete = false, CancellationToken cancellationToken = default);

    public  Task<Resault<ResponseRoleDatails>> GetAsync(string id);
    public  Task<Resault<ResponseRoleDatails>> CreateAsync(RequestRole request);
    public Task<Resault> UpdateAsync(string id, RequestRole request);
    public Task<Resault> ToggleStatusAsync(string id);
}
