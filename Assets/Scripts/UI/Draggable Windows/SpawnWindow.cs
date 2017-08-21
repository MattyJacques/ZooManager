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
  private GameObject UI;
  private Vector3 localScale;


  public void OnClick()
  {
    if (window == null) 
    { 
      UI = GameObject.Find("SideButtons");
      localScale = UI.transform.localScale;
      window = (GameObject)Instantiate (windowPrefab); 
      window.transform.SetParent(GameObject.Find("SideButtons").transform);
      window.transform.localScale = localScale;
      window.transform.localPosition = windowPrefab.transform.position;
    } 
    else { Destroy (window); }
  }
}
