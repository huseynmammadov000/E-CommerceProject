using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Ecommerce.Shared.Models.Common;


namespace Ecommerce.Identity.Api.Data.Entities;

[Table(nameof(User), Schema = IdentityDbContext.Schema)]
public class User : BaseEntity
{
    public string Name { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}