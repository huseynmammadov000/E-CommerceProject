using Ecommerce.Shared.Models.Common;

namespace Ecommerce.Startup.Api.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Startup> Startups { get; set; }
    }
}
