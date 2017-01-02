using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    private Dictionary<string, ObjectPoolStack> _pool = new Dictionary<string, ObjectPoolStack> ();
    public Vector3 _hidePosition = new Vector3 (-5000, -5000, -5000);

    public class ObjectPoolStack
    {   //Custom stack class for storing the gameObjects, that keeps a reference to the original object and object tag
        private Stack<GameObject> _stack;
        private GameObject _originalObject;
        private string _tag = "pooledObject";
        private string _origTag;
        private Transform _parent;

        public ObjectPoolStack(GameObject gameObjectToStore, Transform parent)
        {   //Constructor
            _stack = new Stack<GameObject> ();
            _origTag = gameObjectToStore.tag;
            _parent = parent;
            if (UnityEditor.PrefabUtility.GetPrefabType (gameObjectToStore) != UnityEditor.PrefabType.None)
            {   //If gameObjectToStore is a prefab, instantiate it first
                _originalObject = Instantiate (gameObjectToStore);
                _stack.Push (_originalObject);
            }
            else
            {
                _originalObject = gameObjectToStore;
                _stack.Push (_originalObject);
            }
        }   //ObjectPoolStack()

        public GameObject Pop()
        {   //Pops from the stack, and if stack is empty instantiates and returns new object
            if (_stack.Count > 0)
            {
                GameObject g = _stack.Pop ();
                g.transform.parent = null;
                g.tag = _origTag;
                return g;
            }
            else
            {
                Debug.Log ("Creating new obj");
                GameObject g = Instantiate (_originalObject);
                return g;
            }
        }   //Pop()

        public void Push(GameObject g)
        {   //Adds object to the stack
            g.tag = _tag;
            if (_parent != null)
            {
                g.transform.parent = _parent;
            }
            _stack.Push (g);
        }   //Push()

        public GameObject[] ToArray()
        {   //Exposes ToArray method
            return _stack.ToArray ();
        }   //ToArray()

        public void DestroyObjectsInStack()
        {   //Destroys all the objects in the stack
            foreach (GameObject g in _stack)
            {
                Destroy (g);
            }
        }   //DestroyObjectsInStack()

        public int Count()
        {   //Exposes Count method
            return _stack.Count;
        }   //Count()

    }   //ObjectPoolStack

    public static ObjectPool NewObjectPool()
    {   //Constructor for this class
        GameObject g = new GameObject ("ObjectPool - " + Time.realtimeSinceStartup);
        ObjectPool o = g.AddComponent<ObjectPool> ();
        return o;
    }   //NewObjectPool

    public GameObject GetObject(string ObjectName)
    {   //Returns GameObject from pool by name
        string sanitizedName = SanitizeName (ObjectName);

        if (_pool.ContainsKey (sanitizedName))
        {
           return _pool[sanitizedName].Pop ();
        }
        else
        {
            Debug.LogError (name + " does not contain GameObject " + ObjectName + ", store it first before getting it!");
            return null;
        }

    }   //GetObject()

    public void StoreObject(GameObject g)
    {   //Hides the GameObject (instead of destroying it)
        string sanitizedName = SanitizeName (g.name);
        if (_pool.ContainsKey (sanitizedName))
        {
            g.transform.position = _hidePosition;
            _pool[sanitizedName].Push (g);
            Debug.Log ("Stored new object " + sanitizedName + ", ObjectPoolStack now contains " + _pool[sanitizedName].Count ().ToString () + " objects");
        }
        else
        {
            Debug.LogWarning (name + " does not contain a ObjectPoolStack that for \"" + sanitizedName
                + "\", use InitializeNewObjectPoolStack to initialize a new ObjectPoolStack before storing an object.");
        }
    }   //StoreObject

    public void InitializeNewObjectPoolStack(string name, GameObject g)
    {   //Initializes a new ObjectPoolStack
        string sanitizedName = SanitizeName(name);
        _pool.Add (sanitizedName, new ObjectPoolStack (g, transform));
    }   //InitializeNewObjectPoolStack()

    public GameObject[] GetAllObjectsOfName(string name)
    {   //Returns all the objects in a ObjectPoolStack
        string sanitizeName = SanitizeName (name);
        if (_pool.ContainsKey (sanitizeName))
        {
            GameObject[] gameObjects = _pool[sanitizeName].ToArray ();
            _pool.Remove (sanitizeName);
            return gameObjects;
        }
        else
        {
            Debug.LogError (name + " tried getting all objects of name " + sanitizeName + ", but pool has no ObjectPoolStack with that name.");
            return null;
        }
    }   //GetAllObjectsOfName()

    public GameObject[] GetAllObjects()
    {   //Returns ALL the objects from ALL the managed ObjectPoolStacks with no special sorting
        if (_pool.Count > 0)
        {
            List<GameObject> allStoredObjects = new List<GameObject> ();
            foreach (KeyValuePair<string, ObjectPoolStack> stack in _pool)
            {
                allStoredObjects.AddRange (stack.Value.ToArray());
            }
            _pool.Clear ();
            return allStoredObjects.ToArray();
        }
        else
        {
            Debug.LogError (name + " contains no ObjectPoolStacks, and can't return any objects.");
            return null;
        }
    }   //GetAllObjects

    public void DestroyObjectPoolStack(string name)
    {   //Destroys all the gameObjects in a stack and removes the stack
        Debug.Log ("dest stack " + name);
        string sanitizedName = name;
        if (_pool.ContainsKey (sanitizedName))
        {
            _pool[sanitizedName].DestroyObjectsInStack ();
            _pool.Remove (sanitizedName);
        }
        else
        {
            Debug.LogWarning (name + " tried destroying ObjectPoolStack by name " + sanitizedName + ", which does not exist");
        }
    }   //DestroyObjectPoolStack

    public void DestroyObjectPool()
    {   //Destroys the object pool and any objects still managed by the pool
        Debug.Log ("dest pool");
        foreach (KeyValuePair<string, ObjectPoolStack> stack in _pool)
        {
            stack.Value.DestroyObjectsInStack ();
        }
        Destroy (this);
    }   //DestroyObjectPool()

    private string SanitizeName(string name)
    {   //Removes (Clone) from name
        if (name.Contains ("(Clone)"))
        {
            return name.Replace ("(Clone)", string.Empty);
        }
        else
        {
            return name;
        }
    }   //SanitizeName()
}
