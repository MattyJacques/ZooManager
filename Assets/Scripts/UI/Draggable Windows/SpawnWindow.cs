using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Title        : SpawnWindow.cs
// Purpose      : Spawns a window if it dosn't exist and button is clicked, otherwise destroys the window if it exsists and the button is clicked.
// Author       : aimmmmmmmmm
// Date         : 26/01/2017
public class SpawnWindow : MonoBehaviour {
  public GameObject windowPrefab;
  private GameObject window;
  public void OnClick()
  {
    if (window == null) { window = (GameObject)Instantiate (windowPrefab, new Vector3 (0, 0, 0), Quaternion.identity); } 
    else { Destroy (window); }
  }
}
