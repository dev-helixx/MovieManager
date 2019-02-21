using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MovieManager.ViewModel;

namespace MovieManager.FilesystemHandler
{
  public class Filewatcher
  {

    private FileSystemWatcher watcher;
    private MainViewModel mainViewModel;
    private const string DBPath = @"c:\tmp\movie_db.txt";

    public Filewatcher(MainViewModel mainViewModel)
    {
      this.mainViewModel = mainViewModel;


    }

    public void Init()
    {
      // Create a new FileSystemWatcher and set its properties.
      watcher = new FileSystemWatcher();


      watcher.Path = Path.GetDirectoryName(DBPath);
      // Watch for changes in LastAccess and LastWrite times, and
      // the renaming of files or directories.
      watcher.NotifyFilter = NotifyFilters.LastAccess
                           | NotifyFilters.LastWrite
                           | NotifyFilters.FileName
                           | NotifyFilters.DirectoryName;

      // Only watch single file.
      watcher.Filter = Path.GetFileName(DBPath);

      // Add event handlers.
      watcher.Changed += OnChanged;
      watcher.Deleted += OnChanged;

      // Begin watching.
      watcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType == WatcherChangeTypes.Changed)
      {

        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => mainViewModel.CheckCanLoad = true));

      }
      else if (e.ChangeType == WatcherChangeTypes.Deleted)
      {
        //MessageBox.Show("File has been deleted");
      }

    }

  }
}
