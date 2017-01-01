using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_objectPoolTest_DeleteMe : MonoBehaviour {
    public GameObject _originalObject;
    public ObjectPool _pool;
    public List<GameObject> gameObjects = new List<GameObject> ();

    void Start()
    {
        _pool = ObjectPool.NewObjectPool ();
        _pool.StoreObject (_originalObject);
    }

	void Update()
    {
        if (Input.GetMouseButton (0))
        {
            if (gameObjects.Count < 50)
            {
                gameObjects.Add(_pool.GetObject (_originalObject.name));
            }

            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit rayHit = new RaycastHit ();
            if (Physics.Raycast (ray, out rayHit))
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].transform.position = rayHit.point + new Vector3 (i * 1, 0);
                }
            }
        }

        if (Input.GetMouseButtonDown (1))
        {
            foreach (GameObject g in gameObjects)
            {
                _pool.StoreObject (g);
            }
        }
    }
}
