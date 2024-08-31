using Mapster;

using Ardalis.GuardClauses;

using Microsoft.EntityFrameworkCore;

using Ecommerce.Identity.Api.DTOs;
using Ecommerce.Identity.Api.Repostories.Abstactions;
using Ecommerce.Identity.Api.Exceptions;
using Ecommerce.Identity.Api.Data.Entities;
using Ecommerce.Identity.Api.Services;


namespace Ecommerce.Identity.Api.Repostories;

public class UsersRepository : IUsersRepository
{
    private readonly Data.IdentityDbContext _dbContext;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ISessionRepository _sessionRepository ;
    private readonly UserCreatedPublisher _publisher;

    public UsersRepository(
    Data.IdentityDbContext dbContext,
     IRefreshTokenRepository refreshTokenRepository,
     ISessionRepository sessionRepository,
     UserCreatedPublisher _publisher
    )
    {
        _dbContext = Guard.Against.Null(dbContext);
        _refreshTokenRepository = Guard.Against.Null(refreshTokenRepository);
        _sessionRepository = Guard.Against.Null(sessionRepository);
        _publisher = new UserCreatedPublisher();
    }


    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        return (await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id))
                .Adapt<UserDto>()
            ?? throw new UserNotFoundException();
    }


    public async Task<UserDto> AddAsync(UserDto request)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
       

        if (existingUser is not null)
            throw new EmailAlreadyInUseException();

        var user = request.Adapt<User>();
        

        await _dbContext.Users.AddAsync(user);
        //await _dbContext.SaveChangesAsync();

        //_publisher.PublishUserCreatedEvent(user.Id.ToString());

        return user.Adapt<UserDto>();
    }
    
    private async Task<UserDto> getByEmailAsync(string email)
    {
        return (await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email))
                .Adapt<UserDto>()
            ?? throw new UserNotFoundException();
    }

    public async Task UpdateAsync(UserDto request)
    {
        var existingUser = await this.GetByIdAsync(request.Id);

        var existingUserState = _dbContext.Set<User>().Local.FirstOrDefault(u => u.Id == request.Id);
        if (existingUserState != null)
            _dbContext.Users.Entry(existingUserState).State = EntityState.Detached;

        existingUser.Id = request.Id;   
        existingUser.Name = request.Name;
        existingUser.Email = request.Email;
        existingUser.PasswordHash = request.PasswordHash;

        _dbContext.Users.Update(existingUser.Adapt<User>());
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingUser = await this.GetByIdAsync(id);

        var existingUserState = _dbContext.Set<User>().Local.FirstOrDefault(e => e.Id == id);
        if (existingUserState != null)
            _dbContext.Users.Entry(existingUserState).State = EntityState.Detached;

        try
        {
            var refreshToken = await _refreshTokenRepository.GetByUserIdAsync(existingUser.Id.ToString());
            var session = await _sessionRepository.GetByUserIdAsync(existingUser.Id.ToString());

            if (refreshToken.Id != null)
                await _refreshTokenRepository.RemoveAsync(refreshToken.Id);
            if (session.Id != null)
                await _sessionRepository.RemoveAsync(session.Id.ToString());
        }
        catch (RefreshTokenNotExistException) { }
        catch (SessionNotFoundException) { }

        _dbContext.Users.Remove(existingUser.Adapt<User>());
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        return (await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email))
                .Adapt<UserDto>()
            ?? throw new UserNotFoundException();
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        return (await _dbContext.Users
              .AsNoTracking()
              .FirstOrDefaultAsync(u => u.Name == username))
              .Adapt<UserDto>()
          ?? throw new UserNotFoundException();
    }
}