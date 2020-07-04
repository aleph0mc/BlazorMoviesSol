using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMovies.Server.Data;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RatingController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RatingController(DataContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Rate(MovieRating movieRating)
        {
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var userId = user.Id;

            var currentRating = await _context.MovieRating
                .FirstOrDefaultAsync(x => x.MovieId == movieRating.MovieId &&
                x.UserId == userId);

            if (currentRating == null)
            {
                movieRating.UserId = userId;
                movieRating.RatingDate = DateTime.Today;
                _context.Add(movieRating);
                await _context.SaveChangesAsync();
            }
            else
            {
                currentRating.Rate = movieRating.Rate;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
