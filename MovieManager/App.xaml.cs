using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MovieManager.FilesystemHandler;
using MovieManager.Models;
using MovieManager.ViewModel;

namespace MovieManager
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {


    private const string DBPath = @"c:\tmp\movie_db.txt";


    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);


      ReadingModel readingModel = new ReadingModel(DBPath); // Reading Model ( Reads data from db file and saves it in a list of movie objects)

      var MainWindowViewModel = new MainViewModel(readingModel); // Pass model to MainViewModel

      // Initialize filewatcher to watch for changes in the DB file
      new Filewatcher(MainWindowViewModel).Init();


      var mainWindow = new MainWindow
      {
        DataContext = MainWindowViewModel // Set datacontext to main ViewModel
      };

      mainWindow.Show();
    }
  }
}
