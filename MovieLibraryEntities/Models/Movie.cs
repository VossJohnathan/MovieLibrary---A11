namespace MovieLibraryEntities.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        // Entity Framework Navigational Properties
        // Describes the relationship between tables.

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}