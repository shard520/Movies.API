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
    public class ActorsController : ControllerBase
    {
        private readonly MoviesContext _context;

        public ActorsController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDetailedDTO>>> GetActors()
        {
            if (_context.Actors == null)
            {
                return NotFound();
            }
            return await _context.Actors
                .Include(a => a.MovieActors)
                .ThenInclude(m => m.Movie)
                .AsNoTracking()
                .Select(x => ActorToDTO(x))
                .ToListAsync();
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDetailedDTO>> GetActor(int id)
        {
            if (_context.Actors == null)
            {
                return NotFound();
            }
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                .ThenInclude(m => m.Movie)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return ActorToDTO(actor);
        }

        // PUT: api/Actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActor(int id, ActorDetailedDTO actorDTO)
        {
            if (id != actorDTO.Id)
            {
                return BadRequest();
            }
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            actor.Name = actorDTO.Name;

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ActorExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actor>> CreateActor(ActorDTO actorDTO)
        {
            if (_context.Actors == null)
            {
                return Problem("Entity set 'MoviesContext.Actors'  is null.");
            }

            var actor = new Actor
            {
                Name = actorDTO.Name
            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, ActorToDTO(actor));
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            if (_context.Actors == null)
            {
                return NotFound();
            }
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return (_context.Actors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static ActorDetailedDTO ActorToDTO(Actor actor)
        {
            var movies = new List<MovieDTO>();

            foreach (var entry in actor.MovieActors)
            {
                var movie = new MovieDTO()
                {
                    Id = entry.MovieId,
                    Name = entry.Movie.Name,
                    YearOfRelease = entry.Movie.YearOfRelease
                };
                movies.Add(movie);
            }

            return new ActorDetailedDTO
            {
                Id = actor.Id,
                Name = actor.Name,
                Movies = movies
            };
        }
    }
}
