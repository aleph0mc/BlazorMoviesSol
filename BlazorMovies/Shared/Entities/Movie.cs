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
        public string Poster { get; set; }

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
