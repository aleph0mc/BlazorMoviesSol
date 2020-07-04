using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMovies.Server.Data;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        private readonly DataContext _context;

        public GenresController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _context.Genres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => id == g.Id);

            if (null == genre)
                return NotFound();

            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return genre.Id;
        }

        [HttpPut]
        public async Task<IActionResult> Put(Genre genre)
        {
            _context.Attach(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genreToDelete = await _context.Genres.FirstOrDefaultAsync(g => id == g.Id);
            if (null == genreToDelete)
                return NotFound($"No genre found with ID: {id}");

            _context.Genres.Remove(genreToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
