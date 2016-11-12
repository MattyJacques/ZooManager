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

    // Which mode to find the animal template with
    enum CreateMode { ID, NAME };

    // Holds all animal templates read from JSON array
    public AnimalTemplateCollection _templates;

    private static List<GameObject> _animalPrefabs;    // List of all available animals
    Dictionary<int, string> _prefabIndex;

    // List of all active animals
    List<AnimalBase> _animals = new List<AnimalBase> { };

    BehaviourCreator _behaviours;         // Creates all behaviours for animals

    void Start()
    { // Call to get the templates from JSON

      // Setup behaviour tree
      _behaviours = new BehaviourCreator();
      _templates = JSONReader.ReadJSON<AnimalTemplateCollection>("Animals/Animals");
      _behaviours.CreateBehaviours();

      // Load all animal prefabs
      _prefabIndex = new Dictionary<int, string>();
      LoadAnimals();

    } // Start()

    void Update()
    {
      foreach (AnimalBase animal in _animals)
      {
        animal.CheckNeeds();
      }
    }

    public void Create(int id, int amount, Vector3 location)
    { // Create an animal instance using the ID field of the templates

      // Find index in array
      int index = GetTemplateIndex(id, "", CreateMode.ID);

      if (index >= 0)
      { // Make sure template was found before creating the animal
        CreateAnimal(index, amount, location);
      }

    } // Create(id)


    public void Create(string name, int amount, Vector3 location)
    { // Create an animal instance using the name field of the templates

      // Find index in array
      int index = GetTemplateIndex(0, name, CreateMode.NAME);

      if (index >= 0)
      { // Make sure template was found before creating the animal
        CreateAnimal(index, amount, location);
      }

    } // Create(name)


    public void Create(LevelAnimalTemplate template)
    { // Create an animal instance using the template loaded from the level
      // loader

      // Find index in array
      int index = GetTemplateIndex(0, template.name, CreateMode.NAME);

      if (index >= 0)
      { // Make sure template was found before creating the animal
        CreateAnimal(index, 1, new Vector3(template.posX,
                                           template.posY,
                                           template.posZ));
      }

    } // Create()


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


    private void CreateAnimal(int index, int amount, Vector3 location)
    { // Create and store the animal using the template index, amount of animals
      // and the current location of the animal

      for (int i = 0; i < amount; i++)
      { // Create as many animals as needed

        // Create new animal with found template
        AnimalBase newBase = new AnimalBase(_templates.animalTemplates[index]);

        // Update location of object
        newBase.UnityTransform.position = location;

        // Add animal to instances list
        _animals.Add(newBase);
      }

    } // Create()

    private void LoadAnimals()
    { // Load animals from Assets/Resources

      DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Animals");
      DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

      _animalPrefabs = new List<GameObject>();

      int count = 0;

      foreach (DirectoryInfo dir in subDirectories)
      {
        Debug.Log("Searching directory: " + dir.Name);

        foreach (FileInfo file in dir.GetFiles())
        {
          if (file.Name.EndsWith("prefab"))
          {
            _animalPrefabs.Add((GameObject)Resources.Load(dir.Name + "/" + file.Name));
            _prefabIndex.Add(_prefabIndex.Count, Path.GetFileNameWithoutExtension(file.Name));
            Debug.Log("Loaded " + dir.Name + "/" + file.Name);
          }
        }
      }
    } // LoadAnimals()

  } // AnimalManager

} // Namespace
