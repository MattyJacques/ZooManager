// Title        : AnimalManager.cs
// Purpose      : Initiates templates, manages instances of animals
// Author       : Matthew Jacques
// Date         : 27/08/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Characters.Animals;
using Assets.Scripts.Helpers;
using Assets.Scripts.BehaviourTree;


namespace Assets.Scripts.Managers
{

  public class AnimalManager : MonoBehaviour
  {

    struct Animal
    { // Struct to hold all the information on an animal, this includes the ID,
      // the template and the prefab
      public string ID { get; set; }
      public AnimalTemplate Template { get; set; }
      public GameObject Prefab { get; set; }
    };

    // Holds all animal templates read from JSON array
    public AnimalTemplateCollection _templates;

    private List<Animal> _animalCollection;

    // List of all active animals
    List<AnimalBase> _animals = new List<AnimalBase> { };

    BehaviourCreator _behaviours;         // Creates all behaviours for animals

    void Start()
    { // Call to get the templates from JSON

      // Setup behaviour tree
      _behaviours = new BehaviourCreator();
      _templates = JSONReader.ReadJSON<AnimalTemplateCollection>("Animals/Animals");
      _behaviours.CreateBehaviours();

      // Load all animals
      _animalCollection = new List<Animal>();
      LoadAnimals();

    } // Start()

    void Update()
    {
      foreach (AnimalBase animal in _animals)
      {
        animal.CheckNeeds();
      }
    }

    public void Create(string id, int amount, Vector3 location)
    { // Create an animal instance using the ID field of the templates

      // Find index in array
      int index = GetAnimalIndex(id);

      if (index >= 0)
      { // Make sure template was found before creating the animal
        CreateAnimal(index, amount, location);
      }

    } // Create(id)

    public void Create(LevelAnimalTemplate template)
    { // Create an animal instance using the template loaded from the level
      // loader

      // Find index in array
      int index = GetAnimalIndex(template.id);

      if (index >= 0)
      { // Make sure template was found before creating the animal
        CreateAnimal(index, 1, new Vector3(template.posX,
                                           template.posY,
                                           template.posZ));
      }

    } // Create(LevelAnimalTemplate)


    private int GetAnimalIndex(string id)
    { // Get the index of the Animal struct within the _animalCollection

      int animalIndex = -1;              // Holds the template index found

      for (int i = 0; i < _templates.animalTemplates.Length; i++)
      { // Check if there is a match for every template in the array


        if (_templates.animalTemplates[i].id == id)
        { // Check for matching ID, if found set index and break out of loop
          animalIndex = i;
          break;
        }
      }

      return animalIndex;

    } // GetTemplateIndex()


    private void CreateAnimal(int index, int amount, Vector3 location)
    { // Create and store the animal using the template index, amount of animals
      // and the current location of the animal

      for (int i = 0; i < amount; i++)
      { // Create as many animals as needed

        // Create new animal with found template
        AnimalBase newBase = new AnimalBase(_animalCollection[index].Template,
                                            _animalCollection[index].Prefab);

        // Update location of object
        newBase.Model.transform.position = location;

        // Add animal to instances list
        _animals.Add(newBase);
      }

    } // Create()

    private void LoadAnimals()
    { // Load animals from Assets/Resources

      DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Animals");
      DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

      foreach (DirectoryInfo dir in subDirectories)
      {
        Debug.Log("Searching directory: " + dir.Name);

        foreach (FileInfo file in dir.GetFiles())
        {
          if (file.Name.EndsWith("prefab"))
          { // Create a new Animal struct with the ID, prefab and template of
            // the animal found
            Animal newAnimal = new Animal();
            newAnimal.ID = Path.GetFileNameWithoutExtension(file.Name);
            newAnimal.Prefab = (GameObject)Resources.Load(dir.Name + "/" + file.Name);
            
            foreach (AnimalTemplate template in _templates.animalTemplates)
            {
              if (template.id == newAnimal.ID)
              {
                newAnimal.Template = template;
                break;
              }
            }

            Debug.Log("Loaded " + dir.Name + "/" + file.Name);
          }
        }
      }
    } // LoadAnimals()

  } // AnimalManager

} // Namespace
