using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cubiquity;
// Title        : SpawnWindow.cs
// Purpose      : Spawns a window if it dosn't exist and button is clicked, otherwise destroys the window if it exsists and the button is clicked.
// Author       : aimmmmmmmmm
// Date         : 26/01/2017
public class SpawnWindow : MonoBehaviour {
   
  public GameObject windowPrefab;
  public string type;

  private GameObject window;
  private GameObject UI;
  private Vector3 localScale;

  private Toggle[] toggles;


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

      Color color = window.GetComponent<Image>().color;
      color.a = 1.0f;
      window.GetComponent<Image>().color = color;

      switch ( type )
      {
        case "TerrainTab":
          Debug.Log("Window Type is Terrain");
          TerrainTools terrainTools = window.GetComponent<TerrainTools>();
          TerrainVolume terrainVolume = GameObject.Find("TerrainVolume").GetComponent<TerrainVolume>();
          terrainTools._terrainVolume = terrainVolume;
          GameObject.Find("TerrainTab(Clone)").GetComponent<TerrainTools>()._sizeScrollbar = window.GetComponentInChildren<Slider>();

          Debug.Log(terrainTools._terrainVolume);

          window.GetComponent<TerrainTools>().StartTerrainEditing();
          break;
         case "EnclosureWindow":
         case "PathWindow":
           window.transform.SetParent(GameObject.Find("BuildTab(Clone)").transform);
           window.transform.position = window.transform.parent.position + new Vector3(50,0,0);
           break;
        case "AnimalTab":
        case "BuildTab":
        case null:
          break;
                   
        default:
          window.GetComponent<PlacementWindow>().buildingType = type;
          window.transform.SetParent(GameObject.Find("BuildTab(Clone)").transform);
          break;
      }    
    } 
    else 
    { 
      Destroy (window); 
      GameObject.Find("TerrainEngine").GetComponent<TerrainTools>().StopTerrainEditing();
    }
  }
}
