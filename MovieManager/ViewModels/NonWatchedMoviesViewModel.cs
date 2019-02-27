using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MovieManager.Models;

namespace MovieManager.ViewModels
{
  public class NonWatchedMoviesViewModel : BaseViewModel
  {

    #region Private Fields
    private List<MovieModel> nonWatchedMovies;
    #endregion

    #region Properties
    public ObservableCollection<MovieViewModel> NonWatchedMoviesCollection { get; set; }
    #endregion


    #region Constructors
    public NonWatchedMoviesViewModel(List<MovieModel> nonWatchedMovies)
    {
      this.nonWatchedMovies = nonWatchedMovies;

      // Collection to hold MovieViewModel objects
      NonWatchedMoviesCollection = new ObservableCollection<MovieViewModel>();

      // Fill collection with objects of type MovieViewModel
      LoadValues(nonWatchedMovies);
    }
    #endregion


    #region Methods
    public List<MovieModel> SaveValues()
    {

      List<MovieModel> result = new List<MovieModel>();

      foreach (MovieViewModel movie in NonWatchedMoviesCollection)
      {
        // Overrides existing content in the list
        //if (!movie.IsMovieSeen)
          result.Add(movie.SaveValues());
      }


      return result;
    }

    public void LoadValues(List<MovieModel> nonWatchedMovies)
    {

      // Run constructor containing pub/sub event
      new MovieViewModel();

      NonWatchedMoviesCollection.Clear();

      foreach (MovieModel movie in nonWatchedMovies)
      {
        // For each movie object add a PropertyChanged event to it
        MovieViewModel mvm = new MovieViewModel(movie);
        mvm.PropertyChanged += Mvm_PropertyChanged;
        NonWatchedMoviesCollection.Add(mvm);

        //MoviesCollection.Add(new MovieViewModel(movie));
      }


    }

    public void AddNewMovieToCollection(string title, string genre, int duration, int releaseYear, bool seen)
    {
      MovieModel movie = new MovieModel { Title = title, Genre = genre, Duration = duration, ReleaseYear = releaseYear, IsMovieSeen = seen };

      MovieViewModel mvm = new MovieViewModel(movie);
      mvm.PropertyChanged += Mvm_PropertyChanged;

      NonWatchedMoviesCollection.Add(mvm);
      
    }

    private void Mvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      // Call OnPropertyChanged each time a change is detected in a movie object, which invokes the ViewModels OnPropertyChanged event defined in MainViewModel
      OnPropertyChanged(nameof(Mvm_PropertyChanged));
    }
    #endregion


  }
}
