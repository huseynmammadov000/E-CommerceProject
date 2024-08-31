using Mapster;

using Ardalis.GuardClauses;


using Ecommerce.Shared.Helpers.Abstractions;
using Ecommerce.Shared.Models.Responses;

using Ecommerce.Identity.Api.DTOs;
using Ecommerce.Identity.Api.Models.Requests;
using Ecommerce.Identity.Api.Models.Responses;
using Ecommerce.Identity.Api.Services.Abstractions;
using Ecommerce.Identity.Api.Repostories.Abstactions;
using Ecommerce.Identity.Api.Exceptions;


namespace Ecommerce.Identity.Api.Services;

public class UsersService : IUsersService
{
    private readonly ILogger<UsersService> _logger;
    private readonly IUsersRepository _userRepository;
    private readonly IHashHelper _hashHelper;


    public UsersService(
        ILogger<UsersService> logger,
        IUsersRepository userRepository,
        IHashHelper hashHelper
    )
    {
        _logger = Guard.Against.Null(logger);
        _logger.LogInformation("Users service created.");
        _userRepository = Guard.Against.Null(userRepository);
        _hashHelper = Guard.Against.Null(hashHelper);
    }


    public async Task<GetUserResponse> GetByIdAsync(Guid id)
    {
        return (await _userRepository.GetByIdAsync(id)).Adapt<GetUserResponse>();
    }

    public async Task<BaseModelCreationResponse> CreateAsync(CreateUserRequest request)
    {
        var result = request.Adapt<UserDto>();

        result.PasswordHash = await _hashHelper.ToSHA256Async(request.Password);

        return (await _userRepository.AddAsync(result)).Adapt<BaseModelCreationResponse>();
    }

    public async Task UpdateAsync(UpdateUserRequest request)
    {
        await _userRepository.UpdateAsync(request.Adapt<UserDto>());
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }
}