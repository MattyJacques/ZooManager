// Title        : PaveManager.cs
// Purpose      : Initiates templates, manages instances of pavement
// Author       : Jacob Miller
// Date         : 09/11/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Buildings;
using System;

namespace Assets.Scripts.Managers
{
  public class PaveManager : MonoBehaviour
  {
    enum CreateMode { ID, NAME };           // Which mode to find the building template with

    // Lists
    // Collection of building templates
    private PavementTemplateCollection _pavementTemplates;
    private List<GameObject> _pavements;    // List of current active paveings

    // Objects
    public GameObject _pole;
    public Transform _currentPavement;         // Current paveing to be placed

    private float _currentPaveY;              // Current paveing Y placement
    
    private Rect _rotateLeftRect;           //Rect for the rotate left button. Used to prevent over clicking.
    private Rect _rotateRightRect;          //Rect for the rotate right button. Used to prevent over clicking.
    
    public Texture2D _leftArrow;            //Images for buttons
    public Texture2D _rightArrow;           //Images for buttons
    
    public Terrain terrain;                 //Allows the script to find the highest y point to place the building

    public float _fakeMoney = 100.0f;
    private string _paveType;
      
    void Start()
    { // Load the buildings from Resources
      //627H & 880W //Test values to make sure that unity properlly streches the buttons to the right size.
      _rotateLeftRect = new Rect((Screen.width/44),Screen.height - (Screen.height/11),(Screen.width / 8.8f),(Screen.height / 31.35f));
      _rotateRightRect = new Rect((Screen.width/4.4f),Screen.height - (Screen.height/11),(Screen.width / 8.8f),(Screen.height / 31.35f));
      
      LoadPave();

    } // Start()

    
    void Update()
    { // Check if building is currently following mouse position

      if (_currentPavement)
      { // If there is currently a building being placed, update position
        // and check for mouse input

        UpdateMouseBuild();

        if (Input.GetMouseButtonDown(0))
        { // If left click, place the building
          if (!_rotateLeftRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) &&
          !_rotateRightRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
          {
            PlaceBuilding();
          }
        }
        else if (Input.GetMouseButtonDown(1))
        { // If right click, cancel build
          DeleteCurrBuild();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
          Vector3 newRotation = new Vector3(0,-45,0);
          _currentPavement.Rotate(newRotation);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
          Vector3 newRotation = new Vector3(0,45,0);
          _currentPavement.Rotate(newRotation);
        }
      }
      

    } // Update()


    private void PlaceBuilding()
    { // Place the building in the world, add to buildings list
      // stop mouse position updating building position

      _pavements.Add(_currentPavement.gameObject);
      _currentPavement = null;
      if (_fakeMoney >= 5.0f)
      {
        Pave(_paveType);
        _fakeMoney -= 5;
      }
      else
      {
        Debug.Log("Out of Money");
      }
        
     
    } // PlaceBuilding()


    private void DeleteCurrBuild()
    { // Delete that current building that has been instantiated

      Destroy(_currentPavement.gameObject);
      _currentPavement = null;

    } // DeleteCurrBuild()


    private void UpdateMouseBuild()
    { // Update the position of the building object that is following the
      // mouse position to the new mouse position

      // Create raycast from screen point using mouse position
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      { // If raycast hits collider, update position of _currentPavement

        Vector3 newPos = new Vector3(hit.point.x, _currentPaveY, hit.point.z);
        
        if (Input.GetKey(KeyCode.LeftShift))
        {//Snap
          //48 / 10 = 4.8 -> (int) 4
          //4 * 10 = 40
          /*int roundX = (int)(newPos.x / 2);
          newPos.x = roundX * 2;
          int roundZ = (int)(newPos.z / 2);
          newPos.z = roundZ * 2;*/
          int roundX = (int)newPos.x;
          int roundZ = (int)newPos.z;
          newPos.x = roundX;
          newPos.z = roundZ;
        }
        
        //Resets _currentPaveY = a usable variable
        _currentPaveY = 1;
        //Gets the highest usable y point on the terrain.
        newPos.y = terrain.SampleHeight(newPos);
        
        _currentPavement.position = newPos;
        
      }

    } // UpdateMouseBuilding()

    public void Pave(string buildName)
    { // Get the index of the building with the name provided, then set the
      // current building to the building found

      _paveType = buildName;
      buildName = "Pavings/Prefabs/" + buildName;
      _currentPavement = ((GameObject)Instantiate(Resources.Load(buildName))).transform;

    } // Create()

    private int GetBuildingIndex(int id, string name, CreateMode mode)
    { // Get the template index using the name or id, whichever mode is passed in
      // Returns -1 if not found

      int templateIndex = -1;              // Holds the template index found

      for (int i = 0; i < _pavements.Count; i++)
      { // Check if there is a match for every template in the array

        if (mode == CreateMode.ID)
        { // If mode is ID, check for matching ID

          if (_pavementTemplates.pavementTemplates[i].id == id)
          { // Check for matching ID, if found set index and break out of loop
            templateIndex = i;
            break;
          }
        }
        else if (mode == CreateMode.NAME)
        { // If mode is name, check for matching name

          if (_pavementTemplates.pavementTemplates[i].name == name)
          { // Check for matching name, if found set index and break out of loop
            templateIndex = i;
            break;
          }
        }
      }

      return templateIndex;

    } // GetTemplateIndex()


    private void LoadPave()
    { // Load buildings from Assets/Resources

      DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Pavings");
      DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

      _pavements = new List<GameObject>();
      
      foreach (DirectoryInfo dir in subDirectories)
      {
        Debug.Log("Searching directory: " + dir.Name);

        foreach (FileInfo file in dir.GetFiles())
        {
          if (file.Name.EndsWith("prefab"))
          {
            _pavements.Add((GameObject)Resources.Load(dir.Name + "/" + file.Name));
            Debug.Log("Loaded " + dir.Name + "/" + file.Name);
          }
        }

      }

    } // LoadBuildings()
    
    private void OnGUI()
    { // Display buttons for rotation 
      if (_currentPavement != null)
      {
        //Remove this next line to remove all trace of text
        GUI.Label(new Rect(_rotateLeftRect.x + (Screen.width/58),_rotateLeftRect.y - (Screen.height/31.35f),_rotateLeftRect.width,_rotateLeftRect.height), "Rotate Left");
        if(GUI.Button(_rotateLeftRect, _leftArrow))
        {
          Vector3 newRotation = new Vector3(0,-45,0);
          _currentPavement.Rotate(newRotation);
        }
        
        //Remove this next line to remove all trace of text
        GUI.Label(new Rect(_rotateRightRect.x + (Screen.width/58),_rotateRightRect.y - (Screen.height/31.35f),_rotateRightRect.width,_rotateRightRect.height), "Rotate Right");
        if(GUI.Button(_rotateRightRect, _rightArrow))
        {
          Vector3 newRotation = new Vector3(0,45,0);
          _currentPavement.Rotate(newRotation);
        }
      }
    } // OnGUI()

  } // BuildingManager
}
