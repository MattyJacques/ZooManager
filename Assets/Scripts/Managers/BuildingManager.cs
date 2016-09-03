// Title        : BuildingManager.cs
// Purpose      : Initiates templates, manages instances of animals
// Author       : Matthew Jacques
// Date         : 03/09/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Buildings;
using System;

namespace Assets.Scripts.Managers
{
  public class BuildingManager : MonoBehaviour
  {

    // Which mode to find the building template with
    enum CreateMode { ID, NAME };

    // Lists
    // Collection of building templates
    private BuildingTemplateCollection _buildingTemplates;
    private List<GameObject> _buildings;    // List of current active buildings

    // Objects
    private Transform _currentBuild;        // Current building to be placed
    public GameObject prefab;

    void Start()
    { // Load the buildings from Resources

      LoadBuildings();

    } // Start()

    
    void Update()
    { // Check if building is currently following mouse position

      if (_currentBuild)
      { // If there is currently a building being placed, update position
        // and check for mouse input

        UpdateMouseBuild();

        if (Input.GetMouseButtonDown(0))
        { // If left click, place the building
          PlaceBuilding();
        }
        else if (Input.GetMouseButtonDown(1))
        { // If right click, cancel build
          DeleteCurrBuild();
        }
      }

    } // Update()


    private void PlaceBuilding()
    { // Place the building in the world, add to buildings list
      // stop mouse position updating building position

      _buildings.Add(_currentBuild.gameObject);
      _currentBuild = null;
     
    } // PlaceBuilding()


    private void DeleteCurrBuild()
    { // Delete that current building that has been instantiated

      Destroy(_currentBuild.gameObject);
      _currentBuild = null;

    } // DeleteCurrBuild()


    private void UpdateMouseBuild()
    { // Update the position of the building object that is following the
      // mouse position to the new mouse position

      // Create raycast from screen point using mouse position
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      { // If raycast hits collider, update position of _currentBuild

        Vector3 newPos = new Vector3(hit.point.x, 0, hit.point.z);
        _currentBuild.position = newPos;
      }

    } // UpdateMouseBuilding()


    public void Create(string buildName)
    { // Get the index of the building with the name provided, then set the
      // current building to the building found

      //int index = GetBuildingIndex(0, buildName, CreateMode.NAME);
      //_currentBuild = ((GameObject)Instantiate(_buildingsTemplates[index])).transform;

      _currentBuild = ((GameObject)Instantiate(prefab)).transform;
    } // Create()


    private int GetBuildingIndex(int id, string name, CreateMode mode)
    { // Get the template index using the name or id, whichever mode is passed in
      // Returns -1 if not found

      int templateIndex = -1;              // Holds the template index found

      for (int i = 0; i < _buildings.Count; i++)
      { // Check if there is a match for every template in the array

        if (mode == CreateMode.ID)
        { // If mode is ID, check for matching ID

          if (_buildingTemplates.buildingTemplates[i].id == id)
          { // Check for matching ID, if found set index and break out of loop
            templateIndex = i;
            break;
          }
        }
        else if (mode == CreateMode.NAME)
        { // If mode is name, check for matching name

          if (_buildingTemplates.buildingTemplates[i].name == name)
          { // Check for matching name, if found set index and break out of loop
            templateIndex = i;
            break;
          }
        }
      }

      return templateIndex;

    } // GetTemplateIndex()


    private void LoadBuildings()
    { // Load buildings from Assets/Resources

      DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Buildings");
      DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

      _buildings = new List<GameObject>();
      
      foreach (DirectoryInfo dir in subDirectories)
      {
        Debug.Log("Searching directory: " + dir.Name);

        foreach (FileInfo file in dir.GetFiles())
        {
          if (file.Name.EndsWith("prefab"))
          {
            _buildings.Add((GameObject)Resources.Load(dir.Name + "/" + file.Name));
            Debug.Log("Loaded " + dir.Name + "/" + file.Name);
          }
        }

      }

    } // LoadBuildings()

  } // BuildingManager
}
