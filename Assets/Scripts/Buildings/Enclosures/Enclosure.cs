﻿// Title        : Enclosure.cs
// Purpose      : This class controls both the GUI and functionality for the enclosure
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Characters.Animals;

//TODO: separate the GUI into it's own class
public class Enclosure : MonoBehaviour
{
  private int _maxNameLength = 20;
  private int _minNameLength = 3;
  
  /*
  //TODO: Implement State to track what state the enclosure currently is in
  //Commented out for now to suppress warnings generated by unused variables
  private enum State
  {
    Idle,
    DisplayingMenu,
  };
  
  private State _state = State.Idle;
  */

  private string _name;
  private GameObject _canvas;
  private EnclosureGUIController _enclosureGUIController;

  private List<EnclosureInteriorItem> _interiorItems = new List<EnclosureInteriorItem>();
  private List<AnimalBase> _animals = new List<AnimalBase>();

  public void Start()
  { //Initialization

    //Check that we have a general collider
    if (!GetComponent<BoxCollider>() && !GetComponent<MeshCollider>())
    {
      Debug.LogError("Enclosure " + name + " at " + transform.position.ToString() + " has no collider!");
    }

    //Get the GUI controller
    _enclosureGUIController = GetComponent<EnclosureGUIController> ();

  } //Start()

  public void OnClick()
  { //Is called when the player clicks on the Enclosure NOTE: replace with onclick?
    //Instantiates and initializes the GUI that allows for player interaction with the enclosure

    if (_enclosureGUIController._state == EnclosureGUIController.UIState.Uninitialized)
    {
      _enclosureGUIController.Initialize ();
    }
    else if (_enclosureGUIController._state == EnclosureGUIController.UIState.Hidden)
    {
      _enclosureGUIController.ShowCanvas ();
    }
    else
    { //We might not want to hide the GUI on aditional clicks.
      _enclosureGUIController.HideCanvas ();
    }
  } //OnClick()

  public Vector3 GetRandomPointOnTheGround()
  { //Returns a random point inside of the enclosure that is on the ground

    //Get the extents of the enclosure's collider
    Bounds colBounds = GetComponent<Collider> ().bounds;
    float xExtent = colBounds.extents.x;
    xExtent -= 0.25f; //Subtract so that the point isn't too close to a wall
    float zExtent = colBounds.extents.z;
    zExtent -= 0.25f;

    //Get a random point
    Vector3 randomPoint = transform.position;
    randomPoint.x += Random.Range (-xExtent, xExtent);
    randomPoint.z += Random.Range (-zExtent, zExtent);

    //Move the point to the ground (incase the ground is uneven)
    randomPoint.y += colBounds.extents.y - 0.2f;
    RaycastHit rayHit = new RaycastHit ();
    if (!Physics.Raycast (randomPoint, Vector3.down, out rayHit))
    {
      Debug.LogError ("Raycast inside of enclosure " + _name + " originating at " + randomPoint.ToString() + 
        " and going straight down, did not hit the ground.");
    }
    
    //TODO: Implement checks for what we hit right here.
    //NOTE: Really should have a layer for the ground to make this easier.
    return rayHit.point;

  } //GetRandomPointOnTheGround()

  public Transform GetClosestInteriorItemTransform(Vector3 fromPosition, EnclosureInteriorItem.InteriorItemType itemType)
  { //Returns the closest Transform of itemType

    //Check if any items of itemType exists
    if (_interiorItems.Count <= 0)
    {
      Debug.LogWarning ("Tried getting the closest interior items of type " + itemType.ToString () +
        " but enclosure " + _name + " contains no interiorItems at all!");
      return null;
    }
    else
    {
      if (_interiorItems.Count (x => x.type == itemType) <= 0)
      {
      Debug.LogWarning ("Tried getting the closest interior items of type " + itemType.ToString () +
        " but enclosure " + _name + " contains no interiorItems of that type!");
        return null;
      }
    }

    //Get the item of type itemType
    if (itemType == EnclosureInteriorItem.InteriorItemType.Random)
    { //Random is a wildcard, so we return any item
      int r = Random.Range (0, _interiorItems.Count);
      return _interiorItems[r].transform;
    }

    else
    { //Return the closest item of itemType
      EnclosureInteriorItem interiorItem = _interiorItems.Where (x => x.type == itemType)
          .OrderBy (x => Vector3.Distance (fromPosition, x.transform.position))
          .FirstOrDefault ();

      if (interiorItem == null)
        return null;

      return interiorItem.transform;
    }
  } //GetClosest()

  public void RegisterNewInteriorItem(GameObject gameObject, EnclosureInteriorItem.InteriorItemType itemType)
  { // Register a new interior object into a enclosure

    EnclosureInteriorItem newItem = new EnclosureInteriorItem(gameObject, itemType);
    _interiorItems.Add(newItem);

    Debug.Log("Added new interiorItem " + gameObject.name
        + ", of type " + System.Enum.GetName(typeof(EnclosureInteriorItem.InteriorItemType), itemType)
        + ", to enclosure " + _name
        + ", at position " + gameObject.transform.position.ToString()
        + ".");

  } //RegisterNewInteriorItem()

  public void RegisterNewAnimal(AnimalBase animal)
  { // Register a new animal into a enclosure

    _animals.Add(animal);

    Debug.Log("Added new animal " + animal.Model.name
          + " to enclosure " + _name);

  }

  public bool Rename(string name)
  { //Renames the enclosure

    if (name.Length > _minNameLength || name.Length < _maxNameLength)
    {
      _name = name;
      if (_canvas != null)
      {
        _canvas.transform.FindChild("Text_EnclosureName").GetComponent<Text>().text = _name;
      }
      return true;
    }
    else
    {
      return false;
    }

  } //Rename()

  public bool PositionExistsInsideEnclosure(Vector3 pos)
  { //Returns true if pos is inside enclosure bounds

    //NOTE: Assumes that enclosure only has ONE collider, might not work with multiple.
    Debug.Log(GetComponent<Collider>().bounds.ToString());
    Debug.Log(pos.ToString());

    return GetComponent<Collider>().bounds.Contains (pos);

  } //PositionExistsInsideEnclosure()

  private void DeleteThisEnclosure()
  { //Deletes this enclosure and all attached objects

    if (_canvas != null)
    {
      Destroy(_canvas);
    }

    Destroy(gameObject);

  } //DeleteThisEnclosure

} // Enclosure
