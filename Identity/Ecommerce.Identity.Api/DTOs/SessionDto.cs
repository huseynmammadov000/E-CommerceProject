namespace Ecommerce.Identity.Api.DTOs
{
    public class SessionDto
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string? ClientRealIp { get; set; }
        public string? ClientUserAgent { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
