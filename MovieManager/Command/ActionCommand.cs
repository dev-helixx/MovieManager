using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MovieManager.Command
{

  namespace MovieManager.Command
  {
    public class ActionCommand : ICommand
    {
      private ActionCommand login;
      private object canLogin;

      public event EventHandler CanExecuteChanged;

      public void RaiseCanExecuteChanged()
      {
        CanExecuteChanged?.Invoke(this, null);
      }

      public ActionCommand(Action execute, Func<bool> canExecute)
      {
        this.execute = execute;
        this.canExecute = canExecute;
      }

      public ActionCommand(ActionCommand login, object canLogin)
      {
        this.login = login;
        this.canLogin = canLogin;
      }

      public ActionCommand(ActionCommand login, Func<bool> canLogin)
      {
        this.login = login;
        this.canLogin = canLogin;
      }

      private Action execute { get; set; }
      private Func<bool> canExecute { get; set; }

      public bool CanExecute(object parameter)
      {
        return canExecute();
      }

      public void Execute(object parameter)
      {
        execute();
      }
    }

  }

}
