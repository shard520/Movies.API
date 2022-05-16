using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.API.Models
{
    public class Movie : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Rating { get; set; }
        public int? YearOfRelease { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; }

        public Movie()
        {
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
