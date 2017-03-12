using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTest : MonoBehaviour {
	public GameObject objectToBuild;
	public Vector3 objectToBuildSize;
	void Start(){
		this.gameObject.GetComponent<BuildingEngine> ().StartBuild (objectToBuild, objectToBuildSize);
	}
}
