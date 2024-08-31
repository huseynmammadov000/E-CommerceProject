using Ecommerce.Shared.Exceptions;


namespace Ecommerce.Identity.Api.Exceptions;

public sealed class EmailAlreadyInUseException : BaseException
{
    public sealed override string Message => "Email already in use";
}