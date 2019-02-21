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
  public class MoviesViewModel : BaseViewModel
  {

    #region Private Fields
    private List<MovieModel> movies;
    #endregion

    #region Properties
    public ObservableCollection<MovieViewModel> MoviesCollection { get; set; }
    #endregion


    #region Constructors
    public MoviesViewModel(List<MovieModel> movies)
    {
      this.movies = movies;

      // Collection to hold MovieViewModel objects
      MoviesCollection = new ObservableCollection<MovieViewModel>();

      // Fill collection with objects of type MovieViewModel
      LoadValues(movies);
    }
    #endregion


    #region Methods
    public List<MovieModel> SaveValues()
    {

      List<MovieModel> result = new List<MovieModel>();

      foreach (MovieViewModel movie in MoviesCollection)
      {
        // Overrides existing content in the list
        result.Add(movie.SaveValues());
      }

      return result;
    }

    public void LoadValues(List<MovieModel> movies)
    {
      MoviesCollection.Clear();

      foreach (MovieModel movie in movies)
      {

        // For each movie object add a PropertyChanged event to it
        MovieViewModel mvm = new MovieViewModel(movie);
        mvm.PropertyChanged += Mvm_PropertyChanged;
        MoviesCollection.Add(mvm);


        //MoviesCollection.Add(new MovieViewModel(movie));

        

      }
    }

    private void Mvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      // Call OnPropertyChanged each time a change is detected in a movie object, which invokes the ViewModels OnPropertyChanged event defined in MainViewModel
      OnPropertyChanged(nameof(Mvm_PropertyChanged));
    }
    #endregion


  }
}
