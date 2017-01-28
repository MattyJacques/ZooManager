using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Title: BuildingEngine.cs
//Author: Aimmmmmmmmm
//Date: 1/28/2017
//Purpose: To build an Object
//TODO: Formatting. I know theres wrong formatting, but I gotta go to bed.
public class BuildingEngine : MonoBehaviour {
	private bool building = false;
	private GameObject gameObjectToBuild;
	private Vector3 gameObjectToBuildSize;
	private Ray raycastMouse;
	private RaycastHit rayHit;
	public LayerMask buildingLayers;
	private GameObject ghostGameObject;
	private int buildingMode = 0; 
	private float gridSize = 1;
	private Vector3 positionToBuild = new Vector3 (0, 0, 0);
	List<BoxCollider> previousBuildingColliders = new List<BoxCollider> ();
	public void StartBuild(GameObject gameObjectBuild, Vector3 buildingSize){
		building = true;
		gameObjectToBuild = gameObjectBuild;
		buildingMode = 0;
		gridSize = 1;
		gameObjectToBuildSize = buildingSize;
	}

	public void StopBuild(){
		building = false;
		gameObjectToBuild = null;
		buildingMode = 0;
		gridSize = 1;
		Destroy (ghostGameObject);
	}

	void Update(){
		if(building){
			raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(raycastMouse, out rayHit, 1000, buildingLayers)){

				//GridBased
				if (buildingMode == 1 && !Input.GetKey ("z")) {
					positionToBuild = new Vector3 (Mathf.RoundToInt (rayHit.point.x * 1 / gridSize) * gridSize, rayHit.point.y, Mathf.RoundToInt (rayHit.point.z * 1 / gridSize) * gridSize);
					//TriangleBased
				} else if (buildingMode == 2 && !Input.GetKey ("z")) {
					positionToBuild = new Vector3 (Mathf.RoundToInt (rayHit.point.x * 1 / gridSize) * gridSize, rayHit.point.y, Mathf.RoundToInt (rayHit.point.z * 1 / gridSize) * gridSize);
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
					} else if (closestDistance == triangle2Dist) {
						positionToBuild = trianglePos2;
					} else if (closestDistance == triangle3Dist) {
						positionToBuild = trianglePos2;
					} else if (closestDistance == triangle4Dist) {
						positionToBuild = trianglePos2;
					}
				} else if (!Input.GetKey ("z")) {
					positionToBuild = new Vector3 (rayHit.point.x, rayHit.point.y, rayHit.point.z);
				}

				if(ghostGameObject == null){
					ghostGameObject = (GameObject)Instantiate (gameObjectToBuild, positionToBuild + new Vector3(0, 5, 0), Quaternion.identity);
				}else{
					ghostGameObject.transform.position = Vector3.MoveTowards(ghostGameObject.transform.position, positionToBuild, 1);
				}

				if(Input.GetKey("z")){
					//TODO: I know theres gonna be a rotation error here where it rotates wrong, will fix in morning. 
					ghostGameObject.transform.LookAt (new Vector3(rayHit.point.x, positionToBuild.y + 5, rayHit.point.z));
				}

				if(Input.GetMouseButtonDown(0)){
					if (!CheckIfOccupied (positionToBuild, gameObjectToBuildSize)) {
						Instantiate (gameObjectToBuild, positionToBuild, ghostGameObject.transform.rotation);
						GameObject tmpBoundingBox = new GameObject ();
						tmpBoundingBox.AddComponent<BoxCollider> ().size = gameObjectToBuildSize;
						tmpBoundingBox.transform.position = positionToBuild;
						tmpBoundingBox.name = "Building Collider";
						previousBuildingColliders.Add (tmpBoundingBox.GetComponent<BoxCollider>());
						if(!Input.GetKeyDown(KeyCode.LeftShift)){
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
		}
	}

	public void DecreaseGrid(){
		if (building) {
			gridSize = gridSize / 2;
		}
	}

	public void NormalMode(){
		if (building) {
			buildingMode = 0;
		}
	}

	public void GridMode(){
		if (building) {
			buildingMode = 1;
		}
	}

	public void TriangleMode(){
		if (building) {
			buildingMode = 2;
		}
	}
}
