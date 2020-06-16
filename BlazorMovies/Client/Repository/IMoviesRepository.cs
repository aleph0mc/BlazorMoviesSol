using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    interface IMoviesRepository
    {
        Task<int> CreateMovie(Movie movie);
    }
}
