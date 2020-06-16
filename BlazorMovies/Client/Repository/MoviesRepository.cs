using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTOs;
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

        public async Task<IndexPageDTO> GetIndexPageDTO()
        {
            var response = await _httpService.Get<IndexPageDTO>(URL);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await _httpService.Post<Movie, int>(URL, movie);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }

    }
}
