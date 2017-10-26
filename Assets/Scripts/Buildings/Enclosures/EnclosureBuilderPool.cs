// Title        : EnclosureBuilderPool.cs
// Purpose      : This class is a specifically tailored object pool used by the EnclosureBuilder class
// Author       : Eivind Andreassen
// Date         : 18.01.2017

using System.Collections.Generic;
using UnityEngine;

public class EnclosureBuilderPool
{
  private GameObject _wallReference;
  private GameObject _cornerReference; //NOTE: The rotation changes every time it's instantiated? WTf?
  public enum ObjectType { Wall, Corner };

  private Stack<GameObject> _pooledWalls = new Stack<GameObject>();
  private List<GameObject> _instantiatedWalls = new List<GameObject>(); //TODO: track with bool instead
  private Stack<GameObject> _pooledCorners = new Stack<GameObject>();
  private List<GameObject> _instantiatedCorners = new List<GameObject>();
  private Vector3 _hiddenPos = new Vector3(-5000, -5000, -5000);

  private Material _wallMatOrig;
  private Material _wallMatTranslucent;
  private Material _cornerMatOrig;
  private Material _cornerMatTranslucent;

  public void Initialize(GameObject wall, GameObject corner, Material wallTranslucent, Material cornerTranslucent)
  { // Initialise the pool, setting up members
    //Get the materials
    _wallMatOrig = wall.GetComponent<Renderer>().sharedMaterial;
    _cornerMatOrig = corner.GetComponent<Renderer>().sharedMaterial;
    _wallMatTranslucent = wallTranslucent;
    _cornerMatTranslucent = cornerTranslucent;

#if UNITY_EDITOR
    //Init objects and set materials
    if (UnityEditor.PrefabUtility.GetPrefabType(wall) != UnityEditor.PrefabType.None)
    {
      wall = Object.Instantiate(wall);
    }

    wall.GetComponent<Renderer>().material = _wallMatTranslucent;
    wall.transform.position = _hiddenPos;
    _wallReference = wall;
    _pooledWalls.Push(Object.Instantiate(_wallReference));

    if (UnityEditor.PrefabUtility.GetPrefabType(corner) != UnityEditor.PrefabType.None)
    {
      corner = Object.Instantiate(corner);
    }
#endif // UNITY_EDITOR

    corner.GetComponent<Renderer>().material = _cornerMatTranslucent;
    corner.transform.position = _hiddenPos;
    _cornerReference = corner;
    _pooledCorners.Push(Object.Instantiate(_cornerReference));
  } // Initialize()

  /*
  public void ChangeObject(ObjectType type, GameObject newGameObject)
  {
      switch (type) {
          case ObjectType.Wall:
              DestroyGameObjectsInCollection (_pooledWalls);
              _pooledWalls.Clear ();
              DestroyGameObjectsInCollection (_instantiatedWalls);
              _instantiatedWalls.Clear ();

              InitializeNewGameObject (ObjectType.Wall, newGameObject);
              break;

          case ObjectType.Corner:
              DestroyGameObjectsInCollection (_pooledCorners);
              _pooledCorners.Clear ();
              DestroyGameObjectsInCollection (_instantiatedCorners);
              _instantiatedCorners.Clear ();

              InitializeNewGameObject (ObjectType.Corner, newGameObject);
              break;
      }
  }
  */

  private GameObject Instantiate(ObjectType type)
  { // Instantiate the object taking in account the respective type reference
    GameObject g = null;
    switch (type)
    {
      case ObjectType.Wall:
        if (_pooledWalls.Count < 1)
        {
          g = Object.Instantiate(_wallReference);
        }
        else
        {
          g = _pooledWalls.Pop();
        }
        g.transform.rotation = _wallReference.transform.rotation;
        _instantiatedWalls.Add(g);
        break;

      case ObjectType.Corner:
        if (_pooledCorners.Count < 1)
        {
          g = Object.Instantiate(_cornerReference);
        }
        else
        {
          g = _pooledCorners.Pop();
        }
        g.transform.rotation = _cornerReference.transform.rotation;
        _instantiatedCorners.Add(g);
        break;
    }
    return g;
  } // Instantiate()

  public GameObject[] InstantiateMultiple(ObjectType type, int amount)
  { // Instantate multiple types of an object
    List<GameObject> objectsToReturn = new List<GameObject>();
    for (int i = 0; i < amount; i++)
    {
      objectsToReturn.Add(Instantiate(type));
    }

    return objectsToReturn.ToArray();
  } // InstantiateMultiple()

  public void Destroy(ObjectType type, GameObject g)
  { // Destroy the game object
    switch (type)
    {
      case ObjectType.Wall:
        _instantiatedWalls.Remove(g);
        _pooledWalls.Push(g);
        break;

      case ObjectType.Corner:
        _instantiatedCorners.Remove(g);
        _pooledCorners.Push(g);
        break;
    }

    g.transform.position = _hiddenPos;
  } // Destroy()

  private void DestroyGameObjectsInCollection(IEnumerable<GameObject> col)
  { // Destroy all of the game objects in a collection
    foreach (GameObject g in col)
    {
      Object.Destroy(g);
    }
  } // DestroyGameObjectsInCollection()

  public void DestroyPool()
  { //Destroy pooled objects
    foreach (GameObject g in _pooledCorners)
    {
      Object.Destroy(g);
    }
    _pooledCorners.Clear();

    foreach (GameObject g in _pooledWalls)
    {
      Object.Destroy(g);
    }
    _pooledWalls.Clear();

    //Reset materials
    foreach (GameObject g in _instantiatedWalls)
    {
      g.GetComponent<Renderer>().material = _wallMatOrig;
    }
    _instantiatedWalls.Clear();

    foreach (GameObject g in _instantiatedCorners)
    {
      g.GetComponent<Renderer>().material = _cornerMatOrig;
    }
    _instantiatedCorners.Clear();

    Object.Destroy(_cornerReference);
    _cornerReference = null;
    Object.Destroy(_wallReference);
    _wallReference = null;
  } // DestroyPool()
} // EnclosureBuilderPool
