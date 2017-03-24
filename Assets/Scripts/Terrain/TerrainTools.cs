//Title: TerrainTools.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To Edit Terrain!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cubiquity;

public class TerrainTools : MonoBehaviour {
	public GameObject _terrainToolsUI;
	public Slider _strengthScrollbar;
	public Slider _sizeScrollbar;
  public LayerMask _terrainLayers;
  public LayerMask _heightLayers;
  public TerrainVolume _terrainVolume;
  public TerrainVolumeCollider _terrainVolumeCollider;
  public GameObject __ghostTilePrefab;
  public GameObject _heightPrefab;
  public Slider __gridSizeSlider;

  private RaycastHit _rayHit;
  private Ray _raycastMouse;
	private bool _editingTerrain = false;
  private int _terrainMode = 0;
	private float _brushSize = 1;
	private float _brushStrength = 1;
  private Vector3 _startingFlatten;
  private float _gridSize = 1;
  private GameObject _ghostTile;
  private bool _changingTerrain = false;
  private Vector3 _gridPosition;
  private GameObject _heightBlock;
  private float _previousHeight = 0;

  void Update () {
    if (_editingTerrain) {
      _raycastMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
      if (Physics.Raycast (_raycastMouse, out _rayHit, 1000, _terrainLayers)) {
        if(Input.GetMouseButtonDown(0) && _terrainMode == 3){
          _startingFlatten = _rayHit.point;
        }
   
        if(_terrainMode == 4 && Input.GetMouseButtonUp(0)){
          _changingTerrain = false;
          Destroy (_heightBlock);
      
          if(_gridPosition.y != _previousHeight){
            MaterialSet materialSet = new MaterialSet ();
            if(_gridPosition.y > _previousHeight){
              materialSet.weights [0] = 0;
              materialSet.weights [1] = 0;
              materialSet.weights [2] = 255;
              for (int i = 0; i < _gridPosition.y - _previousHeight; i++) {
                for (int x = 0; x < _gridSize; x++) {
                  for (int z = 0; z < _gridSize; z++) {
                    _terrainVolume.data.SetVoxel (x + Mathf.RoundToInt(_gridPosition.x), i + Mathf.RoundToInt(_previousHeight), z + Mathf.RoundToInt(_gridPosition.z), materialSet);
                  }
                }
              }
            }else{
              materialSet.weights [0] = 0;
              materialSet.weights [1] = 0;
              materialSet.weights [2] = 0;
              for (int i = 0; i < _previousHeight - _gridPosition.y + 1; i++) {
                for (int x = 0; x < _gridSize; x++) {
                  for (int z = 0; z < _gridSize; z++) {
                    _terrainVolume.data.SetVoxel (x + Mathf.RoundToInt(_gridPosition.x), -i + Mathf.RoundToInt(_previousHeight) + 1, z + Mathf.RoundToInt(_gridPosition.z), materialSet);
                  }
                }
              }
            }
          }
        }else if(_terrainMode == 4 && Input.GetMouseButtonDown(0) && !GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().IsPointerOverGameObject ()){
          _changingTerrain = true;
          _heightBlock = (GameObject)Instantiate (_heightPrefab, _gridPosition, Quaternion.identity);
          _previousHeight = _gridPosition.y; 
        }else if(_terrainMode == 4 && !_changingTerrain){
          _gridPosition = new Vector3 (Mathf.RoundToInt (_rayHit.point.x * 1 / _gridSize) * _gridSize, Mathf.Round(_rayHit.point.y), Mathf.RoundToInt (_rayHit.point.z * 1 / _gridSize) * _gridSize);
          if(_gridSize == 4){
            _ghostTile.transform.position = _gridPosition + new Vector3 (1.5f, 0.01f, 1.5f);
          }else{
            _ghostTile.transform.position = _gridPosition + new Vector3 (0, 0.01f, 0);
          }
        }else if(_terrainMode == 4 && _changingTerrain){
          if(Physics.Raycast(_raycastMouse, out _rayHit, 1000, _heightLayers)){
            _gridPosition = new Vector3 (_gridPosition.x, Mathf.Round(_rayHit.point.y), _gridPosition.z);
            if(_gridSize == 4){
              _ghostTile.transform.position = _gridPosition + new Vector3 (1.5f, 0.01f, 1.5f);
            }else{
              _ghostTile.transform.position = _gridPosition + new Vector3 (0, 0.01f, 0);
            }
          }
        }
        if (Input.GetMouseButton (0) && !GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().IsPointerOverGameObject ()) {
          if (_terrainMode == 0) {
            TerrainVolumeEditor.SculptTerrainVolume (_terrainVolume, _rayHit.point.x, _rayHit.point.y, _rayHit.point.z, 0, _brushSize, _brushStrength);
            uint materialSet = 2;
            TerrainVolumeEditor.PaintTerrainVolume (_terrainVolume, _rayHit.point.x, _rayHit.point.y, _rayHit.point.z, 1, _brushSize, 1, materialSet);
          } else if (_terrainMode == 1) {
            TerrainVolumeEditor.SculptTerrainVolume (_terrainVolume, _rayHit.point.x, _rayHit.point.y, _rayHit.point.z, 0, _brushSize, -_brushStrength);
          } else if (_terrainMode == 2) {
            TerrainVolumeEditor.BlurTerrainVolume (_terrainVolume, _rayHit.point.x, _rayHit.point.y, _rayHit.point.z, 0, _brushSize, _brushStrength);
          } else if (_terrainMode == 3) {
            TerrainVolumeEditor.BlurTerrainVolume (_terrainVolume, _rayHit.point.x, _startingFlatten.y + 1, _rayHit.point.z, 0, _brushSize, _brushStrength);
            TerrainVolumeEditor.SculptTerrainVolume (_terrainVolume, _rayHit.point.x, _startingFlatten.y - _brushSize, _rayHit.point.z, _brushSize, _brushSize, _brushStrength);
          }
        }
      }
    }
	}

	public void StartTerrainEditing(){
		_editingTerrain = true;
    _terrainMode = 0;
		_terrainToolsUI.SetActive (true);
    _ghostTile = (GameObject)Instantiate (__ghostTilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
    _ghostTile.transform.Rotate (new Vector3(90, 0, 0));
    _ghostTile.SetActive (false);
    _changingTerrain = false;
    _gridSize = 1;
	}

	public void StopTerrainEditing(){
		_editingTerrain = false;
    _terrainMode = 0;
		_terrainToolsUI.SetActive (false);
    Destroy (_ghostTile);
    _changingTerrain = false;
    _gridSize = 1;
    if(_heightBlock != null){
      Destroy (_heightBlock);
    }
	}

	public void ChangeSize(){
		_brushSize = _sizeScrollbar.value;
	}

	public void ChangeStrength(){
		_brushStrength = _strengthScrollbar.value;
	}

	public void BuildTerrain(){
    _terrainMode = 0;
    _ghostTile.SetActive (false);
    _changingTerrain = false;
	}

	public void RemoveTerrain(){
    _terrainMode = 1;
    _ghostTile.SetActive (false);
    _changingTerrain = false;
	}

  public void SmoothTerrain(){
    _terrainMode = 2;
    _ghostTile.SetActive (false);
    _changingTerrain = false;
  }

  public void FlattenTerrain(){
    _terrainMode = 3;
    _ghostTile.SetActive (false);
    _changingTerrain = false;
  }

  public void GridTerrain(){
    _terrainMode = 4;
    _ghostTile.SetActive (true);
    _changingTerrain = false;
  }

  public void IncreaseGrid(){
    if(_terrainMode == 4){
      _gridSize = _gridSize * 2;
      _ghostTile.transform.localScale = new Vector3(_gridSize + 0.1f, _gridSize + 0.1f, 1);
    }
  }

  public void DecreaseGrid(){
    if(_terrainMode == 4){
      _gridSize = _gridSize/2;
      _ghostTile.transform.localScale = new Vector3(_gridSize + 0.1f, _gridSize + 0.1f, 1);
    }
  }

  public void ChangeGridSize(){
    if(_terrainMode == 4){
      _gridSize = __gridSizeSlider.value * 2;
      _ghostTile.transform.localScale = new Vector3(_gridSize + 0.1f, _gridSize + 0.1f, 1);
    }else{
      __gridSizeSlider.value = _gridSize/2;
    }
  }

  public void ToggleTerrainEditing(){
    if(_editingTerrain){
      StopTerrainEditing ();
    }else{
      StartTerrainEditing ();
    }
  }
}
