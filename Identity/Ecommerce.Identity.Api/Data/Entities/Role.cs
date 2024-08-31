using System.ComponentModel.DataAnnotations.Schema;

using Ecommerce.Shared.Models.Common;


namespace Ecommerce.Identity.Api.Data.Entities;

[Table(nameof(Role), Schema = IdentityDbContext.Schema)]
public class Role : BaseEntity
{
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}