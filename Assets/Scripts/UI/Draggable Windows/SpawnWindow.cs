using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWindow : MonoBehaviour {
  public GameObject windowPrefab;
  private GameObject window;
  public void OnClick()
  {
    if (window == null) { window = (GameObject)Instantiate (windowPrefab, new Vector3 (0, 0, 0), Quaternion.identity); } 
    else { Destroy (window); }
  }
}
