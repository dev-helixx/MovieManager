using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MovieManager.Command.MovieManager.Command;

namespace MovieManager.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {




    #region Constructors
    public LoginViewModel()
    {



      RegisterCommand(LoginButtonCommand = new ActionCommand(Login, CanLogin));
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
      MessageBox.Show("Log in pressed");
    }
    #endregion



    #region Properties


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
