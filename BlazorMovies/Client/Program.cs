using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using BlazorMovies.Client.Helpers;
using Blazor.FileReader;

namespace BlazorMovies.Client
{
    public class Program
    {
        private static string _baseAddress;

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // To configure all the servises for DI
            _baseAddress = builder.HostEnvironment.BaseAddress;
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions(); // used for auth. system
            services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(_baseAddress) });
            services.AddSingleton<SingletonService>();
            services.AddTransient<TransientService>();
            services.AddTransient<IRepository, RepositoryInMemory>();
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);
        }
    }
}
