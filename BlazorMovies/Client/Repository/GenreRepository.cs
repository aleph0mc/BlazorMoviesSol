using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private const string URL = "api/genres";

        private readonly IHttpService _httpService;

        public GenreRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task CreateGenre(Genre genre)
        {
            var response = await _httpService.Post(URL, genre);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
