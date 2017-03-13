// Title        : AnimalManager.cs
// Purpose      : Contains the list of active enclosures, can also find
//                whether a position is in a enclosures, returning ref
// Author       : Matthew Jacques
// Date         : 13/03/2017

using System.Collections.Generic;
using UnityEngine;

public class EnclosureUtilities : MonoBehaviour
{
  // List of active enclosures
  static List<Enclosure> _activeEnclosures = new List<Enclosure>();

	static void AddEnclosure(ref Enclosure enclosure)
  { // Add a enclosure to the active list
    _activeEnclosures.Add(enclosure);
  } // AddEnclosure()

  static void RemoveEnclosure(string encName)
  { // Remove the enclosure with the given name from the active list

    for (int i = 0; i < _activeEnclosures.Count; i++)
    { // Check all enclosures for the correct name
      if (_activeEnclosures[i].name == encName)
      { // Correct name found, remove and break from loop
        _activeEnclosures.RemoveAt(i);
        break;
      }
    }

  } // RemoveEnclosure()

  static bool IsActiveEnclosure(Vector3 position, ref Enclosure enclosureRef)
  { // Get the reference to the enclosure if there is one. 

    bool result = false;

    foreach (Enclosure enclosure in _activeEnclosures)
    { // Check each enclosure to see if the position is inside
      if (enclosure.PositionExistsInsideEnclosure(position))
      { // Enclosure found, set reference and result and break
        enclosureRef = enclosure;
        result = true;
        break;
      }
    }

    return result;

  } // IsActiveEnclosure()
}
