namespace Movies.API.Models
{
    public class MovieDetailedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Rating { get; set; }
        public int? YearOfRelease { get; set; }

        public ICollection<ActorDTO> Actors { get; set; }
    }
}
