using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPManager : MonoBehaviour 
{


  private static IPManager _instance;

  public static IPManager Instance
  {
    get
    {
      if(_instance == null)
      {
        _instance = new GameObject("IPManager").AddComponent<IPManager>();
      }

      return _instance;
    }
  }


  public Vector3 GetRandomIP()
  {
    List<Vector3> ips = new List<Vector3>();

    GameObject[] gos = GameObject.FindGameObjectsWithTag("InterestPoint");

    if(gos.Length == 0)
      return Vector3.zero;

    foreach(GameObject go in gos)
    {
        ips.Add(go.transform.position);
    }

    return ips[Random.Range(0, ips.Count - 1)];

  }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        // Search for interest points
        // and create them

	}
}
