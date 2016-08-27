using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSONReader : MonoBehaviour {

  TextAsset animal;
  void Start ()
  {
    animal = Resources.Load("Animals/Animals") as TextAsset;
    TextAsset animla2 = animal;
    var collection = JsonUtility.FromJson<AnimalTemplateCollection>(animal.text);

    // You can make any animal you want by passing in the proper template
    var newAnimal = new AnimalBase(collection.animalTemplates[0]);
  }
}
