using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using MovieManager.Command.MovieManager.Command;
using MovieManager.Helpers;
using MovieManager.Models;
using MovieManager.Properties;
using MovieManager.ViewModels;

namespace MovieManager.ViewModel
{
  public class MainViewModel : BaseViewModel
  {


    

    #region Fields
    private const string DBPath = @"c:\tmp\movie_db.txt";
    #endregion

    #region Constructors

    private ReadingEntity readingModel;
    public MainViewModel(ReadingEntity readingModel)
    {

      // Publish an event with eventName = PubSubTest. Others can subscribe to said eventName, in order to catch when it is raised
      PubSub<object>.PublishEvent("PubSubTest", PubSubCheckedHandler);

      // ReadingModel contains a list of movie objects data fetched from the database
      this.readingModel = readingModel;


      // Initialize other viewmodels
      WatchedMoviesViewModel = new WatchedMoviesViewModel(readingModel.WatchedMovies); // Pass list of movie objects to the WatchedMovies
      NonWatchedMoviesViewModel = new NonWatchedMoviesViewModel(readingModel.NonWatchedMovies); // Pass list of movie objects to the NonWatchedMovies
      AddMovieViewModel = new AddMovieViewModel(this, WatchedMoviesViewModel, NonWatchedMoviesViewModel); // Pass reference for mainviewmodel and both datagrids, so when we add a new movie we know where to put it

      // Subscribe to the ViewModels' OnpropertyChanged event
      WatchedMoviesViewModel.PropertyChanged += MoviesViewModel_PropertyChanged;
      NonWatchedMoviesViewModel.PropertyChanged += MoviesViewModel_PropertyChanged;
      this.PropertyChanged += MainWindowViewModel_PropertyChanged;    // Subscribe to MainViewModels OnPropertyChanged event to check for changes in MainViewModel



      // Register commands so we are able to execute specific buttons
      RegisterCommand(SaveCommand = new ActionCommand(Save, CanSave));
      RegisterCommand(LoadCommand = new ActionCommand(Load, CanLoad));
      RegisterCommand(TestCommand = new ActionCommand(Test, CanTest));


    }


    #endregion

    #region Events
    public event PubSubEventHandler<object> PubSubCheckedHandler;
    #endregion

    #region Properties

    public NonWatchedMoviesViewModel NonWatchedMoviesViewModel { get; set; }
    public WatchedMoviesViewModel WatchedMoviesViewModel { get; set; }
    public AddMovieViewModel AddMovieViewModel { get; set; }

   
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

    /* If any changes have been made to any of the MainViewModels UI controls */
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

    #region Commands

    public ActionCommand LoadCommand { get; set; }
    public ActionCommand SaveCommand { get; set; }
    public ActionCommand TestCommand { get; set; }

    #endregion

    #region PropertyChanged Events

    // This event gets called whenever a change in any of MainViewModels properties (those who have UpdateSourceTrigger attached) is detected
    private void MainWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

      //MessageBox.Show(e.PropertyName);
      // Only raise event if OnPropertyChanged event of PubSubTestChecked is called
      if(e.PropertyName == nameof(PubSubTestChecked))
      {
        //When PubSubTestChanged's value is changed to either true or false, raise event
        if (PubSubTestChecked)
          PubSub<object>.RaiseEvent("PubSubTest", this, new PubSubEventArgs<object>("Red"));
        else
          PubSub<object>.RaiseEvent("PubSubTest", this, new PubSubEventArgs<object>("Blue"));
      }
     

    }

    // This event gets called whenever a change in any of MovieModel's properties is detected
    private void MoviesViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      // When a change in either datagrid is detected, change value to true
      ChangesDetected = true;
    }

    #endregion

    #region Bool Return-Methods
    private bool CanExpandView()
    {
      // One can always click the expand addmovieview button
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
      ReadingEntity readingModel = new ReadingEntity(DBPath);
      // Update values
      UpdateValues(readingModel);

      if(ChangesDetected)
        MessageBox.Show("Values loaded:"); // Only show dialog if actual changes has been made in the grid

      CheckCanLoad = false;
    }


    public void UpdateValues(ReadingEntity readingModel)
    {
      // Update values by putting loaded values from DB into movies collection, which then repopulates the datagrid since the collection changed
      NonWatchedMoviesViewModel.LoadValues(readingModel.NonWatchedMovies);
      WatchedMoviesViewModel.LoadValues(readingModel.WatchedMovies);
      // Deactivate Load button after values updated
      ChangesDetected = false;
    }


    private void Test()
    {
      // Change value of PubSubTestChecked, which causes the onpropertychanged event to fire for MainViewModel
      //PubSubTestChecked = !PubSubTestChecked ? true : false;
      //MessageBox.Show(new StackFrame().GetMethod().Name);
      string path_old = @"C:\users\silas\desktop\template.html";
      string path_new = @"C:\users\silas\desktop\template1.html";

      string template = Resources.AlerisEmailTemplate;


      template = template.Replace("EmployeeNameAttribute", "Silas Stryhn");

      File.WriteAllText(path_new, template);
 
    }


    // Method for handling save button logic
    private void Save()
    {
      
      //* Xml Serilizer to write data to an existing txt file */
      XmlSerializer x = new XmlSerializer(typeof(ReadingEntity));
      if (!string.IsNullOrWhiteSpace(DBPath) && File.Exists(DBPath))
      {
        using (TextWriter tw = new StreamWriter(DBPath))
        {
          // Update reading model object with new values

          // Temp list to hold sorted objects according to their IsMovieSeen value
          var tempWatched = new List<MovieModel>();
          var tempNonWatched = new List<MovieModel>();

          // Sort Nonwatched movies and move objects to correct list
          foreach(var movie in NonWatchedMoviesViewModel.SaveValues())
          {
            if (movie.IsMovieSeen)
              tempWatched.Add(movie);
            else
              tempNonWatched.Add(movie);
          }

          // Sort Watched movies and move objects to correct list
          foreach (var movie in WatchedMoviesViewModel.SaveValues())
          {
            if (movie.IsMovieSeen)
              tempWatched.Add(movie);
            else
              tempNonWatched.Add(movie);
          }

          // Update reading model object and save it to the db file
          readingModel.NonWatchedMovies = tempNonWatched;
          readingModel.WatchedMovies = tempWatched;

          x.Serialize(tw, readingModel);
        }
        MessageBox.Show("Values saved");
      }

      // Reloades the just saved values from the db. 
      // TODO: Delete the affected values from their respective Observable collection, instead of overriding with values from db
      Load();



      // Collaps add movie view if visible
      //if (AddMovieViewVisibility)
      //  ExpandOrCollapsAddMovieView();

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
