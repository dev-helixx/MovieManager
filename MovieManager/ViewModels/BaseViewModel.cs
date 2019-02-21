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


  /********************************* Pub/Sub Implementation /*********************************/

  public delegate void PubSubEventHandler<T>(object sender, PubSubEventArgs<T> args);

  public class PubSubEventArgs<T> : EventArgs
  {
    public T Item { get; set; }

    public PubSubEventArgs(T item)
    {
      Item = item;
    }
  }

  public class PubSub<T>
  {
    private static Dictionary<string, PubSubEventHandler<T>> events =
            new Dictionary<string, PubSubEventHandler<T>>();

    public static void PublishEvent(string eventName, PubSubEventHandler<T> handler)
    {
      if (!events.ContainsKey(eventName))
        events.Add(eventName, handler);
    }
    public static void RaiseEvent(string eventName, object sender, PubSubEventArgs<T> args)
    {
      if (events.ContainsKey(eventName) && events[eventName] != null)
        events[eventName](sender, args);
    }
    public static void Subscribe(string eventName, PubSubEventHandler<T> handler)
    {
      if (events.ContainsKey(eventName))
        events[eventName] += handler;
    }
  }

  /****************************************************************************************/

}
