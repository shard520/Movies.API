using System.ComponentModel.DataAnnotations;

namespace Movies.API.Models
{
    public class Actor : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; }

        public Actor()
        {
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
