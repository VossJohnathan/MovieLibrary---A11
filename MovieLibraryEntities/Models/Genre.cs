namespace MovieLibraryEntities.Models
{
    public class Genre
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // Entity Framework Navigational Properties
        // Describes the relationship between tables.

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
