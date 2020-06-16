using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private const string URL = "api/movies";

        private readonly IHttpService _httpService;

        public MoviesRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await _httpService.Post(URL, movie);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

    }
}
