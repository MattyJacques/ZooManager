using System.Collections.Generic;
using UnityEngine;

public class EnclosureBuilderTemplate
{
    public GameObject _wallReference;
    public GameObject _cornerReference;
    public enum ObjectType { Wall, Corner };
    
    private Stack<GameObject> _pooledWalls = new Stack<GameObject> ();
    private List<GameObject> _instantiatedWalls = new List<GameObject> (); //TODO: track with bool instead
    private Stack<GameObject> _pooledCorners = new Stack<GameObject> ();
    private List<GameObject> _instantiatedCorners = new List<GameObject> ();
    private Vector3 _hiddenPos = new Vector3 (-5000, -5000, -5000);

    public void Initialize(GameObject wall, GameObject corner)
    {
        InitializeNewGameObject (ObjectType.Wall, wall);
        InitializeNewGameObject (ObjectType.Corner, corner);
    }

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

    private void InitializeNewGameObject(ObjectType type, GameObject newGameObject)
    {
        Debug.Log ("Initializing new gameobject of prefabType " + UnityEditor.PrefabUtility.GetPrefabType (newGameObject).ToString ()
            + ", and object type of " + type.ToString ());

        if (UnityEditor.PrefabUtility.GetPrefabType (newGameObject) != UnityEditor.PrefabType.None) //Does this work?
        {
            newGameObject = Object.Instantiate (newGameObject);
        }

        newGameObject.transform.position = _hiddenPos;

        switch (type) {
            case ObjectType.Wall:
                _wallReference = newGameObject;
                _pooledWalls.Push (Object.Instantiate (_wallReference));
                break;

            case ObjectType.Corner:
                _cornerReference = newGameObject;
                _pooledCorners.Push (Object.Instantiate (_cornerReference));
                break;
        }
    }

    public GameObject Instantiate(ObjectType type)
    {
        GameObject g = null;
        switch (type) {
            case ObjectType.Wall:
                if (_pooledWalls.Count < 1)
                {
                    g = Object.Instantiate (_wallReference);
                }
                else
                {
                    g = _pooledWalls.Pop ();
                }
                g.transform.rotation = _wallReference.transform.rotation;
                _instantiatedWalls.Add (g);
                break;

            case ObjectType.Corner:
                if (_pooledCorners.Count < 1)
                {
                    g = Object.Instantiate (_cornerReference);
                }
                else
                {
                    g = _pooledCorners.Pop ();
                }
                g.transform.rotation = _cornerReference.transform.rotation;
                _instantiatedCorners.Add (g);
                break;
        }

        return g;
    }

    public GameObject[] InstantiateMultiple(ObjectType type, int amount)
    {
        List<GameObject> objectsToReturn = new List<GameObject> ();
        for (int i = 0; i < amount; i++) {
            objectsToReturn.Add (Instantiate (type));
        }

        return objectsToReturn.ToArray();
    }

    public void Destroy (ObjectType type, GameObject g)
    {
        switch (type) {
            case ObjectType.Wall:
                _instantiatedWalls.Remove (g);
                _pooledWalls.Push (g);
                break;

            case ObjectType.Corner:
                _instantiatedCorners.Remove (g);
                _pooledCorners.Push (g);
                break;
        }

        g.transform.position = _hiddenPos;
    }

    private void DestroyGameObjectsInCollection(IEnumerable<GameObject> col)
    {
        foreach (GameObject g in col)
        {
            Object.Destroy (g);
        }
    }

    public void DestroyAllHiddenObjects()
    {
        foreach (GameObject g in _pooledCorners)
        {
            Object.Destroy (g);
        }
        _pooledCorners.Clear ();

        foreach (GameObject g in _pooledWalls)
        {
            Object.Destroy (g);
        }
        _pooledWalls.Clear ();

        Object.Destroy (_cornerReference);
        Object.Destroy (_wallReference);
    }
}
