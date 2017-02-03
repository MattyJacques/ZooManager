//Title: TestTerrainTools.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To test terrain editing!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTerrainTools : MonoBehaviour {

  void Start () {
    this.gameObject.GetComponent<TerrainTools> ().StartTerrainEditing ();	
	}
}
