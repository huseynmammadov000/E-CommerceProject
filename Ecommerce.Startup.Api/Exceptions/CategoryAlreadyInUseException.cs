using Ecommerce.Shared.Exceptions;

namespace Ecommerce.Startup.Api.Exceptions
{
    public class CategoryAlreadyInUseException : BaseException
    {
        public override string Message => "Category Already In Use ";
    }
}
