using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Models;

namespace MovieManager.ViewModels
{
  public class MoviesViewModel
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

        MoviesCollection.Add(new MovieViewModel(movie));

        //CustomerViewModel cvm = new CustomerViewModel(customer, this);
        //// Attach PropertyChanged event to each object in the collection
        //cvm.PropertyChanged += Cvm_PropertyChanged;
        //Customers.Add(cvm);

      }
    }
    #endregion


  }
}
