using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Title: BuildingEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/28/2017
//Purpose: To build an Object, render building object UI, and destroy an object.
public class BuildingEngine : MonoBehaviour {
	private bool building = false;
	private GameObject gameObjectToBuild;
	private Vector3 gameObjectToBuildSize;
	private Ray raycastMouse;
	private RaycastHit rayHit;
	public LayerMask buildingLayers;
	private GameObject ghostGameObject;
	public GameObject gridLight;
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
	public Texture gridMaterial;
	public Texture triangleMaterial;
	public GameObject smokePrefab;
	public void StartBuild(GameObject gameObjectBuild, Vector3 buildingSize){
		building = true;
		gameObjectToBuild = gameObjectBuild;
		buildingMode = 0;
		gridSize = 1;
		gameObjectToBuildSize = buildingSize;
		buildingUI.SetActive (true);
    gridLight.GetComponent<Light> ().cookieSize = 1;
    ghostVis = (GameObject)Instantiate (ghostVisPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		ghostVis.transform.Rotate(new Vector3(90, 0, 0));
		ghostVis.transform.localScale = new Vector3 (gameObjectToBuildSize.x, gameObjectToBuildSize.z, 1);
	}

	public void StopBuild(){
		building = false;
		gameObjectToBuild = null;
		buildingMode = 0;
		gridSize = 1;
		Destroy (ghostGameObject);
		buildingUI.SetActive (false);
		Destroy (ghostVis);
    gridLight.SetActive (false);
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
		if (building && buildingMode != 0) {
			gridSize = gridSize * 2;
      gridLight.GetComponent<Light> ().cookieSize = gridSize;
			ghostVis.transform.localScale = new Vector3 (gridSize, gridSize, 1);
		}
	}

	public void DecreaseGrid(){
		if (building && buildingMode != 0) {
			gridSize = gridSize / 2;
			ghostVis.transform.localScale = new Vector3 (gridSize, gridSize, 1);
      gridLight.GetComponent<Light> ().cookieSize = gridSize;
		}
	}

	public void NormalMode(){
		if (building) {
			buildingMode = 0;
      gridLight.SetActive (false);
			ghostVis.GetComponent<Renderer> ().material = ghostVisSquare;
			ghostVis.transform.localScale = new Vector3 (gameObjectToBuildSize.x, gameObjectToBuildSize.z, 1);
		}
	}

	public void GridMode(){
		if (building) {
			buildingMode = 1;
			ghostVis.GetComponent<Renderer> ().material = ghostVisSquare;
      gridLight.GetComponent<Light>().cookie = gridMaterial;
      gridLight.SetActive (true);
      ghostVis.transform.localScale = new Vector3 (gridSize, gridSize, 1);
		}
	}

	public void TriangleMode(){
		if (building) {
			buildingMode = 2;
			gridLight.SetActive (true);
      gridLight.GetComponent<Light>().cookie = triangleMaterial;
      ghostVis.GetComponent<Renderer> ().material = ghostVisTriangle;
      ghostVis.transform.localScale = new Vector3 (gridSize, gridSize, 1);
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
