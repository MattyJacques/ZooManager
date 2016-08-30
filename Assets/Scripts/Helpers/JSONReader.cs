// Title        : JSONReader.cs
// Purpose      : Deserialises animal JSON array into a template collection
// Author       : Matthew Jacques
// Date         : 27/08/2016
// Notes        : You can make any animal you want by passing in the proper template
//                var newAnimal = new AnimalBase(collection.animalTemplates[0]);


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Animals;


namespace Assets.Scripts.Helpers
{

  public class JSONReader
  {
    public static T ReadJSON<T>(string location)
    { // Read in the JSON file and return the AnimalTemplateCollection

      TextAsset animal = Resources.Load(location) as TextAsset;
      return JsonUtility.FromJson<T>(animal.text);

    } // ReadJSON()
  } // JSONReader
} // namespace
