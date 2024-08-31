namespace Ecommerce.Identity.Api.Models.Requests
{
    public class SignInRequest
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
     
    }
}
