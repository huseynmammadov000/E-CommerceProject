using Ecommerce.Identity.Api.DTOs;


namespace Ecommerce.Identity.Api.Repostories.Abstactions;

public interface IUsersRepository
{
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> GetByEmailAsync(string email);
    Task<UserDto> GetByUsernameAsync(string username);

    Task<UserDto> AddAsync(UserDto entity);

    Task UpdateAsync(UserDto entity);

    Task DeleteAsync(Guid id);
}