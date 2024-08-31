namespace Ecommerce.Shared.Helpers.Abstractions;

public interface IHashHelper
{
    Task<string> ToSHA256Async(string rawData);
}