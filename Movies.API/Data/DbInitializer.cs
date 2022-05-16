using Movies.API.Models;

namespace Movies.API.Data
{
    public class DbInitializer
    {
        public static void Initialize(MoviesContext context)
        {
            context.Database.EnsureCreated();

            if (context.Movies.Any() || context.Actors.Any())
            {
                return;
            }

            var movies = new Movie[]
            {
                new Movie{Name="The Matrix", YearOfRelease=1999, Rating=9},
                new Movie{Name="Bill and Ted's Excellent Adventure", YearOfRelease=1989, Rating=9},
                new Movie{Name="The Lost Boys", YearOfRelease=1987, Rating=6}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();

            var actors = new Actor[]
            {
                new Actor{Name="Keanu Reeves"},
                new Actor{Name="Alex Winter"},
            };
            foreach (Actor a in actors)
            {
                context.Actors.Add(a);
            }
            context.SaveChanges();

            var movieActors = new MovieActor[]
            {
                new MovieActor{ActorId=1, MovieId=1},
                new MovieActor{ActorId=1, MovieId=2},
                new MovieActor{ActorId=2, MovieId=2},
                new MovieActor{ActorId=2, MovieId=3},
            };
            foreach (MovieActor mA in movieActors)
            {
                context.MovieActors.Add(mA);
            }
            context.SaveChanges();
        }
    }
}
