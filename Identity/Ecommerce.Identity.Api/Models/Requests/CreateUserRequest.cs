namespace Ecommerce.Identity.Api.Models.Requests;

public class CreateUserRequest
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}