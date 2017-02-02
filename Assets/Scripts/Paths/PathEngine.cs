using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Title: PathEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To manage/create paths!
public class PathEngine : MonoBehaviour {
  public LayerMask pathLayers;
  public Slider sizeSlider;
  public GameObject polePrefab;
  public GameObject pathUI;

  private bool buildingPaths = false;
  private Ray raycastMouse;
  private RaycastHit rayHit;
  private List<Vector3> pathPoints = new List<Vector3> ();
  private GameObject ghostPath;
  private float pathSize = 1;
	// Update is called once per frame
	void Update () {
    if(buildingPaths){
      raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
      Destroy (ghostPath);
      if (Physics.Raycast (raycastMouse, out rayHit, 1000, pathLayers)) {
        if(Input.GetMouseButtonDown(0) && !GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().IsPointerOverGameObject()){
          if (pathPoints.Count == 0) {
            pathPoints.Add (rayHit.point);
          } else {
            pathPoints.Add (rayHit.point);
            float distanceBetween = Vector2.Distance (new Vector2(pathPoints[0].x, pathPoints[0].z), new Vector2(pathPoints[1].x, pathPoints[1].z));
            CreatePath (Vector3.Lerp(pathPoints[0], pathPoints[1], 0.5f), new Vector3(pathSize, 0.1f, distanceBetween), pathPoints[1]);
            pathPoints.RemoveAt (0);
          }
        }

        if(Input.GetMouseButtonDown(1)){
          if (pathPoints.Count == 0) {
            StopBuildingPaths ();
          } else {
            pathPoints.RemoveAt (pathPoints.Count - 1);
          }
        }

        if (pathPoints.Count == 1) {
          float distanceBetween = Vector2.Distance (new Vector2(pathPoints[0].x, pathPoints[0].z), new Vector2(rayHit.point.x, rayHit.point.z));
          ghostPath = CreatePath (Vector3.Lerp(pathPoints[0], rayHit.point, 0.5f), new Vector3(pathSize, 0.1f, distanceBetween), rayHit.point);
        } else {
          ghostPath = (GameObject)Instantiate (polePrefab, rayHit.point, Quaternion.identity);
        }
      }
    }
	}

  private GameObject CreatePath(Vector3 pathPosition, Vector3 pathSize, Vector3 lookAt){
    GameObject result = GameObject.CreatePrimitive (PrimitiveType.Cube);
    result.name = "Path";
    result.transform.position = pathPosition;
    result.transform.localScale = pathSize;
    result.transform.LookAt (lookAt);
    return result;
  }

  public void StartBuildingPaths(){
    buildingPaths = true;
    pathSize = 1;
    pathUI.SetActive (true);
  }

  public void StopBuildingPaths(){
    buildingPaths = false;
    pathSize = 1;
    pathPoints = new List<Vector3> ();
    Destroy (ghostPath);
    pathUI.SetActive (false);
  }

  public void TogglePathBuilding(){
    if (buildingPaths) {
      StopBuildingPaths ();
    } else {
      StartBuildingPaths ();
    }
  }

  public void ChangeSize(){
    if (buildingPaths) {
      pathSize = sizeSlider.value;
    } else {
      sizeSlider.value = pathSize;
    }
  }
}
