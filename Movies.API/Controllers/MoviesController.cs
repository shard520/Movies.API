using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Movies.API.Models;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailedDTO>>> GetMovies()
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            return await _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(a => a.Actor)
                .AsNoTracking()
                .Select(x => MovieToDTO(x))
                .ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailedDTO>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(a => a.Actor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (movie == null)
            {
                return NotFound();
            }

            return MovieToDTO(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieDetailedDTO movieDTO)
        {
            if (id != movieDTO.Id)
            {
                return BadRequest();
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Name = movieDTO.Name;
            movie.Rating = movieDTO.Rating;
            movie.YearOfRelease = movieDTO.YearOfRelease;

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MovieExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetailedDTO>> CreateMovie(MovieDetailedDTO movieDTO)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'MovieContext.Movies'  is null.");
            }

            var movie = new Movie
            {
                Name = movieDTO.Name,
                Rating = movieDTO.Rating,
                YearOfRelease = movieDTO.YearOfRelease,
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, MovieToDTO(movie));
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static MovieDetailedDTO MovieToDTO(Movie movie)
        {
            var actors = new List<ActorDTO>();

            foreach (var entry in movie.MovieActors)
            {
                var actor = new ActorDTO()
                {
                    Id = entry.ActorId,
                    Name = entry.Actor.Name
                };
                actors.Add(actor);
            }

            return new MovieDetailedDTO
            {
                Id = movie.Id,
                Name = movie.Name,
                Rating = movie.Rating,
                YearOfRelease = movie.YearOfRelease,
                Actors = actors
            };

        }    
    }
}
