using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MovieManager.Command.MovieManager.Command;
using MovieManager.Models;
using MovieManager.ViewModels;

namespace MovieManager.ViewModel
{
  public class MainWindowViewModel
  {

    public MoviesViewModel MoviesViewModel { get; set; }

    public ActionCommand LoadCommand { get; set; }
    public ActionCommand SaveCommand { get; set; }



    private ReadingModel readingModel;
    public MainWindowViewModel(ReadingModel readingModel)
    {
      // ReadingModel contains a list (readingModel.Movies) of movie objects data fetched from the database
      this.readingModel = readingModel;

      // Pass list of movie objects to the MoviesViewModel
      MoviesViewModel = new MoviesViewModel(readingModel.Movies);

      

      // Do stuff with mainmodel object, which holds all information from db



   
    }
  }
}
