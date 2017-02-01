using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cubiquity;
//Title: TerrainTools.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To Edit Terrain!
public class TerrainTools : MonoBehaviour {
	public GameObject terrainToolsUI;
	public Slider strengthScrollbar;
	public Slider sizeScrollbar;
  public LayerMask terrainLayers;
  public LayerMask heightLayers;
  public TerrainVolume terrainVolume;
  public GameObject ghostTilePrefab;
  public GameObject heightPrefab;
  public Slider gridSizeSlider;

  private RaycastHit rayHit;
  private Ray raycastMouse;
	private bool editingTerrain = false;
  private int terrainMode = 0;
	private float brushSize = 1;
	private float brushStrength = 1;
  private Vector3 startingFlatten;
  private float gridSize = 1;
  private GameObject ghostTile;
  private bool changingTerrain = false;
  private Vector3 gridPosition;
  private GameObject heightBlock;
  private float previousHeight = 0;
  void Update () {
    if (editingTerrain) {
      raycastMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
      if (Physics.Raycast (raycastMouse, out rayHit, 1000, terrainLayers)) {
        if(Input.GetMouseButtonDown(0) && terrainMode == 3){
          startingFlatten = rayHit.point;
        }
   
        if(terrainMode == 4 && Input.GetMouseButtonUp(0)){
          changingTerrain = false;
          Destroy (heightBlock);
      
          if(gridPosition.y != previousHeight){
            MaterialSet materialSet = new MaterialSet ();
            if(gridPosition.y > previousHeight){
              materialSet.weights [0] = 0;
              materialSet.weights [1] = 0;
              materialSet.weights [2] = 255;
              for (int i = 0; i < gridPosition.y - previousHeight; i++) {
                for (int x = 0; x < gridSize; x++) {
                  for (int z = 0; z < gridSize; z++) {
                    terrainVolume.data.SetVoxel (x + Mathf.RoundToInt(gridPosition.x), i + Mathf.RoundToInt(previousHeight), z + Mathf.RoundToInt(gridPosition.z), materialSet);
                  }
                }
              }
            }else{
              materialSet.weights [0] = 0;
              materialSet.weights [1] = 0;
              materialSet.weights [2] = 0;
              for (int i = 0; i < previousHeight - gridPosition.y + 1; i++) {
                for (int x = 0; x < gridSize; x++) {
                  for (int z = 0; z < gridSize; z++) {
                    terrainVolume.data.SetVoxel (x + Mathf.RoundToInt(gridPosition.x), -i + Mathf.RoundToInt(previousHeight) + 1, z + Mathf.RoundToInt(gridPosition.z), materialSet);
                  }
                }
              }
            }
          }
        }else if(terrainMode == 4 && Input.GetMouseButtonDown(0) && !GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().IsPointerOverGameObject ()){
          changingTerrain = true;
          heightBlock = (GameObject)Instantiate (heightPrefab, gridPosition, Quaternion.identity);
          previousHeight = gridPosition.y; 
        }else if(terrainMode == 4 && !changingTerrain){
          gridPosition = new Vector3 (Mathf.RoundToInt (rayHit.point.x * 1 / gridSize) * gridSize, Mathf.Round(rayHit.point.y), Mathf.RoundToInt (rayHit.point.z * 1 / gridSize) * gridSize);
          if(gridSize == 4){
            ghostTile.transform.position = gridPosition + new Vector3 (1.5f, 0.01f, 1.5f);
          }else{
            ghostTile.transform.position = gridPosition + new Vector3 (0, 0.01f, 0);
          }
        }else if(terrainMode == 4 && changingTerrain){
          if(Physics.Raycast(raycastMouse, out rayHit, 1000, heightLayers)){
            gridPosition = new Vector3 (gridPosition.x, Mathf.Round(rayHit.point.y), gridPosition.z);
            if(gridSize == 4){
              ghostTile.transform.position = gridPosition + new Vector3 (1.5f, 0.01f, 1.5f);
            }else{
              ghostTile.transform.position = gridPosition + new Vector3 (0, 0.01f, 0);
            }
          }
        }
        if (Input.GetMouseButton (0) && !GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().IsPointerOverGameObject ()) {
          if (terrainMode == 0) {
            TerrainVolumeEditor.SculptTerrainVolume (terrainVolume, rayHit.point.x, rayHit.point.y, rayHit.point.z, 0, brushSize, brushStrength);
            uint materialSet = 2;
            TerrainVolumeEditor.PaintTerrainVolume (terrainVolume, rayHit.point.x, rayHit.point.y, rayHit.point.z, 1, brushSize, 1, materialSet);
          } else if (terrainMode == 1) {
            TerrainVolumeEditor.SculptTerrainVolume (terrainVolume, rayHit.point.x, rayHit.point.y, rayHit.point.z, 0, brushSize, -brushStrength);
          } else if (terrainMode == 2) {
            TerrainVolumeEditor.BlurTerrainVolume (terrainVolume, rayHit.point.x, rayHit.point.y, rayHit.point.z, 0, brushSize, brushStrength);
          } else if (terrainMode == 3) {
            TerrainVolumeEditor.BlurTerrainVolume (terrainVolume, rayHit.point.x, startingFlatten.y + 1, rayHit.point.z, 0, brushSize, brushStrength);
            TerrainVolumeEditor.SculptTerrainVolume (terrainVolume, rayHit.point.x, startingFlatten.y - brushSize, rayHit.point.z, brushSize, brushSize, brushStrength);
          }
        }
      }
    }
	}

	public void StartTerrainEditing(){
		editingTerrain = true;
    terrainMode = 0;
		terrainToolsUI.SetActive (true);
    ghostTile = (GameObject)Instantiate (ghostTilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
    ghostTile.transform.Rotate (new Vector3(90, 0, 0));
    ghostTile.SetActive (false);
    changingTerrain = false;
    gridSize = 1;
	}

	public void StopTerrainEditing(){
		editingTerrain = false;
    terrainMode = 0;
		terrainToolsUI.SetActive (false);
    Destroy (ghostTile);
    changingTerrain = false;
    gridSize = 1;
    if(heightBlock != null){
      Destroy (heightBlock);
    }
	}

	public void ChangeSize(){
		brushSize = sizeScrollbar.value;
	}

	public void ChangeStrength(){
		brushStrength = strengthScrollbar.value;
	}

	public void BuildTerrain(){
    terrainMode = 0;
    ghostTile.SetActive (false);
    changingTerrain = false;
	}

	public void RemoveTerrain(){
    terrainMode = 1;
    ghostTile.SetActive (false);
    changingTerrain = false;
	}

  public void SmoothTerrain(){
    terrainMode = 2;
    ghostTile.SetActive (false);
    changingTerrain = false;
  }

  public void FlattenTerrain(){
    terrainMode = 3;
    ghostTile.SetActive (false);
    changingTerrain = false;
  }

  public void GridTerrain(){
    terrainMode = 4;
    ghostTile.SetActive (true);
    changingTerrain = false;
  }

  public void IncreaseGrid(){
    if(terrainMode == 4){
      gridSize = gridSize * 2;
      ghostTile.transform.localScale = new Vector3(gridSize + 0.1f, gridSize + 0.1f, 1);
    }
  }

  public void DecreaseGrid(){
    if(terrainMode == 4){
      gridSize = gridSize/2;
      ghostTile.transform.localScale = new Vector3(gridSize + 0.1f, gridSize + 0.1f, 1);
    }
  }

  public void ChangeGridSize(){
    if(terrainMode == 4){
      gridSize = gridSizeSlider.value * 2;
      ghostTile.transform.localScale = new Vector3(gridSize + 0.1f, gridSize + 0.1f, 1);
    }else{
      gridSizeSlider.value = gridSize/2;
    }
  }

  public void ToggleTerrainEditing(){
    if(editingTerrain){
      StopTerrainEditing ();
    }else{
      StartTerrainEditing ();
    }
  }
}
