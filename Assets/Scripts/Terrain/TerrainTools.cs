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
  public TerrainVolume terrainVolume;
  private RaycastHit rayHit;
  private Ray raycastMouse;
	private bool editingTerrain = false;
  private bool addingTerrain = true;
	private float brushSize = 1;
	private float brushStrength = 1;
	void Update () {
    if (editingTerrain) {
      raycastMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
      if (Physics.Raycast (raycastMouse, out rayHit, 1000, terrainLayers)) {
        if (Input.GetMouseButton (0) && !GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().IsPointerOverGameObject ()) {
          if(addingTerrain && !Input.GetKey(KeyCode.LeftShift) || !addingTerrain && Input.GetKey(KeyCode.LeftShift)){
            TerrainVolumeEditor.SculptTerrainVolume (terrainVolume, rayHit.point.x, rayHit.point.y, rayHit.point.z, brushStrength, brushSize, 1);
          }else if(rayHit.point.y > brushSize){
            TerrainVolumeEditor.SculptTerrainVolume (terrainVolume, rayHit.point.x, rayHit.point.y, rayHit.point.z, brushStrength, brushSize, -1);
          }
        }
      }
    }
	}

	public void StartTerrainEditing(){
		editingTerrain = true;
    addingTerrain = true;
		terrainToolsUI.SetActive (true);
	}

	public void StopTerrainEditing(){
		editingTerrain = false;
    addingTerrain = true;
		terrainToolsUI.SetActive (false);
	}

	public void ChangeSize(){
		brushSize = sizeScrollbar.value;
	}

	public void ChangeStrength(){
		brushStrength = strengthScrollbar.value;
	}

	public void BuildTerrain(){
    addingTerrain = true;
	}

	public void RemoveTerrain(){
    addingTerrain = false;
	}

  public void ToggleTerrainEditing(){
    if(editingTerrain){
      StopTerrainEditing ();
    }else{
      StartTerrainEditing ();
    }
  }
}
