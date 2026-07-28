using Microsoft.JSInterop;

namespace Profesional.Frontend.Services
{
    public class NotificationService
    {
        private readonly IJSRuntime _jsRuntime;

        public NotificationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowSuccessAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("Swal.fire", "Éxito", message, "success");
        }

        public async Task ShowErrorAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("Swal.fire", "Error", message, "error");
        }

        public async Task ShowWarningAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("Swal.fire", "Advertencia", message, "warning");
        }

        public async Task ShowInfoAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("Swal.fire", "Información", message, "info");
        }

        public async Task<bool> ShowConfirmAsync(string title, string message)
        {
            var result = await _jsRuntime.InvokeAsync<bool>("Swal.fire", new
            {
                title = title,
                text = message,
                icon = "question",
                showCancelButton = true,
                confirmButtonText = "Sí",
                cancelButtonText = "No"
            });
            return result;
        }
    }
}