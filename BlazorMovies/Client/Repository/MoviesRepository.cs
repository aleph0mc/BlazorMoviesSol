using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        private async Task<T> Get<T>(string url)
        {
            var response = await _httpService.Get<T>(url);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<DetailsMovieDTO> GetMovie(int id)
        {
            return await Get<DetailsMovieDTO>($"{URL}/{id}");
        }

        public async Task<IndexPageDTO> GetIndexPageDTO()
        {
            return await Get<IndexPageDTO>(URL);
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
