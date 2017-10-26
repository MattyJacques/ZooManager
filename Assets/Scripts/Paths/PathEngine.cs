//Title: PathEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To manage/create paths!

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathEngine : MonoBehaviour {
  public LayerMask _pathLayers;
  public Slider _sizeSlider;
  public GameObject _polePrefab;
  public GameObject _pathUI;

  private bool _buildingPaths = false;
  private Ray _raycastMouse;
  private RaycastHit _rayHit;
  private List<Vector3> _pathPoints = new List<Vector3> ();
  private bool _previouslyConnected = false;
  private List<GameObject> _ghostPath = new List<GameObject> ();
  private float _pathSize = 1;
  private List<GameObject> _previousPaths = new List<GameObject> ();
  private List<GameObject> _previousPathConnectors = new List<GameObject> ();
  private float _pathHeight = 0;

  void Update () {
    if(_buildingPaths){
      _raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
      for (int i = 0; i < _ghostPath.Count; i++) {
        Destroy (_ghostPath[i]);
        _ghostPath.RemoveAt (i);
      }
      if (Physics.Raycast (_raycastMouse, out _rayHit, 1000, _pathLayers)) {
        if(Input.GetMouseButtonDown(0) && !GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().IsPointerOverGameObject()){
          if (_pathPoints.Count == 0) {
            _pathPoints.Add (_rayHit.point + new Vector3(0, _pathHeight, 0));
          } else {
            _pathPoints.Add (_rayHit.point + new Vector3(0, _pathHeight, 0));
            _previousPaths.AddRange(CreatePath (_pathPoints[0], _pathPoints[1], _pathHeight));
            if(_previouslyConnected){
              GameObject pathConnector = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
              pathConnector.name = "Path_Connector";
              pathConnector.transform.position = _pathPoints [0];
              if(_previousPaths[_previousPaths.Count - 2].transform.localScale.x > _pathSize){
                pathConnector.transform.localScale = new Vector3 (_previousPaths[_previousPaths.Count - 2].transform.localScale.x, 0.05f, _previousPaths[_previousPaths.Count - 2].transform.localScale.x);
              }else{
                pathConnector.transform.localScale = new Vector3 (_pathSize, 0.05f, _pathSize);
              }
              _previousPathConnectors.Add (pathConnector);
            }
            _pathPoints.RemoveAt (0);
            _previouslyConnected = true;
          }
        }

        if(Input.GetMouseButtonDown(1)){
          if (_pathPoints.Count == 0) {
            StopBuildingPaths ();
            return;
          } else {
            _pathPoints.RemoveAt (_pathPoints.Count - 1);
            _previouslyConnected = false;
          }
        }

        if (_pathPoints.Count == 1) {
          _ghostPath.AddRange(CreatePath (_pathPoints[0], _rayHit.point + new Vector3(0, _pathHeight, 0), _pathHeight));
        } else {
          _ghostPath.Add((GameObject)Instantiate (_polePrefab, _rayHit.point + new Vector3(0, _pathHeight, 0), Quaternion.identity));
        }

        if(Input.GetKeyDown(KeyCode.KeypadPlus)){
          _pathHeight = _pathHeight + 1;
        }
        if(Input.GetKeyDown(KeyCode.KeypadMinus) && _pathHeight > 0){
          _pathHeight = _pathHeight - 1;
        }
      }
    }
	}

  private List<GameObject> CreatePath(Vector3 point1, Vector3 point2, float pathsHeight){
    List<GameObject> result = new List<GameObject> ();
    Vector3 pos1;
    Vector3 pos2;
    RaycastHit pathHit1;
    RaycastHit pathHit2;
    float pathLength = Vector2.Distance (new Vector2(point1.x, point1.z), new Vector2(point2.x, point2.z));
    for (int i = 0; i < pathLength; i++) {
      if(i == 0){
        pos1 = point1;
      }else{
        pos1 = Vector3.Lerp (point1, point2, 1/pathLength * i);
      }
      pos2 = Vector3.Lerp (point1, point2, Mathf.Min(1/pathLength * (i + 1), 1));
      Physics.Raycast (pos1, Vector3.down, out pathHit1, 1000, _pathLayers);
      Physics.Raycast (pos2, Vector3.down, out pathHit2, 1000, _pathLayers);
      pos1 = new Vector3 (pos1.x, pathHit1.point.y + pathsHeight + 0.1f, pos1.z);
      pos2 = new Vector3 (pos2.x, pathHit2.point.y + pathsHeight + 0.1f, pos2.z);
      result.Add (GameObject.CreatePrimitive(PrimitiveType.Cube));
      result[result.Count - 1].transform.position = Vector3.Lerp(pos1, pos2, 0.5f);
      result[result.Count - 1].transform.localScale = new Vector3 (_pathSize, 0.1f, Vector3.Distance(pos1, pos2));
      result[result.Count - 1].name = "Path";
      result[result.Count - 1].transform.LookAt(pos2);
    }
    return result;
  }

  public void StartBuildingPaths(){
    _buildingPaths = true;
    _previouslyConnected = false;
    _pathSize = 1;
    _pathUI.SetActive (true);
    _pathHeight = 0;
  }

  public void StopBuildingPaths(){
    _buildingPaths = false;
    _previouslyConnected = false;
    _pathSize = 1;
    _pathPoints = new List<Vector3> ();
    for (int i = 0; i < _ghostPath.Count; i++) {
      Destroy (_ghostPath[i]);
      _ghostPath.RemoveAt (i);
    }
    _pathUI.SetActive (false);
    _pathHeight = 0;
  }

  public void TogglePathBuilding(){
    if (_buildingPaths) {
      StopBuildingPaths ();
    } else {
      StartBuildingPaths ();
    }
  }

  public void ChangeSize(){
    if (_buildingPaths) {
      _pathSize = _sizeSlider.value;
    } else {
      _sizeSlider.value = _pathSize;
    }
  }
}
