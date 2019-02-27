using System;
using MovieManager.Command.MovieManager.Command;
using MovieManager.ViewModel;

namespace MovieManager.ViewModels
{
  public class AddMovieViewModel : BaseViewModel
  {

    WatchedMoviesViewModel wmvm;
    NonWatchedMoviesViewModel nwvm;
    MainViewModel mvm;

    #region Constructors
    public AddMovieViewModel(MainViewModel mvm, WatchedMoviesViewModel wmvm, NonWatchedMoviesViewModel nwvm)
    {
      this.mvm = mvm;
      this.wmvm = wmvm;
      this.nwvm = nwvm;

      // Register OnPropertyChanged event for this viewmodel
      this.PropertyChanged += AddMovieViewModel_PropertyChanged;

      // Register commands so we are able to execute specific buttons
      RegisterCommand(AddMovieCommand = new ActionCommand(AddMovie, CanAddMovie));
      RegisterCommand(ExpandAddMovieViewCommand = new ActionCommand(ExpandOrCollapsAddMovieView, CanExpandView));
    }

    #region PropertyChanged Events
    private void AddMovieViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      CheckCanAddMovie = (AddTitle != null && AddGenre != null && AddDuration >= 1 && (AddReleaseYear >= 1895 && AddReleaseYear <= 2100)) ? true : false;
    }
    #endregion


    #endregion

    #region Commands
    public ActionCommand AddMovieCommand { get; set; }
    public ActionCommand ExpandAddMovieViewCommand { get; set; }
    #endregion

    #region Properties
    // Controls whether Add button is enabled or not
    private bool _checkCanAddMovie;
    public bool CheckCanAddMovie
    {
      get { return _checkCanAddMovie; }
      set
      {
        if (value != _checkCanAddMovie)
        {
          _checkCanAddMovie = value;
          OnPropertyChanged(nameof(CheckCanAddMovie));
        }
      }
    }



    private bool _addMovieViewVisibility;
    public bool AddMovieViewVisibility
    {
      get { return _addMovieViewVisibility; }
      set
      {
        if (_addMovieViewVisibility != value)
        {
          _addMovieViewVisibility = value;
          OnPropertyChanged(nameof(AddMovieViewVisibility));
        }
      }
    }


    private string _addTitle;
    public string AddTitle
    {
      get { return _addTitle; }
      set
      {
        if (_addTitle != value)
        {
          _addTitle = value;
          OnPropertyChanged(nameof(AddTitle));
        }
      }
    }

    private string _addGenre;
    public string AddGenre
    {
      get { return _addGenre; }
      set
      {
        if (_addGenre != value)
        {
          _addGenre = value;
          OnPropertyChanged(nameof(AddGenre));
        }
      }
    }

    private int _addDuration;
    public int AddDuration
    {
      get
      {
        return _addDuration;
      }
      set
      {
        if (_addDuration != value)
        {
          _addDuration = value;
          OnPropertyChanged(nameof(AddDuration));
        }
      }
    }

    private int _addReleaseYear;
    public int AddReleaseYear
    {
      get
      {
        return _addReleaseYear;
      }
      set
      {
        if (_addReleaseYear != value)
        {
          _addReleaseYear = value;
          OnPropertyChanged(nameof(AddReleaseYear));
        }
      }
    }
    private bool _addIsSeen;
    public bool AddIsSeen
    {
      get { return _addIsSeen; }
      set
      {
        if (_addIsSeen != value)
        {
          _addIsSeen = value;
          OnPropertyChanged(nameof(AddIsSeen));
        }
      }
    }

    /* If any changes have been made to any of the AddMovie UI fields */
    private bool _changesDetected;
    public bool ChangesDetected
    {
      get { return _changesDetected; }
      set
      {
        if (value != _changesDetected)
        {
          _changesDetected = value;
          OnPropertyChanged(nameof(ChangesDetected));
        }
      }
    }



    #endregion

    #region Bool return methods
    private bool CanAddMovie()
    {
      return CheckCanAddMovie;
    }

    private bool CanExpandView()
    {
      return true;
    }


    #endregion


    #region Methods
    private void ExpandOrCollapsAddMovieView()
    {

      if (!AddMovieViewVisibility)
      {
        AddMovieViewVisibility = true;
      }
      else
      {
        AddMovieViewVisibility = false;

      }
      //AddMovieViewVisibility = !AddMovieViewVisibility ? true : false;
    }


    private void AddMovie()
    {

      // Add new entry to collection
      if (AddIsSeen)
        wmvm.AddNewMovieToCollection(AddTitle, AddGenre, AddDuration, AddReleaseYear, AddIsSeen);
      else
        nwvm.AddNewMovieToCollection(AddTitle, AddGenre, AddDuration, AddReleaseYear, AddIsSeen);


      // Clear fields 
      AddTitle = ""; AddGenre = ""; AddDuration = 0; AddReleaseYear = 0;
      // If Movie is seen, uncheck 
      if (AddIsSeen) AddIsSeen = false;
      // Enable save button which lies in the datacontext of MainViewModel
      mvm.ChangesDetected = true;

    }
    #endregion


  }
}
