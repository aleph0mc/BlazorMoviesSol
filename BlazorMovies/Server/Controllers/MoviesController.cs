using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMovies.Server.Data;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFileStorageService _fileStorageService;

        public MoviesController(DataContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 6;

            var moviesInTheaters = await _context.Movies.Where(m => m.InTheaters)
                .OrderByDescending(m => m.ReleaseDate).Take(limit)
                .ToListAsync();

            var todaysDate = DateTime.Today;

            var upcomingReleases = await _context.Movies.Where(m => m.ReleaseDate > todaysDate)
                .OrderByDescending(m => m.ReleaseDate).Take(limit)
                .ToListAsync();

            var response = new IndexPageDTO
            {
                InTheaters = moviesInTheaters,
                UpcomingReleases = upcomingReleases
            };

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var personPicture = Convert.FromBase64String(movie.Poster);
                //The extension jpg works with *.gif and *png as well
                movie.Poster = await _fileStorageService.SaveFile(personPicture, "jpg", "movies");
            }

            if (null != movie.MoviesActors)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                    movie.MoviesActors[i].Order = i + 1;
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }
    }
}
