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

            await builder.Build().RunAsync();
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
