using Ecommerce.Shared.Models.Responses;

using Ecommerce.Identity.Api.Models.Requests;
using Ecommerce.Identity.Api.Models.Responses;


namespace Ecommerce.Identity.Api.Services.Abstractions;

public interface IUsersService
{
    Task<GetUserResponse> GetByIdAsync(Guid id);

    Task<BaseModelCreationResponse> CreateAsync(CreateUserRequest request);

    Task UpdateAsync(UpdateUserRequest request);

    Task DeleteAsync(Guid id);
}