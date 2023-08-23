using System.Net.Http.Headers;

namespace DapApp.Admin.API
{
    public class AuthHeaderInterceptor : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHeaderInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request.Headers.Authorization.Any())
            {
                var authHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization.First();
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeader);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

    public static class AuthHeaderInterceptorExtensions
    {
        public static IServiceCollection AddAuthHeaderInterceptor(this IServiceCollection services)
        {


            return services;
        }
    }
}
