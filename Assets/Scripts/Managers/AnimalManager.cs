// Title        : AnimalManager.cs
// Purpose      : Initiates templates, manages instances of animals
// Author       : Matthew Jacques
// Date         : 27/08/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZooManager
{

  public class AnimalManager : MonoBehaviour
  {

    // Which mode to find the animal template with
    enum CreateMode { ID, NAME };

    // Holds all animal templates read from JSON array
    public AnimalTemplateCollection _templates;

    List<AnimalBase> _animals;


    void Start()
    { // Call to get the templates from JSON

      _templates = JSONReader.ReadJSON();

    } // Start()


    public void CreateAnimal(int id)
    { // Create an animal instance using the ID field of the templates

      // Find index in array
      int index = GetTemplateIndex(id, "", CreateMode.ID);

      // Create new animal with found template
      AnimalBase newAnimal = new AnimalBase(_templates.animalTemplates[index]);

      // Add animal to instances list
      _animals.Add(newAnimal);

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
