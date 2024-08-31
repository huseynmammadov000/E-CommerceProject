using Ecommerce.Shared.Models.Responses;
using Ecommerce.Startup.Api.Requests;
using Ecommerce.Startup.Api.Services;
using Ecommerce.Startup.Api.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Startup.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("create-category")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ApiResponse> CreateCategoryAsync([FromBody] CreateCategoryRequest request)
        {
            return new ApiResponse()
            {
                Result = true,
                Payload = await _categoryService.AddAsync(request)
            };
        }

        [HttpPut("update-category")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ApiResponse> UpdateCategoryAsync([FromBody] UpdateCategoryRequest request)
        {
            await _categoryService.UpdateAsync(request);
            return new ApiResponse()
            {
                Result = true,
                
            };
        }

        [HttpDelete("delete-category")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ApiResponse> DeleteCategoryAsync([FromBody] Guid categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return new ApiResponse()
            {
                Result = true,
                
            };
        }

        [HttpGet("get-category-by-id")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ApiResponse> GetCategoryByIdAsync([FromBody]Guid categoryId)
        {
            return new ApiResponse()
            {
                Result = true,
                Payload = await _categoryService.GetByIdAsync(categoryId)
            };
        }

        [HttpGet("get-category-by-name")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ApiResponse> GetCategoryByNameAsync([FromBody] string categoryName)
        {
            return new ApiResponse()
            {
                Result = true,
                Payload = await _categoryService.GetByCategoryNameAsync(categoryName)
            };
        }

        [HttpGet("get-all-category")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ApiResponse> GetAllCategoryAsync()
        {
            return new ApiResponse()
            {
                Result = true,
                Payload = await _categoryService.GetAllAsync()
            };
        }
    }
}
