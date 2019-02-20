using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Command.MovieManager.Command;

namespace MovieManager.ViewModels
{
  public class BaseViewModel : INotifyPropertyChanged
  {
    private readonly BaseViewModel Parent;

    public BaseViewModel(BaseViewModel parent)
    {
      Parent = parent;
    }

    public BaseViewModel()
    {
    }

    // List containing various commands (button handlers)
    List<ActionCommand> commands = new List<ActionCommand>();


    /* Method for adding commands to a list */
    public void RegisterCommand(ActionCommand command)
    {
      commands.Add(command);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string property)
    {

      foreach (ActionCommand command in commands)
      {
        command.RaiseCanExecuteChanged();
      }

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
  }
}
