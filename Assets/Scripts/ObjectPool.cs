using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    private Dictionary<string, List<ObjectPoolObject>> _pool = new Dictionary<string, List<ObjectPoolObject>> ();
    public Vector3 _hiddenObjectsPosition = new Vector3 (-5000, -5000, -5000);
    private string _hiddenObjectsTag = "Hidden";

    public class ObjectPoolObject   //TODO: rename
    {
        public Transform _transform;
        public string _oldTag;  
        public bool _oldActiveState;    //If the object was active or not before storing
        public string _oldName; //The old name of the gameObject
        public Vector3 _oldPosition;    //The position of the object prior to being added to the pool

        public ObjectPoolObject(Transform transform)
        {   //Constructor
            _transform = transform;
            _oldTag = _transform.tag;
            _oldActiveState = transform.gameObject.activeSelf;
            _oldName = _transform.name;
            _oldPosition = _transform.position;
        }

        public ObjectPoolObject(GameObject gameObject)
        {   //Constructor
            _transform = gameObject.transform;
            _oldTag = _transform.tag;
            _oldActiveState = gameObject.activeSelf;
            _oldName = _transform.name;
            _oldPosition = _transform.position;
        }
    }   //ObjectPoolObject

    public enum PoolDestructionMethod
    {
        Destroy_All_GameObjects,
        Return_All_GameObjects,
        Destroy_Stored_And_Return_Unstored_GameObjects
    }

    /// <summary>
    /// THIS WILL NOT WORK! Use ObjectPool.NewObjectPool() instead
    /// </summary>
    public ObjectPool()
    {   //Empty constructor just to edit the summary

    }   //ObjectPool()

    public GameObject GetObject(string GameObjectName)
    {   //Retrieve a object of GameObjectName name from the pool

        if (!_pool.ContainsKey (SanitizeName (GameObjectName)))
        {
            Debug.LogWarning ("Tried getting object using the name " + GameObjectName + " from object pool "
                + name + ", but the object pool contains no entry for that name.");
            return null;
        }
        else if (_pool[SanitizeName (GameObjectName)].Count > 0)
        {
            GameObject g = _pool[SanitizeName (GameObjectName)][0]._transform.gameObject;
            _pool[SanitizeName (GameObjectName)].RemoveAt (0);
            return g;
        }
        else
        {
            GameObject g = Instantiate (_pool[SanitizeName (GameObjectName)][0]._transform.gameObject);
            _pool[SanitizeName (GameObjectName)].Add (new ObjectPoolObject (g));
            return g;
        }
    }   //GetObject()

    public void StoreObject(GameObject objectToStore)
    {   //'stores' the object in the pool, effectively removing it from the scene
        string name = objectToStore.name;
        if (!_pool.ContainsKey (SanitizeName(name)))
        {
            _pool.Add (name, new List<ObjectPoolObject> ());
        }
        _pool[name].Add (new ObjectPoolObject (objectToStore));
    }   //StoreObject()
    
    public static ObjectPool NewObjectPool()
    {   //Instantiates and returns a new ObjectPool
        GameObject g = new GameObject ("ObjectPool" + Time.realtimeSinceStartup.ToString());
        g.AddComponent<ObjectPool> ();
        return g.GetComponent<ObjectPool>();
    }   //NewObjectPool()

    public static ObjectPool NewObjectPool(Vector3 hiddenObjectsPosition)
    {   //Instantiates and returns a new ObjectPool with a custom position for where to store hidden objects
        GameObject g = new GameObject ("ObjectPool" + Time.realtimeSinceStartup.ToString());
        ObjectPool o = g.AddComponent<ObjectPool> ();
        o._hiddenObjectsPosition = hiddenObjectsPosition;
        return o;
    }   //NewObjectPool()

    public List<GameObject> DestroyPool(PoolDestructionMethod method)
    {   //Destroys the pool based on the method given

        //This is instantiated in the case unless the method is Destroy_All_GameObjects, in which case it returns null
        List<GameObject> objectsToReturn = null;

        switch (method) {
            case PoolDestructionMethod.Destroy_All_GameObjects:
                foreach (var v in _pool)
                {
                    for (int i = 0; i < v.Value.Count; i++)
                    {
                        Destroy (v.Value[i]._transform.gameObject);
                    }
                }
                break;

            case PoolDestructionMethod.Return_All_GameObjects:
                objectsToReturn = new List<GameObject> ();
                foreach (KeyValuePair<string, List<ObjectPoolObject>> objList in _pool)
                {
                    objectsToReturn.AddRange (objList.Value.Select (x => x._transform.gameObject));
                }
                break;

            case PoolDestructionMethod.Destroy_Stored_And_Return_Unstored_GameObjects:
                objectsToReturn = new List<GameObject> ();
                foreach (KeyValuePair<string, List<ObjectPoolObject>> objList in _pool)
                {
                    for (int i = objList.Value.Count - 1; i >= 0; i--)
                    {
                        /*
                        if (objList.Value[i]._isStored)
                        {
                            Destroy (objList.Value[i]._transform);
                            //Do we need to remove it as well? I don't think so
                        }
                        else
                        {
                            objectsToReturn.Add (objList.Value[i]._transform.gameObject);
                        }
                        */
                    }
                }
                break;
        }

        //Destroy should be delayed until the end of the frame, so calling destroy and then returning should be safe
        Destroy (this);
        return objectsToReturn;
    }   //DestroyPool()

    private void HideObject(ObjectPoolObject g)
    {
        g._transform.position = _hiddenObjectsPosition;
        g._transform.tag = _hiddenObjectsTag;
    }   //HideObject()

    private void UnhideObject(ObjectPoolObject g)
    {
        g._transform.position = g._oldPosition;
        g._transform.tag = g._oldTag;
        //TODO: name and active state
    }   //UnhideObject

    private string SanitizeName(string name)
    {   //Removes (Clone) from the name if it exists and return it
        if (name.Contains ("(Clone)"))
        {
            return name.Substring (0, name.Length - 7);
        }
        else
        {
            return name;
        }
    }   //SanitizeName()
}
