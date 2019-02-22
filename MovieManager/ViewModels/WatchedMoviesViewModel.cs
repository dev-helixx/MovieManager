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
  public class WatchedMoviesViewModel
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

    public void LoadValues(List<MovieModel> watchedMovies)
    {

      WatchedMoviesCollection.Clear();

      foreach (MovieModel watchedMovie in watchedMovies)
      {
        // For each movie object add a PropertyChanged event to it
        MovieViewModel mvm = new MovieViewModel(watchedMovie);
        mvm.PropertyChanged += Mvm_PropertyChanged;
        WatchedMoviesCollection.Add(mvm);

        //MoviesCollection.Add(new MovieViewModel(movie));
      }

      #endregion

    }

    private void Mvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      MessageBox.Show("Watched Movies property changed");
    }
  }
}