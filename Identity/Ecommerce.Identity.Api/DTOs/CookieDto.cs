using Ecommerce.Identity.Api.Data.Entities;

namespace Ecommerce.Identity.Api.DTOs
{
    public class CookieDto
    {
        public Session Session { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
