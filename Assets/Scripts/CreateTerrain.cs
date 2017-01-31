using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
//Title: CreateTerrain.cs
//Author: Aimmmmmmmmm
//Date: 1/31/2017
//Purpose: To create an empty map!
public class CreateTerrain : MonoBehaviour {
  private TerrainVolumeData terrainVolume;
	
	void Start () {
    terrainVolume = TerrainVolumeData.CreateEmptyVolumeData<TerrainVolumeData>(new Region(0, 0, 0, 512, 512, 256));
    MaterialSet materialSet = new MaterialSet();
    for (int z = 0; z < 512; z++) {
      for (int y = 0; y < 60; y++) {
        for (int x = 0; x < 512; x++) {
          if(y < 40){
            materialSet.weights [0] = 255;
            materialSet.weights [1] = 0;
            materialSet.weights [2] = 0;
          }else if(y < 58){
            materialSet.weights [0] = 0;
            materialSet.weights [1] = 255;
            materialSet.weights [2] = 0;
          }else{
            materialSet.weights [0] = 0;
            materialSet.weights [1] = 0;
            materialSet.weights [2] = 255;
          }
          terrainVolume.SetVoxel (x, y, z, materialSet);
        }
      }
    }
    terrainVolume.CommitChanges();
    GameObject.Find ("Terrain").GetComponent<TerrainVolume> ().data = terrainVolume;
	}
}
