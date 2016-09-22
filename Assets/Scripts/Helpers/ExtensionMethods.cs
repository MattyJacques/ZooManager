using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Helpers
{
  public static class ExtensionMethods
  {
    //Convert a string[3] to a Vector3
    public static Vector3 ParseVec3(this string[] s)
    {
      return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }

    //Check if a tring is numeric
    public static bool IsNumeric(this string s)
    {
      float output;
      return float.TryParse(s, out output);
    }
  }
}
