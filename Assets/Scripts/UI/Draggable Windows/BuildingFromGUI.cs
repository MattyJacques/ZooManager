using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Title        : BuildingFromGUI.cs
// Purpose      : Lets you click on an animal in the GUI and spawn it
// Author       : WeirdGamer
// Date         : 25/05/2017
public class BuildingFromGUI : MonoBehaviour {

  public string buildingName;

  private GameObject engine;

  private GameObject prefab;
  private GameObject objectToBuild;

  void Awake()
  {
    engine = GameObject.Find("BuildingEngine");
  }
 
  public void OnClick()
    {

      buildingName = "Buildings/Prefabs/" + buildingName;
      GameObject loadedObject = Resources.Load(buildingName) as GameObject;

      if (loadedObject)
      { // If the object has been loaded, instantiate
        objectToBuild = (GameObject) Instantiate(loadedObject);
      }

      engine.gameObject.GetComponent<BuildingEngine> ().StartBuild (objectToBuild, new Vector3 ( 1, 1, 1 ));
    }
}