// Title        : JSONReader.cs
// Purpose      : Deserialises animal JSON array into a template collection
// Author       : Matthew Jacques
// Date         : 27/08/2016
// Notes        : You can make any animal you want by passing in the proper template
//                var newAnimal = new AnimalBase(collection.animalTemplates[0]);


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZooManager
{

  public class JSONReader
  {

    public static AnimalTemplateCollection ReadJSON()
    { // Read in the JSON file and return the AnimalTemplateCollection

      TextAsset animal = Resources.Load("Animals/Animals") as TextAsset;
      return JsonUtility.FromJson<AnimalTemplateCollection>(animal.text);

    } // ReadJSON()


  } // JSONReader
} // namespace
