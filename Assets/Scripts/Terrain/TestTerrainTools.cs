using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Title: TestTerrainTools.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To test terrain editing!
public class TestTerrainTools : MonoBehaviour {

	// Use this for initialization
	void Start () {
    this.gameObject.GetComponent<TerrainTools> ().StartTerrainEditing ();	
	}
}
