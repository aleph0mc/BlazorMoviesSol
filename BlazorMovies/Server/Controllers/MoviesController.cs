﻿using AutoMapper;
using BlazorMovies.Server.Data;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MoviesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public MoviesController(DataContext context, IFileStorageService fileStorageService,
            IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailsMovieDTO>> GetMovie(int id)
        {
            var movie = await _context.Movies.Where(m => id == m.Id)
                .Include(mg => mg.MoviesGenres).ThenInclude(g => g.Genre)
                .Include(ma => ma.MoviesActors).ThenInclude(p => p.Person)
                .FirstOrDefaultAsync();

            if (null == movie)
                return NotFound($"No movie found with id:{id}");

            var voteAverage = 0.0;
            var userVote = 0;

            if (await _context.MovieRating.AnyAsync(m => id == m.MovieId))
            {
                voteAverage = await _context.MovieRating.Where(m => id == m.MovieId).AverageAsync(r => r.Rate);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
                    var userId = user.Id;

                    var userVoteDB = await _context.MovieRating
                        .FirstOrDefaultAsync(m => id == m.MovieId && userId == m.UserId);

                    if (null != userVoteDB)
                        userVote = userVoteDB.Rate;
                }
            }

            movie.MoviesActors = movie.MoviesActors.OrderBy(o => o.Order).ToList();

            var detailsMovieDTO = new DetailsMovieDTO
            {
                Movie = movie,
                Genres = movie.MoviesGenres.Select(g => g.Genre).ToList(),
                Actors = movie.MoviesActors.Select(x => new Person
                {
                    Name = x.Person.Name,
                    Picture = x.Person.Picture,
                    Character = x.Character,
                    Id = x.PersonId

                })
                .ToList(),
                AverageVote = voteAverage,
                UserVote = userVote
            };

            return detailsMovieDTO;
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Movie>>> Filter(FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }

            if (filterMoviesDTO.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InTheaters);
            }

            if (filterMoviesDTO.UpcomingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }

            await HttpContext.InsertPaginationParametersInResponse(moviesQueryable,
                filterMoviesDTO.RecordsPerPage);

            var movies = await moviesQueryable.Paginate(filterMoviesDTO.Pagination).ToListAsync();

            return movies;
        }


        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
            var movieActionResult = await GetMovie(id);
            if (movieActionResult.Result is NotFoundResult)
                return NotFound();

            var movieDetailDTO = movieActionResult.Value;
            var selectedGenres = movieDetailDTO.Genres.Select(g => g.Id).ToList();
            var notSelecgtedGenres = await _context.Genres.Where(g => !selectedGenres.Contains(g.Id))
                .ToListAsync();

            var model = new MovieUpdateDTO
            {
                Movie = movieDetailDTO.Movie,
                SelecgtedGenres = movieDetailDTO.Genres,
                NotselectedGenres = notSelecgtedGenres,
                Actors = movieDetailDTO.Actors
            };

            return model;
        }

        [HttpGet]
        [AllowAnonymous]
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

        [HttpPut]
        public async Task<IActionResult> Put(Movie movie)
        {
            var movieDb = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movie.Id);

            if (null == movieDb)
                return NotFound();

            movieDb = _mapper.Map(movie, movieDb);

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var personPicture = Convert.FromBase64String(movie.Poster);
                movie.Poster = await _fileStorageService.EditFile(personPicture, "jpg", "movies", movieDb.Poster);
            }

            // Mark these records for deletion
            _context.MoviesActors.RemoveRange(_context.MoviesActors.Where(ma => movie.Id == ma.MovieId));
            _context.MoviesGenres.RemoveRange(_context.MoviesGenres.Where(mg => movie.Id == mg.MovieId));

            if (null != movie.MoviesActors)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                    movie.MoviesActors[i].Order = i + 1;
            }

            movieDb.MoviesActors = movie.MoviesActors;
            movieDb.MoviesGenres = movie.MoviesGenres;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movieToDelete = await _context.Movies.FirstOrDefaultAsync(p => id == p.Id);
            if (null == movieToDelete)
                return NotFound($"No movie found with ID: {id}");

            _context.Movies.Remove(movieToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
