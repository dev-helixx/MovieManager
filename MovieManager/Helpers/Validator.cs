using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieManager.Helpers
{
  public static class Validator
  {

    /* Check out this websit https://regexr.com */


    public static bool IsValidEmailAddress(string emailInput)
    {
      Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
      if (emailInput != null)
        return regex.IsMatch(emailInput);
      return false;
    }
  }

}
