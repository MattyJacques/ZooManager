using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Title: BuildingEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/28/2017
//Purpose: To build an Object, render building object UI, and destroy an object.
//TODO: Formatting. I know theres wrong formatting, but I gotta go to bed.
public class BuildingEngine : MonoBehaviour {
	private bool building = false;
	private GameObject gameObjectToBuild;
	private Vector3 gameObjectToBuildSize;
	private Ray raycastMouse;
	private RaycastHit rayHit;
	public LayerMask buildingLayers;
	private GameObject ghostGameObject;
	public GameObject ghostVisPrefab;
	public Material ghostVisSquare;
	public Material ghostVisTriangle;
	private GameObject ghostVis;
	private int buildingMode = 0; 
	private float gridSize = 1;
	private Vector3 positionToBuild = new Vector3 (0, 0, 0);
	List<BoxCollider> previousBuildingColliders = new List<BoxCollider> ();
	List<GameObject> previousBuildings = new List<GameObject>();
	public GameObject buildingUI;
	public GameObject gridPrefab;
	public Material gridMaterial;
	public Material triangleMaterial;
	public GameObject smokePrefab;
	private GameObject grid;
	public void StartBuild(GameObject gameObjectBuild, Vector3 buildingSize){
		building = true;
		gameObjectToBuild = gameObjectBuild;
		buildingMode = 0;
		gridSize = 1;
		gameObjectToBuildSize = buildingSize;
		buildingUI.SetActive (true);
		grid = (GameObject)Instantiate (gridPrefab, new Vector3(gridPrefab.transform.localScale.x/2, 0.01f, gridPrefab.transform.localScale.y/2), Quaternion.identity);
		grid.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (0.5f * gridSize, 0.5f * gridSize);
		grid.transform.Rotate (new Vector3(90, 0, 0));
		grid.SetActive (false);
		ghostVis = (GameObject)Instantiate (ghostVisPrefab, new Vector3(0, 0.02f, 0), Quaternion.identity);
		ghostVis.transform.Rotate(new Vector3(90, 0, 0));
	}

	public void StopBuild(){
		building = false;
		gameObjectToBuild = null;
		buildingMode = 0;
		gridSize = 1;
		Destroy (ghostGameObject);
		Destroy (grid);
		buildingUI.SetActive (false);
		Destroy (ghostVis);
	}

	void Update(){
		if(building){
			raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(raycastMouse, out rayHit, 1000, buildingLayers)){

				//GridBased
				if (buildingMode == 1 && !Input.GetKey ("z")) {
					positionToBuild = new Vector3 (Mathf.RoundToInt (rayHit.point.x * 1 / gridSize) * gridSize, rayHit.point.y, Mathf.RoundToInt (rayHit.point.z * 1 / gridSize) * gridSize);
					ghostVis.transform.position = positionToBuild + new Vector3 (0, 0.02f, 0);
					//TriangleBased
				} else if (buildingMode == 2 && !Input.GetKey ("z")) {
					positionToBuild = new Vector3 (Mathf.RoundToInt (rayHit.point.x * 1 / gridSize) * gridSize, rayHit.point.y, Mathf.RoundToInt (rayHit.point.z * 1 / gridSize) * gridSize);
					ghostVis.transform.position = positionToBuild + new Vector3 (0, 0.02f, 0);
					Vector3 trianglePos1 = positionToBuild + new Vector3 (0.25f * gridSize, 0, 0);
					Vector3 trianglePos2 = positionToBuild - new Vector3 (0.25f * gridSize, 0, 0);
					Vector3 trianglePos3 = positionToBuild + new Vector3 (0, 0, 0.25f * gridSize);
					Vector3 trianglePos4 = positionToBuild - new Vector3 (0, 0, 0.25f * gridSize);
					float triangle1Dist = Vector3.Distance (trianglePos1, rayHit.point);
					float triangle2Dist = Vector3.Distance (trianglePos2, rayHit.point);
					float triangle3Dist = Vector3.Distance (trianglePos3, rayHit.point);
					float triangle4Dist = Vector3.Distance (trianglePos4, rayHit.point);
					float closestDistance = Mathf.Min (new float[]{ triangle1Dist, triangle2Dist, triangle3Dist, triangle4Dist });
					if (closestDistance == triangle1Dist) {
						positionToBuild = trianglePos1;
						ghostVis.transform.eulerAngles = new Vector3(90, 90, 0);
					} else if (closestDistance == triangle2Dist) {
						positionToBuild = trianglePos2;
						ghostVis.transform.eulerAngles = new Vector3(90, 270, 0);
					} else if (closestDistance == triangle3Dist) {
						positionToBuild = trianglePos3;
						ghostVis.transform.eulerAngles = new Vector3(90, 0, 0);
					} else if (closestDistance == triangle4Dist) {
						positionToBuild = trianglePos4;
						ghostVis.transform.eulerAngles = new Vector3(90, 180, 0);
					}
				} else if (!Input.GetKey ("z")) {
					positionToBuild = new Vector3 (rayHit.point.x, rayHit.point.y, rayHit.point.z);
					ghostVis.transform.position = positionToBuild + new Vector3 (0, 0.02f, 0);
				}

				if(ghostGameObject == null){
					ghostGameObject = (GameObject)Instantiate (gameObjectToBuild, positionToBuild + new Vector3(0, 5, 0), Quaternion.identity);
				}else{
					ghostGameObject.transform.position = Vector3.MoveTowards(ghostGameObject.transform.position, positionToBuild + new Vector3(0, 5, 0), 5);
				}

				if(Input.GetKey("z")){
					//TODO: I know theres gonna be a rotation error here where it rotates wrong, will fix in morning. 
					ghostGameObject.transform.LookAt (new Vector3(rayHit.point.x, positionToBuild.y + 5, rayHit.point.z));
				}

				if(Input.GetMouseButtonDown(0) && !GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().IsPointerOverGameObject()){
					if (!CheckIfOccupied (positionToBuild, gameObjectToBuildSize)) {
						previousBuildings.Add((GameObject)Instantiate (gameObjectToBuild, positionToBuild, ghostGameObject.transform.rotation));
						GameObject tmpBoundingBox = new GameObject ();
						tmpBoundingBox.AddComponent<BoxCollider> ().size = gameObjectToBuildSize;
						tmpBoundingBox.transform.position = positionToBuild;
						tmpBoundingBox.name = "Building Collider";
						previousBuildingColliders.Add (tmpBoundingBox.GetComponent<BoxCollider>());
						((GameObject)Instantiate (smokePrefab, positionToBuild, Quaternion.identity)).transform.Rotate(new Vector3(90, 0, 0));
						if(!Input.GetKey(KeyCode.LeftShift)){
							StopBuild();
						}
					} else {
						//TODO: Add Failed To Build Sound!
					}
				}

				if(Input.GetMouseButtonDown(1)){
					StopBuild ();
				}
			}
		}
	}

	public bool CheckIfOccupied(Vector3 position, Vector3 size){
		GameObject tmpCollider = new GameObject ();
		tmpCollider.AddComponent<BoxCollider> ().size = size;
		tmpCollider.transform.position = position;
		for (int i = 0; i < previousBuildingColliders.Count; i++) {
			if (tmpCollider.GetComponent<BoxCollider> ().bounds.Intersects (previousBuildingColliders[i].bounds)) {
				Destroy (tmpCollider);
				return true;
			}
		}
		Destroy (tmpCollider);
		return false;
	}

	public void IncreaseGrid(){
		if (building) {
			gridSize = gridSize * 2;
			grid.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (grid.gameObject.transform.localScale.x * (1/gridSize), grid.gameObject.transform.localScale.y * (1/gridSize));
			ghostVis.transform.localScale = new Vector3 (gridSize, gridSize, 1);
			grid.transform.position = new Vector3 (grid.transform.localScale.x/2 + gridSize / 2 - gridSize, 0.01f, grid.transform.localScale.y/2 + gridSize / 2 - gridSize);
		}
	}

	public void DecreaseGrid(){
		if (building) {
			gridSize = gridSize / 2;
			grid.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (grid.gameObject.transform.localScale.x * (1/gridSize), grid.gameObject.transform.localScale.y * (1/gridSize));
			ghostVis.transform.localScale = new Vector3 (gridSize, gridSize, 1);
			grid.transform.position = new Vector3 (grid.transform.localScale.x/2 + gridSize / 2 - gridSize, 0.01f, grid.transform.localScale.y/2 + gridSize / 2 - gridSize);
		}
	}

	public void NormalMode(){
		if (building) {
			buildingMode = 0;
			grid.SetActive (false);
			ghostVis.GetComponent<Renderer> ().material = ghostVisSquare;
			grid.transform.position = new Vector3 (grid.transform.localScale.x/2 + gridSize / 2 - gridSize, 0.01f, grid.transform.localScale.y/2 + gridSize / 2 - gridSize);
		}
	}

	public void GridMode(){
		if (building) {
			buildingMode = 1;
			ghostVis.GetComponent<Renderer> ().material = ghostVisSquare;
			grid.SetActive (true);
			grid.GetComponent<Renderer> ().material = gridMaterial;
			grid.transform.position = new Vector3 (grid.transform.localScale.x/2 + gridSize / 2 - gridSize, 0.01f, grid.transform.localScale.y/2 + gridSize / 2 - gridSize);
		}
	}

	public void TriangleMode(){
		if (building) {
			buildingMode = 2;
			grid.SetActive (true);
			grid.GetComponent<Renderer> ().material = triangleMaterial;
			grid.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (grid.gameObject.transform.localScale.x * (1/gridSize), grid.gameObject.transform.localScale.y * (1/gridSize));
			ghostVis.GetComponent<Renderer> ().material = ghostVisTriangle;
			grid.transform.position = new Vector3 (grid.transform.localScale.x/2 + gridSize / 2 - gridSize, 0.01f, grid.transform.localScale.y/2 + gridSize / 2 - gridSize);
		}
	}

	public bool DestroyBuilding(GameObject buildingToDestroy){
		for (int i = 0; i < previousBuildings.Count; i++) {
			if(buildingToDestroy == previousBuildings[i]){
				Destroy (previousBuildings[i]);
				Destroy (previousBuildingColliders[i]);
				return true;
			}
		}
		return false;
	}
}
