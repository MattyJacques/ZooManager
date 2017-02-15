using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cubiquity;

public class PathfindingEngine : MonoBehaviour {

    public AstarPath pathfinding;
    public TerrainVolume terrain;

	// Use this for initialization
	void Start () {
		
        terrain.OnMeshSyncComplete += TerrainComplete;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TerrainComplete()
    {
        Debug.Log("Scanning graphs");
        RecastGraph graph = (RecastGraph)pathfinding.graphs[0];
        graph.SnapForceBoundsToScene();
        pathfinding.Scan();
        Debug.Log("Scan complete");
    }

}
