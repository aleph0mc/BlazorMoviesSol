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
using BlazorMovies.Client.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Mail;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System.Globalization;

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
            builder.Services.AddHttpClient<HttpClientWithToken>(
                client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>(); //This takes care of the JWT token in the request header automatically

            builder.Services.AddHttpClient<HttpClientWithoutToken>(
              client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));


            // To configure all the servises for DI
            _baseAddress = builder.HostEnvironment.BaseAddress;
            ConfigureServices(builder.Services);

            //Get the localization from the localStorage, if any
            var host = builder.Build();
            //Get an instance of the IJSRuntime
            var js = host.Services.GetRequiredService<IJSRuntime>();
            var culture = await js.InvokeAsync<string>("getFromLocalStorage", "culture");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture =
                null == culture ? new CultureInfo("en-US") : new CultureInfo(culture);

            await host.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions(); // used for auth. system
            services.AddLocalization();
            services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(_baseAddress) });
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMoviesRepository, MoviesRepository>();
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IDisplayMessage, DisplayMessage>();
            services.AddScoped<IUsersRepository, UserRepository>();

            services.AddSingleton<SingletonService>();
            services.AddTransient<TransientService>();
            services.AddTransient<IRepository, RepositoryInMemory>();

            //File management (upload)
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);

            //IdentityServer4 authorization
            services.AddApiAuthorization();
        }
    }
}
