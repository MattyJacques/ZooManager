using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PaveScript : MonoBehaviour
{
    // Objects
    public GameObject _pole;                //The prefab used to generate the poles
    public Transform _startPole;            //Where pathing will start
    public Transform _endPole;              //Where pathing will end
    public bool _pathPaving = false;       //Check if we are pathing and whether to reset _currentPavement or not
    
    public bool _snapping = false;         //Snapping button
    public string _paveType;               //Used for repeating placement
    public int _numberOfPavs = 0;           //Keeps track of how many are made and helps with overlapping
    
    public Rect _lockRect;                 //Rect for the lock button. Used to snap
    
    public Vector3 UpdateMouseBuild(Vector3 currentBuild, Terrain terrain)
    { // Update the position of the building object that is following the
      // mouse position to the new mouse position

      // Create raycast from screen point using mouse position
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      { // If raycast hits collider, update position of _currentPavement

        Vector3 newPos = new Vector3(hit.point.x, 1, hit.point.z);
        
        if (Input.GetKey(KeyCode.LeftShift) || _snapping)
        {//Snap
        
          int roundX = (int)newPos.x;
          int roundZ = (int)newPos.z;
          newPos.x = roundX;
          newPos.z = roundZ;
          
        } 
        //Gets the highest usable y point on the terrain.
        newPos.y = terrain.SampleHeight(newPos);
        
        currentBuild = newPos;
        
      }
      return currentBuild;
    } // UpdateMouseBuilding()
    public bool passInput(Vector3 placement)
    {
      bool place = false;
      if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
      {//PathFind instead
        PlacePole(placement);
      }
       else if (!Input.GetKey(KeyCode.LeftControl))
      {
        place = true;
      }
      
      return place;
    }//passInput()
    
    public void Start()
    {
      _lockRect = new Rect((Screen.width/44),Screen.height - (Screen.height/6),(Screen.width / 8.8f),(Screen.height / 31.35f));
      
      _startPole = new GameObject().transform;
      _endPole = new GameObject().transform;
      
      resetPoles();
    }
    
    private void PlacePole(Vector3 placement)
    {
      if (_startPole.position == new Vector3(-1000,-1000,-1000))
      {
        Destroy(_startPole.gameObject);
        string poleName = "Pavings/Prefabs/Pole";
        _startPole = ((GameObject)Instantiate(Resources.Load(poleName))).transform;
        _startPole.position = new Vector3((int)placement.x,(int)placement.y,(int)placement.z);
      }
      else if (_endPole.position == new Vector3(-1000,-1000,-1000))
      {
        Destroy(_endPole.gameObject);
        string poleName = "Pavings/Prefabs/Pole";
        _endPole = ((GameObject)Instantiate(Resources.Load(poleName))).transform;
        _endPole.position = new Vector3((int)placement.x,(int)placement.y,(int)placement.z);
      }
      
    } //PlacePoles()
    
    public void resetPoles()
    {
      _startPole.position = new Vector3(-1000,-1000,-1000);
      _endPole.position = new Vector3(-1000,-1000,-1000);
      _pathPaving = false;
    }//ResetPoles()
  }