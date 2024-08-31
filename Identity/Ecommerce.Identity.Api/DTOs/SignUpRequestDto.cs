namespace Ecommerce.Identity.Api.DTOs
{
    public class SignUpRequestDto
    {
        public  string Name { get; set; }

        public  string Email { get; set; }

        public  string Password { get; set; }
        public  string RoleName { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientUserAgent { get; set; }
    }
}
