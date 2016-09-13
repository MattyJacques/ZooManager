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
    enum CreateMode { ID, NAME };           // Which mode to find the paving template with

    // Lists
    // Collection of building templates
    private PavementTemplateCollection _pavementTemplates;
    private List<GameObject> _pavements;    // List of current active paveings

    // Objects
    public GameObject _pole;                //The prefab used to generate the poles
    public Transform _startPole;            //Where pathing will start
    public Transform _endPole;              //Where pathing will end
    private bool _pathPaving = false;       //Check if we are pathing and whether to reset _currentPavement or not
    
    private bool _snapping = false;         //Snapping button
    public Transform _currentPavement;      // Current paveing to be placed

    private float _currentPaveY;            // Current paveing Y placement
    
    private Rect _rotateLeftRect;           //Rect for the rotate left button. Used to prevent over clicking.
    private Rect _rotateRightRect;          //Rect for the rotate right button. Used to prevent over clicking.
    private Rect _lockRect;                 //Rect for the lock button. Used to snap
    
    public Texture2D _leftArrow;            //Images for buttons
    public Texture2D _rightArrow;           //Images for buttons
    public Texture2D _lockTexture;           //Images for lock
    
    public Terrain terrain;                 //Allows the script to find the highest y point to place the building

    public float _fakeMoney = 100.0f;       //Used until money is implemented
    private string _paveType;               //Used for repeating placement
    private int numberOfPavs = 0;           //Keeps track of how many are made and helps with overlapping
      
    void Start()
    { // Load the buildings from Resources
      //627H & 880W //Test values to make sure that unity properlly streches the buttons to the right size.
      _rotateLeftRect = new Rect((Screen.width/44),Screen.height - (Screen.height/11),(Screen.width / 8.8f),(Screen.height / 31.35f));
      _rotateRightRect = new Rect((Screen.width/4.4f),Screen.height - (Screen.height/11),(Screen.width / 8.8f),(Screen.height / 31.35f));
      _lockRect = new Rect((Screen.width/44),Screen.height - (Screen.height/6),(Screen.width / 8.8f),(Screen.height / 31.35f));
      
      _startPole = new GameObject().transform;
      _endPole = new GameObject().transform;
      
      resetPoles();
      
      LoadPave();

    } // Start()
  
    void resetPoles()
    {
      _startPole.position = new Vector3(-1000,-1000,-1000);
      _endPole.position = new Vector3(-1000,-1000,-1000);
      _pathPaving = false;
    }
    
    void Update()
    { // Check if building is currently following mouse position

      if (_currentPavement)
      { // If there is currently a building being placed, update position
        // and check for mouse input

        UpdateMouseBuild();

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        { // If left click, place the building
          if (!_rotateLeftRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) &&
          !_rotateRightRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) &&
          !_lockRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
          {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
            {//PathFind instead
              PlacePole();
            }
            else if (!Input.GetKey(KeyCode.LeftControl))
            {
              PlaceBuilding();
            }
          }
        }
        else if (Input.GetMouseButtonDown(1))
        { // If right click, cancel build
          DeleteCurrBuild();
          resetPoles();
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
       if (_currentPavement == null)
       {
        resetPoles();
       }
      
      if (_startPole.position != new Vector3(-1000,-1000,-1000) && _endPole.position != new Vector3(-1000,-1000,-1000))
      {//Start paving
        _pathPaving = true;
        //set the starting point
         _currentPavement.position = _startPole.position;
        //Marking the current spot to remember where to come back to
        Vector3 currentPlace = _currentPavement.position;
        //Place the first one at the start
        PlaceBuilding();
        while(_currentPavement.position.x != _endPole.position.x && _currentPavement.position.z != _endPole.position.z)
        {//Proccess logic for pathing
          _currentPavement.position = currentPlace;
          Vector3 distance = _currentPavement.position - _endPole.position;
         
          if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
          {
            if (_endPole.position.x > _currentPavement.position.x)
            {
              _currentPavement.position = _currentPavement.position + new Vector3 ( 1, 0, 0);
            }
            else
            {
              _currentPavement.position = _currentPavement.position - new Vector3 ( 1, 0, 0);
            }
          }
          else
          {
            if (_endPole.position.z > _currentPavement.position.z)
            {
              _currentPavement.position = _currentPavement.position + new Vector3 ( 0, 0, 1);
            }
            else
            {
              _currentPavement.position = _currentPavement.position - new Vector3 ( 0, 0, 1);
            }
          }
          currentPlace = _currentPavement.position;
          PlaceBuilding();
        }
        
        resetPoles();
      }
      

    } // Update()


    private void PlaceBuilding()
    { // Place the building in the world, add to buildings list
      // stop mouse position updating building position
      bool place = true;
      Collider[] colliders;
       if((colliders = Physics.OverlapSphere(_currentPavement.position, 0.39f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
       {//Checks to see if anything is in the way
        foreach(Collider collider in colliders)
        {
          GameObject go = collider.gameObject; //This is the game object you collided with
          if(go.transform.name != _currentPavement.name &&  go.transform.name != "Terrain") 
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
      _pavements.Add(_currentPavement.gameObject);
      if (!_pathPaving)
        _currentPavement = null;
      numberOfPavs++;
      //Take money
      //_fakeMoney -= 5;
      if (_fakeMoney >= 5.0f)
      {//If you have enough money then keep making roads
        Pave(_paveType);
      }
      else
      {
        Debug.Log("Out of Money");
      }
        
     
    } // PlaceBuilding()
    
    private void PlacePole()
    {
    
      if (_startPole.position == new Vector3(-1000,-1000,-1000))
      {
        Destroy(_startPole.gameObject);
        string poleName = "Pavings/Prefabs/Pole";
        _startPole = ((GameObject)Instantiate(Resources.Load(poleName))).transform;
        _startPole.position = new Vector3((int)_currentPavement.position.x,(int)_currentPavement.position.y,(int)_currentPavement.position.z);
      }
      else if (_endPole.position == new Vector3(-1000,-1000,-1000))
      {
        Destroy(_endPole.gameObject);
        string poleName = "Pavings/Prefabs/Pole";
        _endPole = ((GameObject)Instantiate(Resources.Load(poleName))).transform;
        _endPole.position = new Vector3((int)_currentPavement.position.x,(int)_currentPavement.position.y,(int)_currentPavement.position.z);
      }
      
    }

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
        
        if (Input.GetKey(KeyCode.LeftShift) || _snapping)
        {//Snap
        
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
      if (_fakeMoney >= 5.0f)
      {
        _paveType = buildName;
        buildName = "Pavings/Prefabs/" + buildName;
        _currentPavement = ((GameObject)Instantiate(Resources.Load(buildName))).transform;
        _currentPavement.name = _currentPavement.name + numberOfPavs;
      }

    } // Pave()

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
        
        //Remove this next line to remove all trace of text
        GUI.Label(new Rect(_lockRect.x + (Screen.width/58),_lockRect.y - (Screen.height/31.35f),_lockRect.width,_lockRect.height), "Snap");
        if(GUI.Button(_lockRect, _lockTexture))
        {
          _snapping = !_snapping;
        }
      }
    } // OnGUI()

  } // BuildingManager
}
