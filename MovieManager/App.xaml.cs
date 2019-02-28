using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using MovieManager.FilesystemHandler;
using MovieManager.Helpers;
using MovieManager.Models;
using MovieManager.ViewModel;
using MovieManager.ViewModels;

namespace MovieManager
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {

    // Database file path
    private const string DBPath = @"c:\tmp\movie_db.txt";


    protected override void OnStartup(StartupEventArgs e)
    {

     
      base.OnStartup(e);

      // If db file does not yet exist, create one with dummy data so the application is able to start
      InitDatabaseIfNotExist();

      //ReadingModel readingModel = new ReadingModel(DBPath); // Reading Model ( Reads data from db file and saves it in a list of movie objects)

      //var mainVM = new MainViewModel(readingModel); // Pass model to MainViewModel

      //// Initialize filewatcher to watch for changes in the DB file
      //new Filewatcher(mainVM).Init();


      //var mainWindow = new MainWindow
      //{
      //  DataContext = MainWindowViewModel // Set datacontext to main ViewModel
      //};

      //mainWindow.Show();


      foreach(var arg in e.Args)
      {
        MessageBox.Show(arg.ToString());
      }

      var loginVM = new LoginViewModel();
      LoginWindow l = new LoginWindow { DataContext = loginVM };
      l.Show();


  
    }

    private void InitDatabaseIfNotExist()
    {
      if (!File.Exists(DBPath))
      {
        File.Create(DBPath).Close();
        XmlSerializer x = new XmlSerializer(typeof(ReadingModel));
        if (!string.IsNullOrWhiteSpace(DBPath) && File.Exists(DBPath))
        {
          using (TextWriter tw = new StreamWriter(DBPath))
          {
            // Update reading model object with new values
            MovieModel model = new MovieModel { Title = "Movie Title: Example", Genre = "Action", Duration = 55, ReleaseYear = 1999, IsMovieSeen = false };

            List<MovieModel> startupList = new List<MovieModel>();
            startupList.Add(model);
            ReadingModel initial = new ReadingModel();
            initial.NonWatchedMovies = startupList;

            x.Serialize(tw, initial);
          }
        }
      }
    }
  }
}
