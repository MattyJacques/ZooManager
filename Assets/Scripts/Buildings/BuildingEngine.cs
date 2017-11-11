//Title: BuildingEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/28/2017
//Purpose: To build an Object, render building object UI, and destroy an object.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingEngine : MonoBehaviour
{
  public LayerMask _buildingLayers;
  public GameObject _gridLight;
  public GameObject _ghostVisPrefab;
  public Material _ghostVisSquare;
  public Material _ghostVisTriangle;
  public GameObject _buildingUI;
  public Texture _gridMaterial;
  public Texture _triangleMaterial;
  public GameObject _smokePrefab;

  private bool _building = false;
  private GameObject _gameObjectToBuild;
  private Vector3 _gameObjectToBuildSize;
  private Ray _raycastMouse;
  private RaycastHit _rayHit;
  private GameObject _ghostGameObject;
  private GameObject _ghostVis;
  private int _buildingMode = 0;
  private float _gridSize = 1;
  private Vector3 _positionToBuild = new Vector3 (0, 0, 0);
  private List<BoxCollider> _previousBuildingColliders = new List<BoxCollider> ();
  private List<GameObject> _previousBuildings = new List<GameObject> ();

  public void StartBuild (GameObject gameObjectBuild, Vector3 buildingSize)
  {
    _building = true;
    _gameObjectToBuild = gameObjectBuild;
    _buildingMode = 0;
    _gridSize = 1;
    _gameObjectToBuildSize = buildingSize;
    _buildingUI.SetActive (true);
    _gridLight.GetComponent<Light> ().cookieSize = 1;
    _ghostVis = (GameObject)Instantiate (_ghostVisPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
    _ghostVis.transform.Rotate (new Vector3 (90, 0, 0));
    _ghostVis.transform.localScale = new Vector3 (_gameObjectToBuildSize.x, _gameObjectToBuildSize.z, 1);
  }

  public void StopBuild ()
  {
    _building = false;
    _gameObjectToBuild = null;
    _buildingMode = 0;
    _gridSize = 1;
    Destroy (_ghostGameObject);
    _buildingUI.SetActive (false);
    Destroy (_ghostVis);
    _gridLight.SetActive (false);
  }

  void Update ()
  {
    if (_building) {
      _raycastMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
      if (Input.GetKeyDown(Assets.Scripts.GameSettings.ZooManagerGameSettings.TOGGLE_GRID))
      {
        ToggleBuildMode();
      }

      if (Physics.Raycast (_raycastMouse, out _rayHit, 1000, _buildingLayers)) {

        //GridBased
        if (_buildingMode == 1 && !Input.GetKey (Assets.Scripts.GameSettings.ZooManagerGameSettings.TOGGLE_ROTATE)) {
          _positionToBuild = new Vector3 (Mathf.RoundToInt (_rayHit.point.x * 1 / _gridSize) * _gridSize, _rayHit.point.y, Mathf.RoundToInt (_rayHit.point.z * 1 / _gridSize) * _gridSize);
          _ghostVis.transform.position = _positionToBuild + new Vector3 (0, 0.02f, 0);
          //TriangleBased
        } else if (_buildingMode == 2 && !Input.GetKey (Assets.Scripts.GameSettings.ZooManagerGameSettings.TOGGLE_ROTATE)) {
          _positionToBuild = new Vector3 (Mathf.RoundToInt (_rayHit.point.x * 1 / _gridSize) * _gridSize, _rayHit.point.y, Mathf.RoundToInt (_rayHit.point.z * 1 / _gridSize) * _gridSize);
          _ghostVis.transform.position = _positionToBuild + new Vector3 (0, 0.02f, 0);
          Vector3 trianglePos1 = _positionToBuild + new Vector3 (0.25f * _gridSize, 0, 0);
          Vector3 trianglePos2 = _positionToBuild - new Vector3 (0.25f * _gridSize, 0, 0);
          Vector3 trianglePos3 = _positionToBuild + new Vector3 (0, 0, 0.25f * _gridSize);
          Vector3 trianglePos4 = _positionToBuild - new Vector3 (0, 0, 0.25f * _gridSize);
          float triangle1Dist = Vector3.Distance (trianglePos1, _rayHit.point);
          float triangle2Dist = Vector3.Distance (trianglePos2, _rayHit.point);
          float triangle3Dist = Vector3.Distance (trianglePos3, _rayHit.point);
          float triangle4Dist = Vector3.Distance (trianglePos4, _rayHit.point);
          float closestDistance = Mathf.Min (new float[]{ triangle1Dist, triangle2Dist, triangle3Dist, triangle4Dist });
          if (closestDistance == triangle1Dist) {
            _positionToBuild = trianglePos1;
            _ghostVis.transform.eulerAngles = new Vector3 (90, 90, 0);
          } else if (closestDistance == triangle2Dist) {
            _positionToBuild = trianglePos2;
            _ghostVis.transform.eulerAngles = new Vector3 (90, 270, 0);
          } else if (closestDistance == triangle3Dist) {
            _positionToBuild = trianglePos3;
            _ghostVis.transform.eulerAngles = new Vector3 (90, 0, 0);
          } else if (closestDistance == triangle4Dist) {
            _positionToBuild = trianglePos4;
            _ghostVis.transform.eulerAngles = new Vector3 (90, 180, 0);
          }
        } else if (!Input.GetKey (Assets.Scripts.GameSettings.ZooManagerGameSettings.TOGGLE_ROTATE)) {
          _positionToBuild = new Vector3 (_rayHit.point.x, _rayHit.point.y, _rayHit.point.z);
          _ghostVis.transform.position = _positionToBuild + new Vector3 (0, 0.02f, 0);
        }

        if (_ghostGameObject == null) {
          _ghostGameObject = (GameObject)Instantiate (_gameObjectToBuild, _positionToBuild + new Vector3 (0, 5, 0), Quaternion.identity);
        } else {
          _ghostGameObject.transform.position = Vector3.MoveTowards (_ghostGameObject.transform.position, _positionToBuild + new Vector3 (0, 5, 0), 5);
        }

        if (Input.GetKey(Assets.Scripts.GameSettings.ZooManagerGameSettings.TOGGLE_ROTATE)) {
          _ghostGameObject.transform.LookAt (new Vector3 (_rayHit.point.x, _positionToBuild.y + 5, _rayHit.point.z));
        }

        if (Input.GetMouseButtonDown (0) && !GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().IsPointerOverGameObject ()) {
          if (!CheckIfOccupied (_positionToBuild, _gameObjectToBuildSize)) {
            _previousBuildings.Add ((GameObject)Instantiate(_gameObjectToBuild, _positionToBuild, _ghostGameObject.transform.rotation));
            GameObject tmpBoundingBox = new GameObject ();
            tmpBoundingBox.AddComponent<BoxCollider> ().size = _gameObjectToBuildSize;
            tmpBoundingBox.transform.position = _positionToBuild;
            tmpBoundingBox.name = "Building Collider";
            _previousBuildingColliders.Add (tmpBoundingBox.GetComponent<BoxCollider> ());
            ((GameObject)Instantiate (_smokePrefab, _positionToBuild, Quaternion.identity)).transform.Rotate (new Vector3 (90, 0, 0));
            if (!Input.GetKey (KeyCode.LeftShift)) {
              StopBuild ();
            }
          } else {
            //TODO: Add Failed To Build Sound!
          }
        }

        if (Input.GetMouseButtonDown (1)) {
          StopBuild ();
        }
      }
    }
  }

  public bool CheckIfOccupied (Vector3 position, Vector3 size)
  {
    GameObject tmpCollider = new GameObject ();
    tmpCollider.AddComponent<BoxCollider> ().size = size;
    tmpCollider.transform.position = position;
    for (int i = 0; i < _previousBuildingColliders.Count; i++) {
      if (tmpCollider.GetComponent<BoxCollider> ().bounds.Intersects (_previousBuildingColliders [i].bounds)) {
        Destroy (tmpCollider);
        return true;
      }
    }
    Destroy (tmpCollider);
    return false;
  }

  public void IncreaseGrid ()
  {
    if (_building && _buildingMode != 0) {
      _gridSize = _gridSize * 2;
      _gridLight.GetComponent<Light> ().cookieSize = _gridSize;
      _ghostVis.transform.localScale = new Vector3 (_gridSize, _gridSize, 1);
    }
  }

  public void DecreaseGrid ()
  {
    if (_building && _buildingMode != 0) {
      _gridSize = _gridSize / 2;
      _ghostVis.transform.localScale = new Vector3 (_gridSize, _gridSize, 1);
      _gridLight.GetComponent<Light> ().cookieSize = _gridSize;
    }
  }

  public void NormalMode ()
  {
    if (_building) {
      _buildingMode = 0;
      _gridLight.SetActive (false);
      _ghostVis.GetComponent<Renderer> ().material = _ghostVisSquare;
      _ghostVis.transform.localScale = new Vector3 (_gameObjectToBuildSize.x, _gameObjectToBuildSize.z, 1);
    }
  }

  public void GridMode ()
  {
    if (_building) {
      _buildingMode = 1;
      _ghostVis.GetComponent<Renderer> ().material = _ghostVisSquare;
      _gridLight.GetComponent<Light> ().cookie = _gridMaterial;
      _gridLight.SetActive (true);
      _ghostVis.transform.localScale = new Vector3 (_gridSize, _gridSize, 1);
    }
  }

  public void TriangleMode ()
  {
    if (_building) {
      _buildingMode = 2;
      _gridLight.SetActive (true);
      _gridLight.GetComponent<Light> ().cookie = _triangleMaterial;
      _ghostVis.GetComponent<Renderer> ().material = _ghostVisTriangle;
      _ghostVis.transform.localScale = new Vector3 (_gridSize, _gridSize, 1);
    }
  }

  public bool DestroyBuilding (GameObject buildingToDestroy)
  {
    for (int i = 0; i < _previousBuildings.Count; i++) {
      if (buildingToDestroy == _previousBuildings [i]) {
        Destroy (_previousBuildings [i]);
        Destroy (_previousBuildingColliders [i]);
        return true;
      }
    }
    return false;
  }

  public void ToggleBuildMode()
  {
    if (_buildingMode == 0) { _buildingMode = 1; }
    else { _buildingMode = 0; }
  }
}
