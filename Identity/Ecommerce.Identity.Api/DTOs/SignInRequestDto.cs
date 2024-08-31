namespace Ecommerce.Identity.Api.DTOs
{
    public class SignInRequestDto
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientUserAgent { get; set; }
    }
}
