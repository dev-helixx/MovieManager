using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace MovieManager.Models
{
  public class ReadingModel
  {

    #region Properties
    public List<MovieModel> Movies { get; set; } // Name of list HAS to match the node in the XML file!

    public List<MovieModel> WatchedMovies { get; set; }
    public List<MovieModel> NonWatchedMovies { get; set; }
    #endregion

    #region Constructors
    public ReadingModel()
    {
      Movies = new List<MovieModel>();
      WatchedMovies = new List<MovieModel>();
      NonWatchedMovies = new List<MovieModel>();
    }

    public ReadingModel(string DBPath)
    {

      // Fetch data from DB file
      ReadingModel readingModel = new ReadingModel();
      XmlSerializer x = new XmlSerializer(typeof(ReadingModel));
      using (TextReader tr = new StreamReader(DBPath))
      {
        // readingModel now contains all movie objects from the db file
        readingModel = (ReadingModel)x.Deserialize(tr);
      }

      // Fill Movies list with movies fetched from db
      Movies = readingModel.Movies;


      var tempWatched = new List<MovieModel>();
      var tempNonWatched = new List<MovieModel>();


      // Fill Lists with movies depending on if the movie is seen or not
      foreach (var movie in readingModel.Movies)
      {
        if (movie.IsMovieSeen)
          tempWatched.Add(movie);
        else
          tempNonWatched.Add(movie);

      }


      WatchedMovies = tempWatched;
      NonWatchedMovies = tempNonWatched;

    }

    

    #endregion
  }
}
