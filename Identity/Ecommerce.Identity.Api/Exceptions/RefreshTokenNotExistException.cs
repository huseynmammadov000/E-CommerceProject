using Ecommerce.Shared.Exceptions;

namespace Ecommerce.Identity.Api.Exceptions
{
    public class RefreshTokenNotExistException : BaseException
    {
        public override string Message => "RefreshToken not exist";
    }
}
