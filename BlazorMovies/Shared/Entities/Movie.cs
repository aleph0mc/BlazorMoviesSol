using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMovies.Shared.Entities
{
    /// <summary>
    /// The code in BlazorMovies.Shared project can be used in the Client or Server projects
    /// </summary>
    public class Movie
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
