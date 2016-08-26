using UnityEngine;
using System.Collections;

public class AnimalCollection : MonoBehaviour
{
  public AnimalBase[] animalCol;
}


public class JSONReader : MonoBehaviour {

  TextAsset animal;

	void Start ()
  {
    animal = Resources.Load("Animals/Animals") as TextAsset;
    TextAsset animla2 = animal;
    AnimalCollection animals = JsonUtility.FromJson<AnimalCollection>(animal.text);
	}
	
}
