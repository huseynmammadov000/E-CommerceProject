using System.ComponentModel.DataAnnotations.Schema;

using Ecommerce.Shared.Models.Common;


namespace Ecommerce.Identity.Api.Data.Entities;

[Table(nameof(Session), Schema = IdentityDbContext.Schema)]
public class Session
{
    [Column("id")]
    public string Id { get; set; }

    [Column("userId")]
    public Guid UserId { get; set; }

    [Column("clientRealIp")]
    public string? ClientRealIp { get; set; }

    [Column("clientUserAgent")]
    public string? ClientUserAgent { get; set; }

    [Column("expiredAt")]
    public DateTime ExpiredAt { get; set; }
}