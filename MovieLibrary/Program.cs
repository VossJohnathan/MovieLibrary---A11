using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Migrations;
using System.Data.Common;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace MyNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // MovieLibraryOO has examples of each of these if you need it.

            //Creating main menu options & Reading user input
            Console.WriteLine("Please select your menu option.");
            Console.WriteLine("It may take some time to properly load.");
            Console.WriteLine("1) Search for a movie.");
            Console.WriteLine("2) Add a movie.");
            Console.WriteLine("3) Update a movie.");
            Console.WriteLine("4) Delete a movie.");
            Console.WriteLine("Press any other key to exit...");
            var menuOption = Console.ReadLine();
            
            //Creating the various options that the user can choose.
            if (menuOption == "1")
            {
                // Searching for a movie.
                Console.WriteLine("Search for a Movie!");

                using (var db = new MovieContext())
                {
                    Console.WriteLine("What type of search would you like to preform?");
                    Console.WriteLine("1) Specific title / Single search");
                    Console.WriteLine("2) Like search, this grabs 10 movies with the search word in the name.");
                    Console.WriteLine("");
                    var searchType = Console.ReadLine();

                    if (searchType == "1")
                    {
                        Console.WriteLine("You have picked: Single or Specific Title search:");

                        // oops misspelled movies...
                        Console.WriteLine("Please enter a search word for the movie title you are looking for");
                        var searchWord = Console.ReadLine();

                        //Make sure the input is not null or empty before going on.
                        while (searchWord.IsNullOrEmpty())
                        {
                            Console.WriteLine("Cannot be empty, please enter a word for the title.");
                            searchWord = Console.ReadLine();

                        }



                        var moveies = db.Movies
                            .FirstOrDefault(mov => mov.Title.Contains(searchWord));

                        Console.WriteLine($"Movie: {moveies.Title} {moveies.ReleaseDate: MM-dd-yyy}");
                        Console.WriteLine("Genres: ");
                        foreach (var genre in moveies.MovieGenres ?? new List<MovieGenre>())
                        {
                            Console.WriteLine($"\t{genre.Genre.Name}");
                        }

                    }
                    else if (searchType == "2")
                    {
                        /*
                        Console.WriteLine("Please enter a search word for the movie titles you are looking for");
                        var searchWord = Console.ReadLine();

                        //Make sure the input is not null or empty before going on.
                        while (searchWord.IsNullOrEmpty())
                        {
                            Console.WriteLine("Cannot be empty, please enter a word for the titles.");
                            searchWord = Console.ReadLine();

                        }
                        */

                        Console.WriteLine("I could not figure this out, so here are ten movies: ");

                        var moveies = db.Movies
                            .Include(x => x.MovieGenres)
                            .ThenInclude(x => x.Genre);

                        
                        var limitedMovies = moveies.Take(10);

                        Console.WriteLine("The Movies are: ");
                        foreach (var mov in moveies.Take(10).ToList())
                        {

                             Console.WriteLine($"Movie Title: {mov.Title} {mov.ReleaseDate:MM-dd-yyyy}  ");
                             Console.WriteLine($"Genres: ");
                             foreach (var genre in mov.MovieGenres ?? new List<MovieGenre>())
                             {
                                Console.WriteLine($"\t{genre.Genre.Name}");
                             }


                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("You found an easter egg!");
                        Console.WriteLine("I didn't add anything to make sure you only chose one of those two choices.");
                    }
                   
                    
                    


                    /*
                    //Use something like this for exact movie match? 
                    //var mov = db.Movies.FirstOrDefault(x => x.Title == movieUpdate);
                    var limitedMovies = moveies.Take(10);

                    Console.WriteLine("The Movies are: ");
                    foreach (var mov in moveies.Take(10).ToList())
                    {
                        Console.WriteLine($"Movie Title: {mov.Title}  ");
                        
                    }
                    */

                }

            }
            else if (menuOption == "2")
            {
                //Adding a new movie to the database. 

                Console.WriteLine("Adding a Movie to the database.");

                Console.WriteLine("Enter a movie Title");
                var movie = Console.ReadLine();
                Console.WriteLine("Enter a movie Release Date");
                Console.WriteLine(" MM / DD / YYYY - Use this format");

                var releaseDate = DateTime.Parse(Console.ReadLine());

                using (var db = new MovieContext())
                {
                    var mov = new Movie();
                    mov.Title = movie;
                    mov.ReleaseDate = releaseDate;

                    //Add in Genres here for the final

                    db.Movies.Add(mov);
                    db.SaveChanges();
                    Console.WriteLine($"Created {mov.Id} {mov.Title} {mov.ReleaseDate}!");
                }

            }
            else if (menuOption == "3")
            {
                // Updating the movie
                Console.WriteLine("Updating a movie title in the database.");

                Console.WriteLine("Enter the title of the movie you wish to update.");
                var movieToUpdate = Console.ReadLine();

                Console.WriteLine("Enter the updated name of the movie.");
                var movieUpdated = Console.ReadLine();

                // To expand on this, Add release date functionality to the update.

                using (var db = new MovieContext())
                {
                    var updateMovie = db.Movies.FirstOrDefault(x => x.Title == movieToUpdate);
                    Console.WriteLine($"({updateMovie.Id}) {updateMovie.Title}");
                    Console.WriteLine($"Changed to:({updateMovie.Id}) {movieUpdated}");

                    updateMovie.Title = movieUpdated;

                    db.Movies.Update(updateMovie);
                    db.SaveChanges();
                }

            }
            else if (menuOption == "4")
            {
                Console.WriteLine("Deleting a movie entry in the database.");

                Console.WriteLine("WARNING: THIS CANNOT BE UNDONE DO YOU WISH TO CONTINUE? (Y/n) - This is case sensitive.");
                var confirmDelete = Console.ReadLine();
                if (confirmDelete == "Y")
                {
                    Console.WriteLine("Continuing with deletion process, proceed with caution.");
                    Console.WriteLine("");
                    Console.WriteLine("Please enter the title of the movie you wish to delete.");
                    var movieToDelete = Console.ReadLine();

                    using (var db = new MovieContext())
                    {
                        var deleteMovie = db.Movies.FirstOrDefault(x => x.Title == movieToDelete);
                        Console.WriteLine($"({deleteMovie.Id}) {deleteMovie.Title}  : Has been permanantly removed.");


                        db.Movies.Remove(deleteMovie);
                        db.SaveChanges();
                    }

                }
                else if (confirmDelete != "Y")
                {
                    Console.WriteLine("You have stopped the deletion process. Thank you for your caution.");
                }


                


            }
            else if (menuOption != "1" && menuOption != "2" && menuOption != "3" && menuOption != "4")
            {
                // Could honestly just do an else instead...
                Console.WriteLine("Exiting Program...");
            }


        }
    }
}