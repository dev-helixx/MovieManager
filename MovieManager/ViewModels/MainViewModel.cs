using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using MovieManager.Command.MovieManager.Command;
using MovieManager.Models;
using MovieManager.ViewModels;

namespace MovieManager.ViewModel
{
  public class MainWindowViewModel : BaseViewModel
  {


    #region Properties

    public MoviesViewModel MoviesViewModel { get; set; }

    public ActionCommand LoadCommand { get; set; }
    public ActionCommand SaveCommand { get; set; }



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

    #endregion



    private ReadingModel readingModel;
    public MainWindowViewModel(ReadingModel readingModel)
    {
      // ReadingModel contains a list (readingModel.Movies) of movie objects data fetched from the database
      this.readingModel = readingModel;

      // Pass list of movie objects to the MoviesViewModel
      MoviesViewModel = new MoviesViewModel(readingModel.Movies);
      // Add an even to check for changes in the viewmodel or if the ViewModels OnpropertyChanged event is called
      MoviesViewModel.PropertyChanged += MoviesViewModel_PropertyChanged;


      // Register commands so we are able to execute specific buttons
      RegisterCommand(SaveCommand = new ActionCommand(Save, CanSave));



    }

    private void MoviesViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      ChangesDetected = true;
    }

    // Method for determining whether save button can be clicked or not
    private bool CanSave()
    {
      return ChangesDetected;
    }

    // Method for handling save button logic
    private void Save()
    {

      string DBPath = @"c:\tmp\movie_db.txt";
      //* Xml Serilizer to write data to an existing txt file */
      XmlSerializer x = new XmlSerializer(typeof(ReadingModel));
      if (!string.IsNullOrWhiteSpace(DBPath) && File.Exists(DBPath))
      {
        using (TextWriter tw = new StreamWriter(DBPath))
        {
          // Update main model object with new values from textboxes
          readingModel.Movies = MoviesViewModel.SaveValues();

          x.Serialize(tw, readingModel);
        }
        MessageBox.Show("Values saved");
      }

      // Deactive Save button when values are saved
      ChangesDetected = false;

    }
  }
}
