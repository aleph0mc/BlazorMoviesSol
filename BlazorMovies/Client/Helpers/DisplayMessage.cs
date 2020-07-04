using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    /// <summary>
    ///Helper for SweetAlert2 JavaScript library
    ///Ref. https://sweetalert2.github.io/
    /// </summary>
    public class DisplayMessage : IDisplayMessage
    {
        private readonly IJSRuntime js;

        public DisplayMessage(IJSRuntime js)
        {
            this.js = js;
        }

        private async ValueTask DoDisplayMessage(string title, string message, string messageType)
        {
            await js.InvokeVoidAsync("Swal.fire", title, message, messageType);
        }

        public async ValueTask DisplayErrorMessage(string message)
        {
            await DoDisplayMessage("Error", message, "error");
        }

        public async ValueTask DisplaySuccessMessage(string message)
        {
            await DoDisplayMessage("Success", message, "success");
        }
    }
}
