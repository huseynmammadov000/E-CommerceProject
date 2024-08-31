using System.Net.Mime;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;

using Ecommerce.Shared.Models.Responses;
using Ecommerce.Startup.Api.Requests;
using Ecommerce.Startup.Api.Services.Abstractions;
using Ecommerce.Startup.Api.Repositories.Abstractions;


namespace Ecommerce.Startup.Api.Controller;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class StartupController(IStartupService startupService) : ControllerBase
{
    private readonly IStartupService _startupService = Guard.Against.Null(startupService);



    [HttpPost("create-startup")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> CreateStartupAsync([FromBody] CreateStartupRequest request)
    {
        return new ApiResponse()
        {
            Result = true,
            Payload = await _startupService.AddAsync(request)
        };
    }

    [HttpPut("update-startup")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> UpdateStartupAsync([FromBody] UpdateStartupRequest request)
    {
        await _startupService.UpdateAsync(request);
        return new ApiResponse()
        {
            Result = true,

        };
    }

    [HttpDelete("delete-startup")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> DeleteStartupAsync([FromBody] Guid id)
    {
        await _startupService.DeleteAsync(id);
        return new ApiResponse()
        {
            Result = true,

        };
    }

    [HttpGet("get-startup-by-id")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> GetByIdAsync([FromBody] Guid id)
    {

        return new ApiResponse()
        {
            Result = true,
            Payload = await _startupService.GetByIdAsync(id)

        };
    }

    [HttpGet("get-by-startupName")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> GetByStartupNameAsync([FromBody] string name)
    {

        return new ApiResponse()
        {
            Result = true,
            Payload = await _startupService.GetByStartupNameAsync(name)

        };
    }

    [HttpGet("get-by-category")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> GetByCategoryAsync([FromBody] string categoryName)
    {

        return new ApiResponse()
        {
            Result = true,
            Payload = await _startupService.GetByCategoryAsync(categoryName)

        };
    }

    [HttpGet("get-all-startups")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ApiResponse> GetAllAsync()
    {

        return new ApiResponse()
        {
            Result = true,
            Payload = await _startupService.GetAllAsync()

        };
    }
}