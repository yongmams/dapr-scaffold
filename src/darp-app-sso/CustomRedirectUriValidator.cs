using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace DaprApp.SSO
{
    public class CustomRedirectUriValidator : StrictRedirectUriValidator, IRedirectUriValidator
    {
        public override Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            var uri = new Uri(requestedUri, UriKind.Absolute);

            if (uri.Host == "localhost" || uri.Host == "127.0.0.1")
            {
                return Task.FromResult(true);
            }

            return base.IsRedirectUriValidAsync(requestedUri, client);
        }
    }
}
