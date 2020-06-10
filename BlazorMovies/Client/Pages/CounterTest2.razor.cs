using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Pages
{
    public partial class CounterTest2
    {
        [Inject] public SingletonService ss { get; set; }
        [Inject] public TransientService ts { get; set; }
        [Inject] public IRepository repo { get; set; }

        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;

            ss.Value = currentCount * 3;
            ts.Value = currentCount * 2;
        }

        private List<Movie> movies;

        protected override void OnInitialized()
        {
            movies = new List<Movie>
            {
            new Movie {Title = "Contact", ReleaseDate = new DateTime(1992, 7, 2)  },
            new Movie {Title = "2001 A Space Odissey", ReleaseDate = new DateTime(1968, 11, 23)  },
            };
        }
    }
}
