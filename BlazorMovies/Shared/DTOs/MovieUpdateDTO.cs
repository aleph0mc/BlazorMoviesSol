using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMovies.Shared.DTOs
{
    public class MovieUpdateDTO
    {
        public Movie Movie { get; set; }
        public List<Person> Actors { get; set; }
        public List<Genre> SelecgtedGenres { get; set; }
        public List<Genre> NotselectedGenres { get; set; }
    }
}
