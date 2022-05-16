using Microsoft.EntityFrameworkCore;
using Movies.API.Models;

namespace Movies.API.Data
{
    public class MoviesContext : BaseDbContext<MoviesContext>
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<MovieActor> MovieActors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Movie>()
            //    .Property(m => m.CreatedDate)
            //    .HasDefaultValueSql("getdate()")
            //    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<Actor>()
            //    .Property(a => a.CreatedDate)
            //    .HasDefaultValueSql("getdate()")
            //    .ValueGeneratedOnAdd();

            //modelBuilder.Entity<Movie>()
            //    .Property(m => m.UpdatedDate)
            //    .HasDefaultValueSql("getdate()")
            //    .ValueGeneratedOnAddOrUpdate();
            //modelBuilder.Entity<Actor>()
            //    .Property(a => a.UpdatedDate)
            //    .HasDefaultValueSql("getdate()")
            //    .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<MovieActor>().
                HasKey(ma => new { ma.ActorId, ma.MovieId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(m => m.Movie)
                .WithMany(ma => ma.MovieActors)
                .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(a => a.Actor)
                .WithMany(ma => ma.MovieActors)
                .HasForeignKey(a => a.ActorId);
        }
    }
}
