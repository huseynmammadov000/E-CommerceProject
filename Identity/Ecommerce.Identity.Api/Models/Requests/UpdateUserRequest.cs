namespace Ecommerce.Identity.Api.Models.Requests;

public class UpdateUserRequest
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}