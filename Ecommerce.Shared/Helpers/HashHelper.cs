using System.Text;
using System.Security.Cryptography;

using Ecommerce.Shared.Helpers.Abstractions;


namespace Ecommerce.Shared.Helpers;

public class HashHelper : IHashHelper
{
    public async Task<string> ToSHA256Async(string rawData)
    {
        return await Task.Run(() =>
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        });
    }
}