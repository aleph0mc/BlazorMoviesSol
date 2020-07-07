using BlazorMovies.Shared.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Data
{
    public class DataContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions
        ) : base(options, operationalStoreOptions) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieRating> MovieRating { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MoviesActors>()
                .HasKey(ma => new { ma.MovieId, ma.PersonId });

            builder.Entity<MoviesGenres>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            base.OnModelCreating(builder);
        }

    }
}
