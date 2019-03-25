using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using MovieManager.Command.MovieManager.Command;
using MovieManager.FilesystemHandler;
using MovieManager.Models;
using MovieManager.ViewModel;

namespace MovieManager.ViewModels
{
  public class LoginViewModel : BaseViewModel, IDataErrorInfo
  {


    // Database file path
    private const string DBPath = @"c:\tmp\movie_db.txt";


    #region Constructors
    public LoginViewModel()
    {


      // Subscribe to LoginVewModels OnpropertyChanged event
      this.PropertyChanged += LoginViewModel_PropertyChanged;

      // Register login button 
      RegisterCommand(LoginButtonCommand = new ActionCommand(Login, CanLogin));
    }

    private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

    public static string GetRandomAlphanumericString(int length)
    {
      const string alphanumericCharacters =
          "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
          "abcdefghijklmnopqrstuvwxyz" +
          "0123456789" +
          "!?";
      return GetRandomString(length, alphanumericCharacters);
    }

    public static string GetRandomString(int length, IEnumerable<char> characterSet)
    {
      if (length < 0)
        throw new ArgumentException("length must not be negative", "length");
      if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
        throw new ArgumentException("length is too big", "length");
      if (characterSet == null)
        throw new ArgumentNullException("characterSet");
      var characterArray = characterSet.Distinct().ToArray();
      if (characterArray.Length == 0)
        throw new ArgumentException("characterSet must not be empty", "characterSet");

      var bytes = new byte[length * 8];
      new RNGCryptoServiceProvider().GetBytes(bytes);
      var result = new char[length];
      for (int i = 0; i < length; i++)
      {
        ulong value = BitConverter.ToUInt64(bytes, i * 8);
        result[i] = characterArray[value % (uint)characterArray.Length];
      }
      return new string(result);
    }

    private void Login()
    {

      //MessageBox.Show(GetRandomAlphanumericString(12));

      //if (UserId.Equals("silas") && Password.Equals("silas"))
      //{


      ReadingEntity readingModel = new ReadingEntity(DBPath); // Reading Model ( Reads data from db file and saves it in a list of movie objects)

      var mainVM = new MainViewModel(readingModel); // Pass model to MainViewModel

      // Initialize filewatcher to watch for changes in the DB file
      new Filewatcher(mainVM).Init();



      ProgressRingVisibility = true;



      var mainWindow = new MainWindow
      {
        DataContext = mainVM // Set datacontext to main ViewModel
      };

      mainWindow.Show();

      Application.Current.Windows[0].Close();

    }


    #endregion



    #region Properties

    public string DisplayedImage { get { return "/MovieManager;component/Images/unnamed.png"; } }

  
    private bool _progressRingVisibility;
    public bool ProgressRingVisibility
    {
      get { return _progressRingVisibility; }
      set
      {
        if(value != _progressRingVisibility)
        {
          _progressRingVisibility = value;
          OnPropertyChanged(nameof(ProgressRingVisibility));
        }
      }
    }


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

    #region INotifyDataErrorInfo Members
    public string Error
    {
      get { return null; }
    }

    public string this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "UserId":
            if (!string.IsNullOrEmpty(UserId))
              if (!UserId.Contains("@") && !UserId.Contains("."))
                return "Email address is not valid";
            break;
        }

        return string.Empty;
      }
    }
    #endregion

  }
}
