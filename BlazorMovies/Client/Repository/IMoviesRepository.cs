using BlazorMovies.Shared.DTOs;
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
        Task DeleteMovie(int id);
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<DetailsMovieDTO> GetMovie(int id);
        Task<MovieUpdateDTO> GetMovieForUpdate(int id);
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO);
        Task UpdateMovie(Movie movie);
    }
}
