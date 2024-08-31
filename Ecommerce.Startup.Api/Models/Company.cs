using Ecommerce.Shared.Models.Common;


namespace Ecommerce.Startup.Api.Models;

public class Company : BaseEntity
{
    public string Name { get; set; }

    public Guid UserId { get; set; }
}