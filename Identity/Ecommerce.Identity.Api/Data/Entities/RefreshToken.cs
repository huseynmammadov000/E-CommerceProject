using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Identity.Api.Data.Entities;

[Table(nameof(RefreshToken), Schema = IdentityDbContext.Schema)]
public class RefreshToken
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("userId")]
    public Guid UserId { get; set; }

    [Column("expiredAt")]
    public DateTime ExpiredAt { get; set; }
}