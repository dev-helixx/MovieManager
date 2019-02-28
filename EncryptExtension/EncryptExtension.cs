using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using EncryptExtension.Properties;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace ËncryptExtension
{
  [ComVisible(true)]
  [COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
  public class EncryptExtension : SharpContextMenu
  {


    protected override bool CanShowMenu()
    {
      //  We will always show the menu.
      return true;
    }

    protected override ContextMenuStrip CreateMenu()
    {
      //  Create the menu strip.
      var menu = new ContextMenuStrip();


      //  Create a 'count lines' item.
      var itemEncrypt = new ToolStripMenuItem
      {
        Text = "Krypter filer",
        // Project -> Properties -> Resources -> Top nav bar choose Image -> drag n drop image 
        Image = Resources.logo
      };

      //  When we click, we'll call the 'PassCommandLineArgs' function.
      itemEncrypt.Click += (sender, args) => PassArgumentsToApplication();

      //  Add the item to the context menu.
      menu.Items.Add(itemEncrypt);

      //  Return the menu.
      return menu;
    }


    private void PassArgumentsToApplication()
    {

      var builder = new StringBuilder();

      //  Go through each file.
      foreach (var filePath in SelectedItemPaths)
      {
        // Add filename to builder with quotes, otherwise it cannot be passed as a single argument
        builder.AppendLine("\"" + filePath +"\"");

      }

      try
      {
        using (Process myProcess = new Process())
        {
          // Path to which application to start when the item in the contextmenu is clicked
          var dir = @"C:\Users\ses\source\repos\MovieManager\MovieManager\bin\Debug\MovieManager.exe";

          //var dir = AppDomain.CurrentDomain.BaseDirectory + @"MovieManager.exe";

          myProcess.StartInfo.UseShellExecute = false;
          myProcess.StartInfo.FileName = dir;
          myProcess.StartInfo.Arguments = builder.ToString();
          myProcess.Start();

        
        }
      }
      catch (Exception e)
      {
        MessageBox.Show("An error happened in EncryptExtension assembly:\n" + e.Message);
      }

    }

  }
}
