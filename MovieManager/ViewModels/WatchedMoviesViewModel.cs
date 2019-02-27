using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MovieManager.Models;

namespace MovieManager.ViewModels
{
  public class WatchedMoviesViewModel : BaseViewModel
  {

    #region Private Fields
    private List<MovieModel> watchedMovies;
    #endregion

    #region Properties
    public ObservableCollection<MovieViewModel> WatchedMoviesCollection { get; set; }
    #endregion


    #region Constructors
    public WatchedMoviesViewModel(List<MovieModel> watchedMovies)
    {
      this.watchedMovies = watchedMovies;

      // Collection to hold MovieViewModel objects
      WatchedMoviesCollection = new ObservableCollection<MovieViewModel>();

      // Fill collection with objects of type MovieViewModel
      LoadValues(watchedMovies);
    }

    #endregion


    #region Methods
    public void LoadValues(List<MovieModel> watchedMovies)
    {

      WatchedMoviesCollection.Clear();

      foreach (MovieModel watchedMovie in watchedMovies)
      {
        // For each movie object add a PropertyChanged event to it
        MovieViewModel mvm = new MovieViewModel(watchedMovie);
        mvm.PropertyChanged += Mvm_PropertyChanged;
        WatchedMoviesCollection.Add(mvm);

      }
    }


    public void AddNewMovieToCollection(string title, string genre, int duration, int releaseYear, bool seen)
    {
      MovieModel movie = new MovieModel { Title = title, Genre = genre, Duration = duration, ReleaseYear = releaseYear, IsMovieSeen = seen };

      MovieViewModel mvm = new MovieViewModel(movie);
      mvm.PropertyChanged += Mvm_PropertyChanged;

      WatchedMoviesCollection.Add(mvm);

    }

    public List<MovieModel> SaveValues()
    {

      List<MovieModel> result = new List<MovieModel>();

      foreach (MovieViewModel movie in WatchedMoviesCollection)
      {
        // Overrides existing content in the list
        //if(movie.IsMovieSeen)
          result.Add(movie.SaveValues());
      }

      return result;
    }
    private void Mvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      OnPropertyChanged(nameof(Mvm_PropertyChanged));
    }


    #endregion

  }

}