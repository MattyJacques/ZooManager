// Title        : AnimalManager.cs
// Purpose      : Initiates templates, manages instances of animals
// Author       : Matthew Jacques
// Date         : 27/08/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Animals;
using Assets.Scripts.Helpers;


namespace Assets.Scripts.Managers
{

  public class AnimalManager : MonoBehaviour
  {

    // Which mode to find the animal template with
    enum CreateMode { ID, NAME };

    // Holds all animal templates read from JSON array
    public AnimalTemplateCollection _templates;

    List<AnimalBase> _animals = new List<AnimalBase> { };


    void Start()
    { // Call to get the templates from JSON
    
      _templates = JSONReader.ReadJSON<AnimalTemplateCollection>("Animals/Animals");
			CreateAnimal (1);


    } // Start()
			
	void Update()
	{
			foreach (AnimalBase animal in _animals) 
			{
				animal.CheckedNeeds ();
			}
	}

    public void CreateAnimal(int id)
    { // Create an animal instance using the ID field of the templates

      // Find index in array
      int index = GetTemplateIndex(id, "", CreateMode.ID);

      // Create new animal with found template
      AnimalBase newAnimal = new AnimalBase(_templates.animalTemplates[index]);
      // Add animal to instances list
      _animals.Add(newAnimal);
			Debug.Log (_templates.animalTemplates [index].animalname);

    } // CreateAnimal(id)


    public void CreateAnimal(string name)
    { // Create an animal instance using the name field of the templates

      // Find index in array
      int index = GetTemplateIndex(0, name, CreateMode.NAME);

      // Create new animal with found template
      AnimalBase newAnimal = new AnimalBase(_templates.animalTemplates[index]);

      // Add animal to instances list
      _animals.Add(newAnimal);

    } // CreateAnimal(name)


    private int GetTemplateIndex(int id, string name, CreateMode mode)
    { // Get the template index using the name or id, whichever mode is passed in
      // Returns -1 if not found

      int templateIndex = -1;              // Holds the template index found

      for (int i = 0; i < _templates.animalTemplates.Length; i++)
      { // Check if there is a match for every template in the array

        if (mode == CreateMode.ID)
        { // If mode is ID, check for matching ID

          if (_templates.animalTemplates[i].id == id)
          { // Check for matching ID, if found set index and break out of loop
            templateIndex = i;
            break;
          }
        }
        else if (mode == CreateMode.NAME)
        { // If mode is name, check for matching name

          if (_templates.animalTemplates[i].animalname == name)
          { // Check for matching name, if found set index and break out of loop
            templateIndex = i;
            break;
          }
        }
      }

      return templateIndex;

    } // GetTemplateIndex()


  } // AnimalManager

} // Namespace
