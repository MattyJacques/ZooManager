using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Vector3 ParseVec3(this string[] s)
    {
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
}
