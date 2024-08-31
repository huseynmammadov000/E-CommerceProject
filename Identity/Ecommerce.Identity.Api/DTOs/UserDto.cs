using Ecommerce.Shared.Models.Common;


namespace Ecommerce.Identity.Api.DTOs;

public class UserDto : BaseEntity
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public ICollection<UserRoleDto> UserRoles { get; set; }
}