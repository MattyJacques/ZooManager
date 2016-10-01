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
    public Transform _currentBuild;        // Current building to be placed

    private float _currBuildY;              // Current building Y placement

    private Rect _rotateLeftRect;           //Rect for the rotate left button. Used to prevent over clicking.
    private Rect _rotateRightRect;          //Rect for the rotate right button. Used to prevent over clicking.

    public Texture2D _leftArrow;            //Images for buttons
    public Texture2D _rightArrow;           //Images for buttons
    public Texture2D _lockTexture;           //Images for lock

    public Terrain terrain;                 //Allows the script to find the highest y point to place the building
    
    public bool _isPave = false;            //Are you placing pavement or building
    public PaveScript _pave;                //Assistance in placing script
    public  AStar _aStar;

    void Start()
    { // Load the buildings from Resources

      //627H & 880W //Test values to make sure that unity properlly streches the buttons to the right size.
      _rotateLeftRect = new Rect((Screen.width / 44), Screen.height - (Screen.height / 11), (Screen.width / 8.8f), (Screen.height / 31.35f));
      _rotateRightRect = new Rect((Screen.width / 4.4f), Screen.height - (Screen.height / 11), (Screen.width / 8.8f), (Screen.height / 31.35f));

      LoadBuildings();
      
      //Set values
      _pave = gameObject.AddComponent<PaveScript>() as PaveScript;
      _pave.Start();

    } // Start()
  
    void Update()
    { // Check if building is currently following mouse position

      if (_currentBuild)
      { // If there is currently a building being placed, update position
        // and check for mouse input

        UpdateMouseBuild();
        
        if (Input.GetMouseButtonDown(0) || (_isPave && Input.GetMouseButtonDown(0)))
        { // If left click, place the building

          if (!_rotateLeftRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) &&
              !_rotateRightRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
          {
            if (_isPave && !_pave._lockRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {//Determine if you are placing placement poles
              if (_pave.passInput(_currentBuild.position))
                PlacePavement();
            }
            else if (!_isPave)
            {
              PlaceBuilding();
            }
          }

        }
        else if (Input.GetMouseButtonDown(1))
        { // If right click, cancel build
          DeleteCurrBuild();
          _pave.resetPoles();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
          Vector3 newRotation = new Vector3(0, -45, 0);
          _currentBuild.Rotate(newRotation);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
          Vector3 newRotation = new Vector3(0, 45, 0);
          _currentBuild.Rotate(newRotation);
        }
        if (_currentBuild == null)
        {
          _pave.resetPoles(); 
        }
        else if (_pave._startPole.position != new Vector3(-1000,-1000,-1000) && _pave._endPole.position != new Vector3(-1000,-1000,-1000))
        {
          paving();
        }
      }

    } // Update()
  
    public void Pave(string type)
    {//Loads the pavement as _currentBuild
      _isPave = true;
      _pave._paveType = type;
      type = "Pavings/Prefabs/" + type;
      _currentBuild = ((GameObject)Instantiate(Resources.Load(type))).transform;
      _currentBuild.name = _currentBuild.name + _pave._numberOfPavs;
    }//Pave()

    private void paving()
    {//The logic of pathing
      _pave._pathPaving = true;
      //set the starting point
       _currentBuild.position = _pave._startPole.position;
      //Marking the current spot to remember where to come back to
      Vector3 currentPlace = _currentBuild.position;
      //Place the first one at the start
      PlacePavement();
      while(_currentBuild.position.x != _pave._endPole.position.x && _currentBuild.position.z != _pave._endPole.position.z)
      {//Proccess logic for pathing
        _currentBuild.position = currentPlace;
        Vector3 distance = _currentBuild.position - _pave._endPole.position;
       
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
        {
          if (_pave._endPole.position.x > _currentBuild.position.x)
          {
            _currentBuild.position = _currentBuild.position + new Vector3 ( 1, 0, 0);
          }
          else
          {
            _currentBuild.position = _currentBuild.position - new Vector3 ( 1, 0, 0);
          }
        }
        else
        {
          if (_pave._endPole.position.z > _currentBuild.position.z)
          {
            _currentBuild.position = _currentBuild.position + new Vector3 ( 0, 0, 1);
          }
          else
          {
            _currentBuild.position = _currentBuild.position - new Vector3 ( 0, 0, 1);
          }
        }
        currentPlace = _currentBuild.position;
        PlacePavement();
      }
      _pave.  resetPoles();
    }//paving()
    
    private void PlaceBuilding()
    { // Place the building in the world, add to buildings list
      // stop mouse position updating building position

      _buildings.Add(_currentBuild.gameObject);
      //_aStar._map = _aStar.PlaceWall((int)_currentBuild.position.x/_aStar._scale,(int)_currentBuild.position.z/_aStar._scale, _aStar._map);
      _currentBuild = null;
     
    } // PlaceBuilding()

    private void PlacePavement()
    { // Place the building in the world, add to buildings list
      // stop mouse position updating building position
      bool place = true;
      Collider[] colliders;
       if((colliders = Physics.OverlapSphere(_currentBuild.position, 0.39f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
       {//Checks to see if anything is in the way
        foreach(Collider collider in colliders)
        {
          GameObject go = collider.gameObject; //This is the game object you collided with
          if(go.transform.name != _currentBuild.name &&  go.transform.name != "Terrain") 
          {
            Debug.Log(go.transform.name);
            place = false;
          }
        }
       }
      if (!place)
      {//if something is in the way then dont place it
        return;
      }
      //Placement stuffs
      //_pavements.Add(_currentBuild.gameObject);
      if (!_pave._pathPaving)
        _currentBuild = null;
      _pave._numberOfPavs++;
      Pave(_pave._paveType);
    }

    private void DeleteCurrBuild()
    { // Delete that current building that has been instantiated

      Destroy(_currentBuild.gameObject);
      _currentBuild = null;

    } // DeleteCurrBuild()

    private void UpdateMouseBuild()
    { // Update the position of the building object that is following the
      // mouse position to the new mouse position

      if (_isPave)
      {
        _currentBuild.position = _pave.UpdateMouseBuild(_currentBuild.position, terrain);
        return;
      }
      // Create raycast from screen point using mouse position
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      { // If raycast hits collider, update position of _currentBuild
        
        Vector3 newPos = new Vector3(hit.point.x, _currBuildY, hit.point.z);

        //Resets _currBuildY = a usable variable
        _currBuildY = 1;
        //Gets the highest usable y point on the terrain.
        newPos.y = terrain.SampleHeight(newPos);

        _currentBuild.position = newPos;
      }

    } // UpdateMouseBuilding()

    public void Create(string buildName)
    { // Get the index of the building with the name provided, then set the
      // current building to the building found

      // Build path then load the object
      buildName = "Buildings/Prefabs/" + buildName;
      UnityEngine.Object loadedObject = Resources.Load(buildName);

      if (loadedObject)
      { // If the object has been loaded, instantiate and store transform
        _currentBuild = ((GameObject)Instantiate(loadedObject)).transform;
      }

    } // Create()

    public void Create(LevelBuildingTemplate template)
    { // Create a building object using the template provided

      // Build path then load the object
      string buildName = "Buildings/Prefabs/" + template.name;
      UnityEngine.Object loadedObject = Resources.Load(buildName);

      if (loadedObject)
      { // If the object has been loaded, instantiate and store object
        _buildings.Add((GameObject)Instantiate(loadedObject));
      }
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

    private void OnGUI()
    { // Display buttons for rotation 
      if (_currentBuild != null)
      {
        //Remove this next line to remove all trace of text
        GUI.Label(new Rect(_rotateLeftRect.x + (Screen.width / 58), _rotateLeftRect.y - (Screen.height / 31.35f), _rotateLeftRect.width, _rotateLeftRect.height), "Rotate Left");
        if (GUI.Button(_rotateLeftRect, _leftArrow))
        {
          Vector3 newRotation = new Vector3(0, -45, 0);
          _currentBuild.Rotate(newRotation);
        }

        //Remove this next line to remove all trace of text
        GUI.Label(new Rect(_rotateRightRect.x + (Screen.width / 58), _rotateRightRect.y - (Screen.height / 31.35f), _rotateRightRect.width, _rotateRightRect.height), "Rotate Right");
        if (GUI.Button(_rotateRightRect, _rightArrow))
        {
          Vector3 newRotation = new Vector3(0, 45, 0);
          _currentBuild.Rotate(newRotation);
        }
        
        if (_isPave)
        {
          //Remove this next line to remove all trace of text
          GUI.Label(new Rect(_pave._lockRect.x + (Screen.width/58),_pave._lockRect.y - (Screen.height/31.35f),_pave._lockRect.width,_pave._lockRect.height), "Snap");
          if(GUI.Button(_pave._lockRect, _lockTexture))
          {
            _pave._snapping = !_pave._snapping;
          }
        }
      }
    } // OnGUI()

  } // BuildingManager
}
