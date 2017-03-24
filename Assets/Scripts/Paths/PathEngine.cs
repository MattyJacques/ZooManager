//Title: PathEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To manage/create paths!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cubiquity;

public class PathEngine : MonoBehaviour {
  public LayerMask _pathLayers;
  public Slider _sizeSlider;
  public GameObject _polePrefab;
  public GameObject _pathUI;
  public TerrainVolume _terrainVolume;
  public Slider _gridSlider;
  public GameObject _gridLight;
  public GameObject _stairPrefab;
  public Material _concreteMaterial;

  private bool _usingGrid = false;
  private bool _buildingPaths = false;
  private Ray _raycastMouse;
  private RaycastHit _rayHit;
  private List<Vector3> _pathPoints = new List<Vector3> ();
  private bool _previouslyConnected = false;
  private List<GameObject> _ghostPath = new List<GameObject>();
  private float _pathSize = 1;
  private List<GameObject> _previousPaths = new List<GameObject> ();
  private List<GameObject> _previousPathConnectors = new List<GameObject> ();
  private float _pathHeight = 0;
  private float _gridSize = 1;
  private Vector3 _positionToPath;
  private GameObject _selectedPath;
  private Vector3 _previousSelectedPathPos1;

  void Update () {
    if(_buildingPaths){
      _raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
      for (int i = 0; i < _ghostPath.Count; i++) {
        Destroy (_ghostPath [i]);
      }
      while(_ghostPath.Count > 0){
        _ghostPath.RemoveAt (0);
      }
      if (Physics.Raycast (_raycastMouse, out _rayHit, 1000, _pathLayers)) {
        if (_usingGrid) {
          _positionToPath = new Vector3 (Mathf.RoundToInt (_rayHit.point.x * 1 / _gridSize) * _gridSize, _rayHit.point.y, Mathf.RoundToInt (_rayHit.point.z * 1 / _gridSize) * _gridSize);
        } else {
          _positionToPath = _rayHit.point;
        }
        if(Input.GetMouseButtonDown(0) && !GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().IsPointerOverGameObject()){
          if (_pathPoints.Count == 0) {
            _pathPoints.Add (_positionToPath + new Vector3(0, _pathHeight + 0.1f, 0));
          } else if(CheckValidPath(_pathPoints[0], _positionToPath + new Vector3(0, _pathHeight, 0), _pathHeight)){
            if(_selectedPath == null){
              _pathPoints.Add (_positionToPath + new Vector3(0, _pathHeight, 0));
              _previousPaths.Add(CreatePathMesh (_pathPoints[0], _pathPoints[1], _pathSize, _pathHeight, _concreteMaterial, false));
              _selectedPath = _previousPaths[_previousPaths.Count - 1];
              _previousSelectedPathPos1 = _pathPoints [0];
              _pathPoints[1] = Vector3.MoveTowards(_pathPoints[1], _pathPoints[0], -0.5f);
              _pathPoints.RemoveAt (0);
              _pathPoints.RemoveAt (0);
              //_previouslyConnected = true;
            }else{
              _pathPoints.Add (_positionToPath + new Vector3(0, _pathHeight, 0));
              _previousPaths.AddRange(AddOnPath (_previousSelectedPathPos1, _pathPoints[0], _pathPoints[1], _pathSize, _pathHeight, _concreteMaterial, false));
              _selectedPath = _previousPaths[_previousPaths.Count - 1];
              _previousSelectedPathPos1 = _pathPoints [0];
              _pathPoints[1] = Vector3.MoveTowards(_pathPoints[1], _pathPoints[0], -0.5f);
              _pathPoints.RemoveAt (0);
            }
          }else{
            //TODO: Failed Build Sound
            Debug.Log("Failed Path Build!");
          }
        }

        if(Input.GetMouseButtonDown(1)){
          if (_pathPoints.Count == 0) {
            StopBuildingPaths ();
            return;
          } else {
            _pathPoints.RemoveAt (_pathPoints.Count - 1);
            _previouslyConnected = false;
            _selectedPath = null;
            _previousSelectedPathPos1 = new Vector3();
          }
        }

        if (_pathPoints.Count == 1) {
          if(_selectedPath != null){
            _ghostPath.AddRange(AddOnPath (_previousSelectedPathPos1, _pathPoints[0], _positionToPath + new Vector3(0, _pathHeight + 0.1f, 0), _pathSize, _pathHeight, _concreteMaterial, true));
          }else{
            _ghostPath.Add(CreatePathMesh (_pathPoints[0], _positionToPath + new Vector3(0, _pathHeight + 0.1f, 0), _pathSize, _pathHeight, _concreteMaterial, true));
          }
          if(!CheckValidPath(_pathPoints[0], _positionToPath, _pathHeight)){
            for (int q = 0; q < _ghostPath.Count; q++) {
              _ghostPath[q].GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0);
              MeshRenderer[] ghostPathChildren = _ghostPath[q].GetComponentsInChildren<MeshRenderer> ();
              for (int i = 0; i < ghostPathChildren.Length; i++) {
                ghostPathChildren [i].material.color = new Color (1, 0, 0);
              }
            }
          }
        } else {
          _ghostPath.Add((GameObject)Instantiate (_polePrefab, _positionToPath + new Vector3(0, _pathHeight, 0), Quaternion.identity));
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

  private bool CheckValidPath(Vector3 point1, Vector3 point2, float pathHeight){
    float pathLength = Vector2.Distance (new Vector2(point1.x, point1.z), new Vector2(point2.x, point2.z));
    float pathMHeight = Mathf.Max (point1.y, point2.y);
    Vector3 pos1 = point1;
    Vector3 pos2 = point1;
    RaycastHit pathHit;
    float angle;
    for (int i = 1; i < pathLength + 1; i++) {
      pos1 = Vector3.Lerp (point1, point2, Mathf.Min(1/pathLength * i, 1));
      pos1.y = pathMHeight;

      Physics.Raycast (pos1, Vector3.down, out pathHit, 1000, _pathLayers);
      pos1.y = pathHit.point.y + pathHeight;
      angle = GetHeightAngle (pos1, pos2);
      if(angle > 45 || angle < -45){
        return false;
      }
      pos2 = pos1;
    }
    return true;
  }

  private float GetHeightAngle(Vector3 pos1, Vector3 pos2){
    return Mathf.Atan2(Mathf.Abs(pos1.y - pos2.y), Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z))) * 180/Mathf.PI;
  }

  private float GetPathAngle(Vector3 pos1, Vector3 pos2){
    return Mathf.Atan2(pos1.z - pos2.z, pos1.x - pos2.x) * 180/Mathf.PI;
  }

  private bool CheckValidAngle(Vector3 pos1, Vector3 pos2){
    float angle = GetPathAngle(pos1, pos2);
    //if(angle > 270 || angle < 45){
      //return false;
    //}
    return true;
  }

  private GameObject CreatePathMesh(Vector3 point1, Vector3 point2, float pathWidth, float pathHeight, Material pathMaterial, bool ghost){
    GameObject result = new GameObject();
    GameObject resultLeft = new GameObject ();
    GameObject resultRight = new GameObject ();
    GameObject resultFront = new GameObject ();
    GameObject resultBack = new GameObject ();
   
    result.name = "Path";

    resultLeft.name = "Path_Left";
    resultLeft.transform.parent = result.transform;

    resultRight.name = "Path_Right";
    resultRight.transform.parent = result.transform;

    resultFront.name = "Path_Front";
    resultFront.transform.parent = result.transform;

    resultBack.name = "Path_Back";
    resultBack.transform.parent = result.transform;

    pathHeight = pathHeight + 0.1f;
    float pathWidthH = pathWidth / 2;
    float pathWidthHP = pathWidthH + pathWidth;
    float pathMHeight = Mathf.Max (point1.y, point2.y) + 1;
    if(point1 == point2){
      return result;
    }

    Mesh resultMesh = new Mesh ();
    Mesh resultLeftMesh = new Mesh ();
    Mesh resultRightMesh = new Mesh ();
    Mesh resultFrontMesh = new Mesh ();
    Mesh resultBackMesh = new Mesh ();

    List<Vector3> resultMeshVerticies = new List<Vector3> ();
    List<Vector3> resultRightMeshVerticies = new List<Vector3> ();
    List<Vector3> resultLeftMeshVerticies = new List<Vector3> ();
    List<Vector3> resultFrontMeshVerticies = new List<Vector3> ();
    List<Vector3> resultBackMeshVerticies = new List<Vector3> ();

    List<Vector2> resultMeshUVs = new List<Vector2> ();
    List<Vector2> resultLeftMeshUVs = new List<Vector2> ();
    List<Vector2> resultRightMeshUVs = new List<Vector2> ();
    List<Vector2> resultFrontMeshUVs = new List<Vector2> ();
    List<Vector2> resultBackMeshUVs = new List<Vector2> ();

    List<int> resultMeshTriangles = new List<int> ();
    List<int> resultLeftMeshTriangles = new List<int> ();
    List<int> resultRightMeshTriangles = new List<int> ();
    List<int> resultFrontMeshTriangles = new List<int> ();
    List<int> resultBackMeshTriangles = new List<int> ();

    result.AddComponent<MeshFilter> ().mesh = resultMesh;
    result.AddComponent<MeshRenderer> ().material = pathMaterial;
    result.AddComponent<MeshCollider> ();

    resultLeft.AddComponent<MeshFilter> ().mesh = resultLeftMesh;
    resultLeft.AddComponent<MeshRenderer> ().material = pathMaterial;
    resultLeft.AddComponent<MeshCollider> ();

    resultRight.AddComponent<MeshFilter> ().mesh = resultRightMesh;
    resultRight.AddComponent<MeshRenderer> ().material = pathMaterial;
    resultRight.AddComponent<MeshCollider> ();

    resultBack.AddComponent<MeshFilter> ().mesh = resultBackMesh;
    resultBack.AddComponent<MeshRenderer> ().material = pathMaterial;
    resultBack.AddComponent<MeshCollider> ();

    resultFront.AddComponent<MeshFilter> ().mesh = resultFrontMesh;
    resultFront.AddComponent<MeshRenderer> ().material = pathMaterial;
    resultFront.AddComponent<MeshCollider> ();

    Vector3 pos1 = point2;
    Vector3 pos2 = point1;
    RaycastHit pathHit;
    float pathLength = Vector2.Distance (new Vector2(point1.x, point1.z), new Vector2(point2.x, point2.z));
    float currentPos = pathLength * Mathf.Min (1/pathLength, 1);
    float angle = 0;

    pos1 = Vector3.Lerp (point1, point2, Mathf.Min(1/pathLength * 1, 1));
    pos1.y = pathMHeight;

    Physics.Raycast (pos1, Vector3.down, out pathHit, 1000, _pathLayers);
    pos1 = new Vector3 (pos1.x, pathHit.point.y + pathHeight, pos1.z);
    //result
    resultMeshVerticies.Add (new Vector3(pathWidthH, point1.y, 0));
    resultMeshVerticies.Add (new Vector3(-pathWidthH, point1.y, 0));
    resultMeshUVs.Add (new Vector2(pathWidthH, 0));
    resultMeshUVs.Add (new Vector2(-pathWidthH, 0));

    resultMeshVerticies.Add (new Vector3(pathWidthH, pos1.y, currentPos));
    resultMeshVerticies.Add (new Vector3(-pathWidthH, pos1.y, currentPos));
    resultMeshUVs.Add (new Vector2(pathWidthH, currentPos));
    resultMeshUVs.Add (new Vector2(-pathWidthH, currentPos));

    resultMeshTriangles.Add (0);
    resultMeshTriangles.Add (1);
    resultMeshTriangles.Add (2);

    resultMeshTriangles.Add (1);
    resultMeshTriangles.Add (3);
    resultMeshTriangles.Add (2);
    //Left
    resultLeftMeshVerticies.Add(new Vector3(-pathWidthH, point1.y, 0));
    resultLeftMeshVerticies.Add(new Vector3(-pathWidthH, point1.y - 1, 0));
    resultLeftMeshUVs.Add (new Vector2(-pathWidthH, 0));
    resultLeftMeshUVs.Add (new Vector2(-pathWidthH, -1));

    resultLeftMeshVerticies.Add (new Vector3(-pathWidthH, pos1.y, currentPos));
    resultLeftMeshUVs.Add (new Vector2(-pathWidthH, currentPos));
    resultLeftMeshVerticies.Add (new Vector3(-pathWidthH, pos1.y - 1, currentPos));
    resultLeftMeshUVs.Add (new Vector2(-pathWidthH, currentPos - 1));

    resultLeftMeshTriangles.Add (0);
    resultLeftMeshTriangles.Add (1);
    resultLeftMeshTriangles.Add (2);

    resultLeftMeshTriangles.Add (1);
    resultLeftMeshTriangles.Add (3);
    resultLeftMeshTriangles.Add (2);
    //Right
    resultRightMeshVerticies.Add(new Vector3(pathWidthH, point1.y, 0));
    resultRightMeshVerticies.Add(new Vector3(pathWidthH, point1.y - 1, 0));
    resultRightMeshUVs.Add (new Vector2(pathWidthH, 0));
    resultRightMeshUVs.Add (new Vector2(pathWidthH, -1));

    resultRightMeshVerticies.Add (new Vector3(pathWidthH, pos1.y, currentPos));
    resultRightMeshUVs.Add (new Vector2 (pathWidthH, currentPos));
    resultRightMeshVerticies.Add (new Vector3(pathWidthH, pos1.y - 1, currentPos));
    resultRightMeshUVs.Add (new Vector2(pathWidthH, currentPos - 1));

    resultRightMeshTriangles.Add (2);
    resultRightMeshTriangles.Add (1);
    resultRightMeshTriangles.Add (0);

    resultRightMeshTriangles.Add (2);
    resultRightMeshTriangles.Add (3);
    resultRightMeshTriangles.Add (1);

    //Front
    resultFrontMeshVerticies.Add (new Vector3(pathWidthH, point1.y, 0));
    resultFrontMeshVerticies.Add (new Vector3(-pathWidthH, point1.y, 0));
    resultFrontMeshUVs.Add (new Vector2(pathWidthH, 0));
    resultFrontMeshUVs.Add (new Vector2(-pathWidthH, 0));

    resultFrontMeshVerticies.Add (new Vector3(pathWidthH, point1.y - 1, 0));
    resultFrontMeshVerticies.Add (new Vector3(-pathWidthH, point1.y - 1, 0));
    resultFrontMeshUVs.Add (new Vector2(pathWidthH, -1));
    resultFrontMeshUVs.Add (new Vector2(-pathWidthH, -1));

    resultFrontMeshTriangles.Add (2);
    resultFrontMeshTriangles.Add (1);
    resultFrontMeshTriangles.Add (0);

    resultFrontMeshTriangles.Add (2);
    resultFrontMeshTriangles.Add (3);
    resultFrontMeshTriangles.Add (1);

    if (!ghost) {
      //TerrainVolumeEditor.SculptTerrainVolume (_terrainVolume, pos1.x, pos1.y, pos1.z, 1, _pathSize / 5, -0.075f * 1 / _pathSize);
    }

    angle = GetHeightAngle (pos2, pos1);
    GameObject TMPStair;
    if (angle > 15 || angle < -15) {
      for (float s = 0; s < Mathf.Ceil (Mathf.Abs (pos2.y - pos1.y) / 0.14f); s++) {
        TMPStair = (GameObject)Instantiate (_stairPrefab, Vector3.Lerp (new Vector3 (0, pos2.y, 0), new Vector3 (0, pos1.y, currentPos), (float)s / (float)Mathf.Ceil (Mathf.Abs (pos2.y - pos1.y) / 0.14f)), Quaternion.Euler (90, 180, 0));
        TMPStair.transform.SetParent (result.transform);
      }
    }
    pos2 = pos1;
    for (int i = 2; i < pathLength + 1; i++) {
      pos1 = Vector3.Lerp (point1, point2, Mathf.Min(1/pathLength * i, 1));
      pos1.y = pathMHeight;

      Physics.Raycast (pos1, Vector3.down, out pathHit, 1000, _pathLayers);
      pos1 = new Vector3 (pos1.x, pathHit.point.y + pathHeight, pos1.z);
      currentPos = pathLength * Mathf.Min (1/pathLength * i, 1);
      angle = GetHeightAngle (pos2, pos1);
      if(angle > 15 || angle < -15){
        for (float s = 0; s < Mathf.Ceil(Mathf.Abs(pos2.y - pos1.y)/0.14f); s++) {
          TMPStair = (GameObject)Instantiate (_stairPrefab, Vector3.Lerp(new Vector3(0, pos2.y, pathLength * Mathf.Min (1/pathLength * (i - 1), 1)), new Vector3(0, pos1.y, currentPos), (float)s / (float)Mathf.Ceil(Mathf.Abs(pos2.y - pos1.y)/0.14f)), Quaternion.Euler(90, 180, 0));
          TMPStair.transform.SetParent(result.transform);
        }
      }
      //result
      resultMeshVerticies.Add (new Vector3(pathWidthH, pos1.y, currentPos));
      resultMeshVerticies.Add (new Vector3(-pathWidthH, pos1.y, currentPos));
      resultMeshUVs.Add (new Vector2(pathWidthH, currentPos));
      resultMeshUVs.Add (new Vector2(-pathWidthH, currentPos));

      resultMeshTriangles.Add (resultMeshVerticies.Count - 4);
      resultMeshTriangles.Add (resultMeshVerticies.Count - 3);
      resultMeshTriangles.Add (resultMeshVerticies.Count - 2);

      resultMeshTriangles.Add (resultMeshVerticies.Count - 3);
      resultMeshTriangles.Add (resultMeshVerticies.Count - 1);
      resultMeshTriangles.Add (resultMeshVerticies.Count - 2);
      //Left
      resultLeftMeshVerticies.Add (new Vector3(-pathWidthH, pos1.y, currentPos));
      resultLeftMeshUVs.Add (new Vector2(-pathWidthH, currentPos));

      resultLeftMeshVerticies.Add (new Vector3(-pathWidthH, pos1.y - 1, currentPos));
      resultLeftMeshUVs.Add (new Vector2(-pathWidthH, currentPos - 1));

      resultLeftMeshTriangles.Add (resultLeftMeshVerticies.Count - 4);
      resultLeftMeshTriangles.Add (resultLeftMeshVerticies.Count - 3);
      resultLeftMeshTriangles.Add (resultLeftMeshVerticies.Count - 2);

      resultLeftMeshTriangles.Add (resultLeftMeshVerticies.Count - 3);
      resultLeftMeshTriangles.Add (resultLeftMeshVerticies.Count - 1);
      resultLeftMeshTriangles.Add (resultLeftMeshVerticies.Count - 2);
      //Right
      resultRightMeshVerticies.Add (new Vector3(pathWidthH, pos1.y, currentPos));
      resultRightMeshUVs.Add (new Vector2(pathWidthH, currentPos));

      resultRightMeshVerticies.Add (new Vector3(pathWidthH, pos1.y - 1, currentPos));
      resultRightMeshUVs.Add (new Vector2(pathWidthH, currentPos - 1));

      resultRightMeshTriangles.Add (resultRightMeshVerticies.Count - 2);
      resultRightMeshTriangles.Add (resultRightMeshVerticies.Count - 3);
      resultRightMeshTriangles.Add (resultRightMeshVerticies.Count - 4);

      resultRightMeshTriangles.Add (resultRightMeshVerticies.Count - 2);
      resultRightMeshTriangles.Add (resultRightMeshVerticies.Count - 1);
      resultRightMeshTriangles.Add (resultRightMeshVerticies.Count - 3);

      if (!ghost) {
        //TerrainVolumeEditor.SculptTerrainVolume (_terrainVolume, pos1.x, pos1.y, pos1.z, 1, _pathSize / 5, -0.075f * 1 / _pathSize);
      }
      pos2 = pos1;
    }

    //Back
    resultBackMeshVerticies.Add(new Vector3(pathWidthH, pos1.y, pathLength));
    resultBackMeshVerticies.Add(new Vector3(pathWidthH, pos1.y - 1, pathLength));
    resultBackMeshUVs.Add (new Vector2(pathWidthH, pathLength));
    resultBackMeshUVs.Add (new Vector2(pathWidthH, pathLength - 1));
    resultBackMeshVerticies.Add(new Vector3(-pathWidthH, pos1.y, pathLength));
    resultBackMeshVerticies.Add(new Vector3(-pathWidthH, pos1.y - 1, pathLength));
    resultBackMeshUVs.Add (new Vector2(-pathWidthH, pathLength));
    resultBackMeshUVs.Add (new Vector2(-pathWidthH, pathLength - 1));

    resultBackMeshTriangles.Add (2);
    resultBackMeshTriangles.Add (1);
    resultBackMeshTriangles.Add (0);

    resultBackMeshTriangles.Add (2);
    resultBackMeshTriangles.Add (3);
    resultBackMeshTriangles.Add (1);

    resultMesh.vertices = resultMeshVerticies.ToArray();
    resultMesh.triangles = resultMeshTriangles.ToArray();
    resultMesh.uv = resultMeshUVs.ToArray();
    resultMesh.RecalculateNormals ();

    resultLeftMesh.vertices = resultLeftMeshVerticies.ToArray();
    resultLeftMesh.triangles = resultLeftMeshTriangles.ToArray();
    resultLeftMesh.uv = resultLeftMeshUVs.ToArray();
    resultLeftMesh.RecalculateNormals ();

    resultRightMesh.vertices = resultRightMeshVerticies.ToArray();
    resultRightMesh.triangles = resultRightMeshTriangles.ToArray();
    resultRightMesh.uv = resultRightMeshUVs.ToArray();
    resultRightMesh.RecalculateNormals ();

    resultFrontMesh.vertices = resultFrontMeshVerticies.ToArray();
    resultFrontMesh.triangles = resultFrontMeshTriangles.ToArray();
    resultFrontMesh.uv = resultFrontMeshUVs.ToArray();
    resultFrontMesh.RecalculateNormals ();

    resultBackMesh.vertices = resultBackMeshVerticies.ToArray();
    resultBackMesh.triangles = resultBackMeshTriangles.ToArray();
    resultBackMesh.uv = resultBackMeshUVs.ToArray();
    resultBackMesh.RecalculateNormals ();

    result.GetComponent<MeshFilter> ().mesh = resultMesh;
    result.transform.position = point1 - new Vector3(0, point1.y, 0);
    result.transform.LookAt (point2 - new Vector3(0, point2.y, 0));
    return result;
  }

  public GameObject[] AddOnPath(Vector3 prevPoint1, Vector3 point1, Vector3 point2, float pathWidth, float pathHeight, Material pathMaterial, bool ghost){
    GameObject result = CreatePathMesh (Vector3.Lerp(point1, point2, 0.5f/Vector2.Distance(new Vector2(point1.x, point1.z), new Vector2(point2.x, point2.z))), point2, pathWidth, pathHeight, pathMaterial, ghost);

    MeshFilter[] resultMeshFilter = result.GetComponentsInChildren<MeshFilter> ();

    GameObject resultCorner = new GameObject ();
    GameObject resultCornerLeft = new GameObject ();
    GameObject resultCornerRight = new GameObject ();

    resultCorner.name = "Corner";
    resultCorner.AddComponent<MeshFilter> ();
    resultCorner.AddComponent<MeshRenderer> ().material = pathMaterial;
    resultCorner.AddComponent<MeshCollider> ();

    resultCornerLeft.name = "Corner_Left";
    resultCornerLeft.transform.SetParent (resultCorner.transform);
    resultCornerLeft.AddComponent<MeshFilter> ();
    resultCornerLeft.AddComponent<MeshRenderer> ().material = pathMaterial;

    resultCornerRight.name = "Corner_Right";
    resultCornerRight.transform.SetParent (resultCorner.transform);
    resultCornerRight.AddComponent<MeshFilter> ();
    resultCornerRight.AddComponent<MeshRenderer> ().material = pathMaterial;

    Mesh resultCornerMesh = new Mesh();
    Mesh resultCornerMeshLeft = new Mesh();
    Mesh resultCornerMeshRight = new Mesh();

    List<Vector3> resultCornerVerticies = new List<Vector3> ();
    List<Vector3> resultCornerLeftVerticies = new List<Vector3> ();
    List<Vector3> resultCornerRightVerticies = new List<Vector3> ();

    List<Vector2> resultCornerUVs = new List<Vector2> ();
    List<Vector2> resultCornerLeftUVs = new List<Vector2> ();
    List<Vector2> resultCornerRightUVs = new List<Vector2> ();

    List<int> resultCornerTriangles = new List<int> ();
    List<int> resultCornerLeftTriangles = new List<int> ();
    List<int> resultCornerRightTriangles = new List<int> ();

    float pathAngle = GetPathAngle (point2, prevPoint1);
    Debug.Log (pathAngle);
    if(pathAngle > 180){

    }else if(pathAngle < 180){

    }else{

    }

    resultCornerMesh.vertices = resultCornerVerticies.ToArray();
    resultCornerMeshLeft.vertices = resultCornerLeftVerticies.ToArray();
    resultCornerMeshRight.vertices = resultCornerRightVerticies.ToArray();

    resultCornerMesh.uv = resultCornerUVs.ToArray();
    resultCornerMeshLeft.uv = resultCornerLeftUVs.ToArray();
    resultCornerMeshRight.uv = resultCornerRightUVs.ToArray();

    resultCornerMesh.triangles = resultCornerTriangles.ToArray();
    resultCornerMeshLeft.triangles = resultCornerLeftTriangles.ToArray();
    resultCornerMeshRight.triangles = resultCornerRightTriangles.ToArray();

    resultCorner.GetComponent<MeshFilter> ().mesh = resultCornerMesh;
    resultCornerLeft.GetComponent<MeshFilter> ().mesh = resultCornerMeshLeft;
    resultCornerRight.GetComponent<MeshFilter> ().mesh = resultCornerMeshRight;

    GameObject[] finalResult = new GameObject[2];
    finalResult [0] = result;
    finalResult [1] = resultCorner;
    return finalResult;
  }

  public void StartBuildingPaths(){
    _buildingPaths = true;
    _previouslyConnected = false;
    _pathSize = 1;
    _pathUI.SetActive (true);
    _pathHeight = 0;
    _usingGrid = false;
    _gridSize = 1;
  }

  public void StopBuildingPaths(){
    _buildingPaths = false;
    _previouslyConnected = false;
    _pathSize = 1;
    _pathPoints = new List<Vector3> ();
    for (int i = 0; i < _ghostPath.Count; i++) {
      Destroy (_ghostPath [i]);
    }
    while(_ghostPath.Count > 0){
      _ghostPath.RemoveAt (0);
    }
    _pathUI.SetActive (false);
    _pathHeight = 0;
    _usingGrid = false;
    _gridLight.SetActive (false);
    _gridSize = 1;
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

  public void StartUsingGrid(){
    if(_buildingPaths){
      _usingGrid = true;
      _gridLight.SetActive (true);
    }
  }

  public void StopUsingGrid(){
    if(_buildingPaths){
      _usingGrid = false;
      _gridLight.SetActive (false);
    }
  }

  public void UpdateGridSize(){
    if (_buildingPaths) {
      _gridSize = _gridSlider.value;
      _gridLight.GetComponent<Light> ().cookieSize = _gridSize;
    } else {
      _gridSlider.value = _gridSize;
    }
  }
}
