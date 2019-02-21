using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MovieManager.Models;

namespace MovieManager.ViewModels
{
  public class MovieViewModel : BaseViewModel
  {

    #region Private Fields
    private MovieModel Movie;
    #endregion

    #region Constructors
    public MovieViewModel(MovieModel movie)
    {
      // MovieModel data passed from MainWindowViewModel
      Movie = movie;

      // Load values from Movie model into properties
      LoadValues();

     

    }


    public MovieViewModel()
    {
      PubSub<object>.Subscribe("PubSubTest", ToggleButtonCheckedHandler);
    }

    private void ToggleButtonCheckedHandler(object sender, PubSubEventArgs<object> args)
    {
      MessageBox.Show("PubSub event item received: " + (string)args.Item);
    }
    #endregion

    #region Properties

    private string _title;
    public string Title
    {
      get { return _title; }
      set
      {
        if(_title != value)
        {
          _title = value;
          OnPropertyChanged(nameof(Title));
        }
      }
    }

    private string _genre;
    public string Genre
    {
      get { return _genre; }
      set
      {
        if (_genre != value)
        {
          _genre = value;
          OnPropertyChanged(nameof(Genre));
        }
      }
    }

    private int _duration;
    public int Duration
    {
      get { return _duration; }
      set
      {
        if (_duration != value)
        {
          _duration = value;
          OnPropertyChanged(nameof(Duration));
        }
      }
    }

    private int _releaseYear;
    public int ReleaseYear
    {
      get { return _releaseYear; }
      set
      {
        if (_releaseYear != value)
        {
          _releaseYear = value;
          OnPropertyChanged(nameof(ReleaseYear));
        }
      }
    }

    private bool _isMovieSeen;
    public bool IsMovieSeen
    {
      get { return _isMovieSeen; }
      set
      {
        if (_isMovieSeen != value)
        {
          _isMovieSeen = value;
          OnPropertyChanged(nameof(IsMovieSeen));
        }
      }
    }

    #endregion

    #region Methods

    public void LoadValues()
    {
      Title = Movie.Title;
      Genre = Movie.Genre;
      Duration = Movie.Duration;
      ReleaseYear = Movie.ReleaseYear;
      IsMovieSeen = Movie.IsMovieSeen;
    }

    public MovieModel SaveValues()
    {
      // When saving new data, create a new Movie model containting that data and save the new object
      return new MovieModel { Title = Title, Genre = Genre, Duration = Duration, ReleaseYear = ReleaseYear, IsMovieSeen = IsMovieSeen };
    }

    #endregion

  }
}
