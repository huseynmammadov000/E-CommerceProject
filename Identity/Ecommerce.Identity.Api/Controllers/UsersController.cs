using System.Net.Mime;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;

using Ecommerce.Shared.Models.Responses;

using Ecommerce.Identity.Api.Models.Requests;
using Ecommerce.Identity.Api.Models.Responses;
using Ecommerce.Identity.Api.Services.Abstractions;


namespace Ecommerce.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class UsersController(IUsersService usersService)
    : ControllerBase
{
    private readonly IUsersService _usersService = Guard.Against.Null(usersService);


    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetUserResponse>), StatusCodes.Status200OK)]
    public async Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid id)
    {
        return new ApiResponse<GetUserResponse>
        {
            Result = true,
            Payload = await _usersService.GetByIdAsync(id)
        };
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BaseModelCreationResponse>), StatusCodes.Status200OK)]
    public async Task<ApiResponse<BaseModelCreationResponse>> CreateAsync([FromBody] CreateUserRequest createUserRequest)
    {
        return new ApiResponse<BaseModelCreationResponse>
        {
            Result = true,
            Payload = await _usersService.CreateAsync(createUserRequest)
        };
    }

    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> UpdateAsync([FromBody] UpdateUserRequest updateUserRequest)
    {
        await _usersService.UpdateAsync(updateUserRequest);

        return new ApiResponse
        {
            Result = true
        };
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> DeleteAsync(Guid id)
    {
        await _usersService.DeleteAsync(id);

        return new ApiResponse
        {
            Result = true
        };
    }
}