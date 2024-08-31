namespace Ecommerce.Shared.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException() { }

    protected BaseException(string? message) : base(message) { }

    public abstract override string Message { get; }
}