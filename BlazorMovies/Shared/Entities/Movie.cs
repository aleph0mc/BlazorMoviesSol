using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorMovies.Shared.Entities
{
    /// <summary>
    /// The code in BlazorMovies.Shared project can be used in the Client or Server projects
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool InTheaters { get; set; }
        public string Trailer { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
        public string TitleBrief
        {
            get
            {
                return string.IsNullOrEmpty(Title) ? null :
                    (Title.Length > 60 ? $"{Title.Substring(0, 60) }...." :
                    Title);
            }
        }


    }
}
