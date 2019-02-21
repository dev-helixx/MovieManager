using System;
using System.Collections.Generic;
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

    public delegate void PubSubEventHandler<T>(object sender, PubSubEventArgs<T> args);

    public class PubSubEventArgs<T> : EventArgs
    {
      public T Item { get; set; }

      public PubSubEventArgs(T item)
      {
        Item = item;
      }
    }

    public static class PubSub<T>
    {
      private static Dictionary<string, PubSubEventHandler<T>> events =
              new Dictionary<string, PubSubEventHandler<T>>();

      public static void AddEvent(string name, PubSubEventHandler<T> handler)
      {
        if (!events.ContainsKey(name))
          events.Add(name, handler);
      }
      public static void RaiseEvent(string name, object sender, PubSubEventArgs<T> args)
      {
        if (events.ContainsKey(name) && events[name] != null)
          events[name](sender, args);
      }
      public static void RegisterEvent(string name, PubSubEventHandler<T> handler)
      {
        if (events.ContainsKey(name))
          events[name] += handler;
      }
    }

  }

}
