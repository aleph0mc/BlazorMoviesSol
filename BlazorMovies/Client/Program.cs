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
using BlazorMovies.Client.Auth;
using System.Net.Mail;

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
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMoviesRepository, MoviesRepository>();
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IDisplayMessage, DisplayMessage>();

            services.AddSingleton<SingletonService>();
            services.AddTransient<TransientService>();
            services.AddTransient<IRepository, RepositoryInMemory>();
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);
            services.AddAuthorizationCore(); //Authorization component
            //services.AddScoped<AuthenticationStateProvider, DummyAuthenticationStateProvider>();

            //In JWTAuthenticationStateProvider there is the ILoginService and to use the DI we need to add this code
            //Create an instance of JWTAuthenticationStateProvider
            services.AddScoped<JWTAuthenticationStateProvider>();
            //Use same instance of JWTAuthenticationStateProvider for both AuthenticationStateProvider
            services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(
                provider => provider.GetRequiredService<JWTAuthenticationStateProvider>()
            );
            //And ILoginService
            services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(
                provider => provider.GetRequiredService<JWTAuthenticationStateProvider>()
            );

            services.AddScoped<TokenRenewer>();
        }
    }
}
