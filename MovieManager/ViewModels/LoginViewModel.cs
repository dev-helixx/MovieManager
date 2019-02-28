using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MovieManager.Command.MovieManager.Command;
using MovieManager.FilesystemHandler;
using MovieManager.Models;
using MovieManager.ViewModel;

namespace MovieManager.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {


    // Database file path
    private const string DBPath = @"c:\tmp\movie_db.txt";


    #region Constructors
    public LoginViewModel()
    {

      CheckCanLogin = true;

      // Subscribe to LoginVewModels OnpropertyChanged event
      this.PropertyChanged += LoginViewModel_PropertyChanged;

      // Register login button 
      RegisterCommand(LoginButtonCommand = new ActionCommand(Login, CanLogin));
    }

    private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

      CheckCanLogin = (!string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Password)) ? true : false;

    }

    #endregion


    #region Bool Return-Methods
    private bool CanLogin()
    {
      return CheckCanLogin;
    }

    #endregion


    #region Methods
    private void Login()
    {



      //if (UserId.Equals("silas") && Password.Equals("silas"))
      //{


      ReadingModel readingModel = new ReadingModel(DBPath); // Reading Model ( Reads data from db file and saves it in a list of movie objects)

      var mainVM = new MainViewModel(readingModel); // Pass model to MainViewModel

      // Initialize filewatcher to watch for changes in the DB file
      new Filewatcher(mainVM).Init();


      var mainWindow = new MainWindow
      {
        DataContext = mainVM // Set datacontext to main ViewModel
      };

      mainWindow.Show();

      Application.Current.Windows[0].Close();

      //}
    }
    #endregion



    #region Properties

    public string DisplayedImage { get { return "/MovieManager;component/Images/unnamed.png"; } }

  

    private bool _checkCanLogin;
    public bool CheckCanLogin
    {
      get { return _checkCanLogin; }
      set
      {
        if(_checkCanLogin != value)
        {
          _checkCanLogin = value;
          OnPropertyChanged(nameof(CheckCanLogin));
        }
      }
    }

    private string _userId;
    public string UserId
    {
      get { return _userId; }
      set
      {
        if (_userId != value)
        {
          _userId = value;
          OnPropertyChanged(nameof(UserId));
        }
      }
    }

    private string _password;
    public string Password
    {
      get { return _password; }
      set
      {
        if (_password != value)
        {
          _password = value;
          OnPropertyChanged(nameof(Password));
        }
      }
    }

    #endregion

    #region Commands
    public ActionCommand LoginButtonCommand { get; set; }
    #endregion

  }
}
