namespace MovieLibraryEntities.Models
{
    public class MovieGenre
    {
        public int Id { get; set; }

        // Entity Framework Navigational Properties
        // Describes the relationship between tables.

        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
