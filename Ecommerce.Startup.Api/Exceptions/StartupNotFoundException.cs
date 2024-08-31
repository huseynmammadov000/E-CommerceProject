using Ecommerce.Shared.Exceptions;


namespace Ecommerce.Startup.Api.Exceptions;

public sealed class StartupNotFoundException : BaseException
{
    public sealed override string Message => "Startup not found";
}