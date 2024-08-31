using Ecommerce.Shared.Exceptions;

namespace Ecommerce.Startup.Api.Exceptions
{
    public class CategoryNotFoundException :BaseException
    {
        public sealed override string Message => "Category not found";
    }
}
