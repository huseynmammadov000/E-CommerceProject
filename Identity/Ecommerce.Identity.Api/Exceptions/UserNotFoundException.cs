using Ecommerce.Shared.Exceptions;


namespace Ecommerce.Identity.Api.Exceptions;

public sealed class UserNotFoundException : BaseException
{
    public sealed override string Message => "User Not Found";
}