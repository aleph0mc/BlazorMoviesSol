﻿using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task<Genre> GetGenre(int id);
        Task<List<Genre>> GetGenres();
        Task UpdateGenre(Genre genre);
    }
}
