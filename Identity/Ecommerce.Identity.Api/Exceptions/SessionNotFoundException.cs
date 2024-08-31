using Ecommerce.Shared.Exceptions;

namespace Ecommerce.Identity.Api.Exceptions
{
    public class SessionNotFoundException : BaseException
    {
        public override string Message => "Session Not Found";
    }
}
