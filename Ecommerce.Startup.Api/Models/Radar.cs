using Ecommerce.Shared.Models.Common;

namespace Ecommerce.Startup.Api.Models
{
    public class Radar:BaseEntity
    {
        public Guid UserId { get; set; }
        public ICollection<Startup>? Startups { get; set; }

    }
}
