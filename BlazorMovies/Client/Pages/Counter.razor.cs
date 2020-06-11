using BlazorMovies.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorMovies.Client.Shared.MainLayout;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        [Inject] public SingletonService ss { get; set; }
        [Inject] public TransientService ts { get; set; }
        [Inject] public IJSRuntime js { get; set; }

        /// <summary>
        /// It's possible to use in this component the cascading parameters declared in the MainLayout component
        /// </summary>
        //[CascadingParameter(Name = "Color")] public string Color { get; set; }
        //[CascadingParameter(Name = "Size")] public string Size { get; set; }

        //Using cascading parameters class
        [CascadingParameter] public AppState AppState { get; set; }

        private int currentCount = 0;
        private static int currentCountStatic = 0;

        [JSInvokable]
        public async Task IncrementCount()
        {
            currentCount++;
            currentCountStatic++;
            ss.Value = currentCount * 3;
            ts.Value = currentCount * 2;
            //Invoke the js method dotnetStaticInvokation in utilities.js
            await js.InvokeVoidAsync("dotnetStaticInvocation");
        }

        [JSInvokable]
        public static async Task<int> GetCurrentCount()
        {
            return await Task.FromResult(currentCountStatic);
        }

        private async void IncrementCountJavascript()
        {
            await js.InvokeVoidAsync("dotnetInstanceInvocation", DotNetObjectReference.Create(this));

            //For another class ex. instance of a Movie (m)
            //await js.InvokeVoidAsync("dotnetInstanceInvocation", DotNetObjectReference.Create(m));
            //then from JS is possible to invoke a method in that class, if any.

        }
    }
}
