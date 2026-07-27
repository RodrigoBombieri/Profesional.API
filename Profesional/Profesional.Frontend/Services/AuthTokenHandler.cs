using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace Profesional.Frontend.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthTokenHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "token");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
