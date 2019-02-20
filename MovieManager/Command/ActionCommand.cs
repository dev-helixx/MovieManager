using System;
using System.Windows.Input;

namespace MovieManager.Command
{

  namespace MovieManager.Command
  {
    public class ActionCommand : ICommand
    {
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
