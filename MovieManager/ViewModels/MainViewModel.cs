using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using MovieManager.Command.MovieManager.Command;
using MovieManager.Helpers;
using MovieManager.Models;
using MovieManager.ViewModels;

namespace MovieManager.ViewModel
{
  public class MainViewModel : BaseViewModel
  {

    #region Fields
    private const string DBPath = @"c:\tmp\movie_db.txt";
    #endregion

    #region Constructors

    private ReadingModel readingModel;
    public MainViewModel(ReadingModel readingModel)
    {


      // Publish an event with eventName = PubSubTest. Others can subscribe to said eventName, in order to catch when it is raised
      PubSub<object>.PublishEvent("PubSubTest", PubSubCheckedHandler);

      // ReadingModel contains a list (readingModel.Movies) of movie objects data fetched from the database
      this.readingModel = readingModel;


      WatchedMoviesViewModel = new WatchedMoviesViewModel(readingModel.WatchedMovies);
      WatchedMoviesViewModel.PropertyChanged += MoviesViewModel_PropertyChanged;

      // Pass list of movie objects to the MoviesViewModel
      MoviesViewModel = new NonWatchedMoviesViewModel(readingModel.NonWatchedMovies);
      // Add an event to check for changes in the viewmodel or if the ViewModels OnpropertyChanged event is called
      MoviesViewModel.PropertyChanged += MoviesViewModel_PropertyChanged;
      // Add event to check for changes in MainViewModel
      this.PropertyChanged += MainWindowViewModel_PropertyChanged;

      // Register commands so we are able to execute specific buttons
      RegisterCommand(SaveCommand = new ActionCommand(Save, CanSave));
      RegisterCommand(LoadCommand = new ActionCommand(Load, CanLoad));
      RegisterCommand(AddMovieCommand = new ActionCommand(AddMovie, CanAddMovie));
      RegisterCommand(ExpandAddMovieViewCommand = new ActionCommand(ExpandOrCollapsAddMovieView, CanExpandView));
      RegisterCommand(TestCommand = new ActionCommand(Test, CanTest));


    }


    #endregion

    #region Events
    public event PubSubEventHandler<object> PubSubCheckedHandler;
    #endregion

    #region Properties

    public NonWatchedMoviesViewModel MoviesViewModel { get; set; }
    public WatchedMoviesViewModel WatchedMoviesViewModel { get; set; }

    private bool _pubSubTestChecked;
    public bool PubSubTestChecked
    {
      get
      {
        return _pubSubTestChecked;
      }
      set
      {
        if (_pubSubTestChecked != value)
        {
          _pubSubTestChecked = value;
          OnPropertyChanged(nameof(PubSubTestChecked));
          
        }
      }
    }

    // Controls whether Load button is enabled or not
    private bool _checkCanLoad;
    public bool CheckCanLoad
    {
      get { return _checkCanLoad; }
      set
      {
        if (value != _checkCanLoad)
        {
          _checkCanLoad = value;
          OnPropertyChanged(nameof(CheckCanLoad));
        }
      }
    }

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



    private string _addTitle;
    public string AddTitle
    {
      get { return _addTitle; }
      set
      {
        if(_addTitle != value)
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
        if(_addIsSeen != value)
        {
          _addIsSeen = value;
          OnPropertyChanged(nameof(AddIsSeen));
        }
      }
    }


    /* If any changes have been made to any of the UI fields */
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


    private bool _addMovieViewVisibility;
    public bool AddMovieViewVisibility
    {
      get { return _addMovieViewVisibility; }
      set
      {
        if(_addMovieViewVisibility != value)
        {
          _addMovieViewVisibility = value;
          OnPropertyChanged(nameof(AddMovieViewVisibility));
        }
      }
    }

    #endregion

    #region Commands

    public ActionCommand LoadCommand { get; set; }
    public ActionCommand SaveCommand { get; set; }
    public ActionCommand AddMovieCommand { get; set; }
    public ActionCommand ExpandAddMovieViewCommand { get; set; }
    public ActionCommand TestCommand { get; set; }


    #endregion

    #region PropertyChanged Events

    // This event gets called whenever a change in any of MainViewModels properties (those who have UpdateSourceTrigger attached) is detected
    private void MainWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      // All fields except IsSeen should be edited before one can add a movie
      CheckCanAddMovie = (AddTitle != null && AddGenre != null && AddDuration >= 1 && (AddReleaseYear >= 1895 && AddReleaseYear <= 2100)) ? true : false;
 
    }

    // This event gets called whenever a change in any of MovieModel's properties is detected
    private void MoviesViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      ChangesDetected = true;
    }

    #endregion

    #region Bool Return-Methods
    private bool CanExpandView()
    {
      return true;
    }

    private bool CanAddMovie()
    {
      // You can only add a new movie if the fields are properly filled
      return CheckCanAddMovie;
    }

    // Method for determining whether save button can be clicked or not
    private bool CanSave()
    {
      // Save button is only enabled if some changes have been noticed
      return ChangesDetected;
    }

    private bool CanTest()
    {
      // Can always check
      return true;
    }

   
    private bool CanLoad()
    {
      // Method for determining whether load button can be clicked or not
      return CheckCanLoad;
    }


    #endregion

    #region Methods


    private void Load()
    {
      // Load new values from db into new reading object
      ReadingModel readingModel = new ReadingModel(DBPath);
      // Update values
      UpdateValues(readingModel);

      if(ChangesDetected)
        MessageBox.Show("Values loaded:"); // Only show dialog if actual changes has been made in the grid

      CheckCanLoad = false;
    }


    public void UpdateValues(ReadingModel readingModel)
    {
      // Update values by putting loaded values from DB into movies collection, which then repopulates the datagrid since the collection changed
      MoviesViewModel.LoadValues(readingModel.NonWatchedMovies);
      WatchedMoviesViewModel.LoadValues(readingModel.WatchedMovies);
      // Deactivate Load button after values updated
      ChangesDetected = false;
    }


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



    private void Test()
    {

      // Change value of PubSubTestChecked raise published event
      PubSubTestChecked = !PubSubTestChecked ? true : false;

      //When PubSubTestChanged's value is changed to either true or false, raise event
      if (PubSubTestChecked)
        PubSub<object>.RaiseEvent("PubSubTest", this, new PubSubEventArgs<object>("Red"));
      else
        PubSub<object>.RaiseEvent("PubSubTest", this, new PubSubEventArgs<object>("Blue"));

    }



    private void AddMovie()
    {

      // Add new entry to collection
      if(AddIsSeen)
        WatchedMoviesViewModel.AddNewMovieToCollection(AddTitle, AddGenre, AddDuration, AddReleaseYear, AddIsSeen);
      else
        MoviesViewModel.AddNewMovieToCollection(AddTitle, AddGenre, AddDuration, AddReleaseYear, AddIsSeen);
      

      // Clear fields 
      AddTitle = ""; AddGenre = ""; AddDuration = 0; AddReleaseYear = 0;
      // If Movie is seen, uncheck 
      if (AddIsSeen) AddIsSeen = false;
      // Enable save button
      ChangesDetected = true;

    }


    // Method for handling save button logic
    private void Save()
    {
      
      //* Xml Serilizer to write data to an existing txt file */
      XmlSerializer x = new XmlSerializer(typeof(ReadingModel));
      if (!string.IsNullOrWhiteSpace(DBPath) && File.Exists(DBPath))
      {
        using (TextWriter tw = new StreamWriter(DBPath))
        {
          // Update reading model object with new values
          readingModel.NonWatchedMovies = MoviesViewModel.SaveValues();
          readingModel.WatchedMovies = WatchedMoviesViewModel.SaveValues();

          x.Serialize(tw, readingModel);
        }
        MessageBox.Show("Values saved");
      }

      // Reloades the just saved values from the db. 
      // TODO: Delete the affected values from their respective Observable collection, instead of overriding with values from db
      Load();



      // Collaps add movie view if visible
      if (AddMovieViewVisibility)
        ExpandOrCollapsAddMovieView();

      // Deactivate Load button. 
      // Because changes are saved to the db file, the filewatcher notices the change and activates the load button.
      // Consider making some kind of validation to check for same content in db file as that in the datagrid
      CheckCanLoad = false;
      // Deactive Save button when values are saved
      ChangesDetected = false;
    }

    #endregion

  }
}
