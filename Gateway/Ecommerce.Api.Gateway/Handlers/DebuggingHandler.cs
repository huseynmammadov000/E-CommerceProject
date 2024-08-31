namespace Ecommerce.Api.Gateway.Handlers;

public class DebuggingHandler : DelegatingHandler
{
    protected sealed override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default
    )
    {
        return await base.SendAsync(request, cancellationToken);
    }
}