using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.ViewModels;

namespace MovieManager.Models
{
  public class MovieModel : BaseViewModel
  {

    #region Properties
    public string Title { get; set; }

    public string Genre { get; set; }

    public int Duration { get; set; }

    public int ReleaseYear { get; set; }

    public bool IsMovieSeen { get; set; }

    #endregion

  }
}
