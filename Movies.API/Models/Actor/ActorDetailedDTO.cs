namespace Movies.API.Models
{
    public class ActorDetailedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieDTO> Movies { get; set; }
    }
}
