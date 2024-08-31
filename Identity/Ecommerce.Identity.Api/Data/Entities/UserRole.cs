using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Identity.Api.Data.Entities;

[Table(nameof(UserRole), Schema = IdentityDbContext.Schema)]
public class UserRole
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}