using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MovieManager.Models
{
  public class ReadingModel
  {

    #region Properties
    public List<MovieModel> Movies { get; set; } // List containing Movie objects
    #endregion

    #region Constructors
    public ReadingModel()
    {
      Movies = new List<MovieModel>();
    }

    public ReadingModel(string DBPath)
    {

      // Fetch data from DB file
      ReadingModel mainModel = new ReadingModel();
      XmlSerializer x = new XmlSerializer(typeof(ReadingModel));
      using (TextReader tr = new StreamReader(DBPath))
      {
        mainModel = (ReadingModel)x.Deserialize(tr);
      }

      // Initialize properties with data fetched from DB
      Movies = mainModel.Movies;
    }

    #endregion
  }
}
