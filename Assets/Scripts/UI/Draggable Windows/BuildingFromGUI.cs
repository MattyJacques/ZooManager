using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Title        : BuildingFromGUI.cs
// Purpose      : Click on a building button and place the building
// Author       : WeirdGamer
// Date         : 25/05/2017
public class BuildingFromGUI : MonoBehaviour {

  public string buildingID;

  private GameObject engine;
  private GameObject objectToBuild;

  void Awake()
  {
    engine = GameObject.Find("BuildingEngine");
  }
 
  //TODO: This will have to be changed to work with the new manager system
  public void OnClick()
    {
      buildingID = "Buildings/Prefabs/" + buildingID;
      GameObject loadedObject = Resources.Load(buildingID) as GameObject;

      if (loadedObject)
      { // If the object has been loaded, instantiate
        objectToBuild = (GameObject) Instantiate(loadedObject);
      }

      engine.gameObject.GetComponent<BuildingEngine> ().StartBuild (objectToBuild, new Vector3 ( 1, 1, 1 ));
    }
}