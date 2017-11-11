﻿// Sifaka Game Studios (C) 2017

using System.Collections.Generic;         
using System.IO;
using Assets.Scripts.Behaviours;
using Assets.Scripts.Buildings.Enclosures;
using UnityEngine;                        
using Assets.Scripts.Characters.Animals;      
using Assets.Scripts.Helpers;             

namespace Assets.Scripts.Managers
{
  public class AnimalManager : MonoBehaviour
  {

    public struct Animal
    { // Struct to hold all the information on an animal, this includes the ID,
      // the template and the prefab
      public string ID { get; set; }
      public AnimalTemplate Template { get; set; }
      public GameObject Prefab { get; set; }
    };

    // Storage for animal data
    public List<Animal> _animalCollection = new List<Animal>();  // Animal templates
    public List<AnimalBase> _animals = new List<AnimalBase>();   // Active animals
    BehaviourCreator _behaviours;                                 // Creates behaviours
    EnclosureUtilities _EnclosureCollection;                      // Holds all the enclsoures

    // Members for animal following mouse position
    private GameObject _currentAnimal;            // Current animal to be placed
    private int _currentIndex;                    // Index of _animals for current animal
    private LayerMask _terrainLayer;              // Used for mouse following

    /////////////////////////////////////////////////////////////////////////////////////////
    //
    // Monobehaviours
    //
    /////////////////////////////////////////////////////////////////////////////////////////
    #region Monobehaviours

    void Start()
    { // Set up behaviour tree and load animals. Set raise exceptions for debugging

      _terrainLayer = LayerMask.GetMask("Ground");

    } // Start()

    // Update is called once per frame
    void Update()
    {
      UpdateMouseAnimal();

    } // Update()

    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////
    //
    // Creating Animals
    //
    /////////////////////////////////////////////////////////////////////////////////////////
    #region Create Animals

    public void CreateFollowMouse(string id)
    { // Create an animal that will follow the mouse

      // Find index in array
      _currentIndex = GetIndex(id);

      if (_currentIndex >= 0)
      { // Make sure template was found before creating the animal

        // Create the animal
        _currentAnimal = Instantiate(_animalCollection[_currentIndex].Prefab);
      }

    } // Create(string)

    public void Create(LevelAnimalTemplate template)
    { // Create an animal instance using the template loaded from the level
      // loader

      // Find index in array
      _currentIndex = GetIndex(template.id);

      if (_currentIndex >= 0)
      { // Make sure template was found before creating the animal

        // Create the animal
        _currentAnimal = Instantiate(_animalCollection[_currentIndex].Prefab);
        _currentAnimal.transform.position = new Vector3(template.coords.x,
                                                        template.coords.y,
                                                        template.coords.z);
      }

    } // Create(LevelAnimalTemplate)

    private int GetIndex(string id)
    { // Get the index of the Animal struct within the _animalCollection

      int animalIndex = -1;              // Holds the template index found

      for (int i = 0; i < _animalCollection.Count; i++)
      { // Check if there is a match for every template in the array

        if (_animalCollection[i].ID == id)
        { // Check for matching ID, if found set index and break out of loop
          animalIndex = i;
          break;
        }
      }

      // Make sure index has been found
      Debug.Assert(-1 != animalIndex, "Animal index not found");

      return animalIndex;

    } // GetIndex()

    private void UpdateMouseAnimal()
    { // Update the position of the animal object that is currently following the
      // mouse position

      if (_currentAnimal)
      {
        if (Input.GetMouseButtonDown(0))
        { // If left mouse button is pressed, place the animal, update animal position with
          // the mouse position again.
          PlaceAnimal();
        }
        else
        { // Update animal position with the mouse position again

          // Create raycast from screen point using mouse position
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit;

          if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
          { // If raycast hits collider, update position of _currentAnimal
            _currentAnimal.transform.position = hit.point;
          }
        }
      }

    } // UpdateMouseAnimal()

    private void PlaceAnimal()
    { // Place the animal located in the _currentAnimal member into the game world

      AnimalBase newAnimal = new AnimalBase(_animalCollection[_currentIndex]);

      newAnimal.Model = _currentAnimal;

      newAnimal.Init();

      _animals.Add(newAnimal);
      _currentAnimal = null;
      _currentIndex = -1;

    } // PlaceAnimal()

    #endregion
    
  } // AnimalManager
} // Namespace
