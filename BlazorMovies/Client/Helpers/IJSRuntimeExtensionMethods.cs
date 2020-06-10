using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime js, string message)
        {
            //WRITE A CONSOLE LOG
            await js.InvokeVoidAsync("console.log", "confirm message:", message);

            return await js.InvokeAsync<bool>("confirm", message);
        }

        /// <summary>
        /// Custom js function
        /// The js file is in wwwroot\js\utilities.js
        /// It is added to index.html file
        /// </summary>
        /// <param name="js"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ValueTask MyJsFunction(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("my_function", message);
        }
    }
}
