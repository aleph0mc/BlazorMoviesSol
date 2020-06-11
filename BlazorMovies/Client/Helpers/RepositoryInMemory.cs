using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public class RepositoryInMemory : IRepository
    {
        public List<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie {
                    Title = "Spider-Man: Far From Home", ReleaseDate = new DateTime(2019, 7, 2),
                    Poster = "https://images-na.ssl-images-amazon.com/images/I/71HQ7lBgbGL._AC_SL1000_.jpg"
                },
                new Movie {
                    Title = "Hulk", ReleaseDate = new DateTime(2003, 11, 23),
                Poster = "https://www.cinematographe.it/wp-content/uploads/2017/09/hulk31n-1-web.jpg"
                },
                new Movie {Title = "Terminator", ReleaseDate = new DateTime(1990, 7, 16),
                Poster = "https://alternativemovieposters.com/wp-content/uploads/2020/04/sheahan_terminator.jpg"
                }
            };
        }
    }
}
