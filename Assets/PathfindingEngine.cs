using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cubiquity;

public class PathfindingEngine : MonoBehaviour {

    public AstarPath pathfinding;
    public TerrainVolume terrain;

    private float lastRescan = 0f;
    private float rescanRate = 5f;

    private bool changesMade = false;

	// Use this for initialization
	void Start () {
		
        terrain.OnMeshSyncComplete += TerrainComplete;

	}
	
	// Update is called once per frame
	void Update () 
    {
        if(changesMade == true && (lastRescan + rescanRate < Time.time))
        {
            Debug.Log("Scanning graphs");
            RecastGraph graph = (RecastGraph)pathfinding.graphs[0];
            graph.SnapForceBoundsToScene();
            pathfinding.Scan();
            Debug.Log("Scan complete");
            changesMade = false;
            lastRescan = Time.time;
        }
	}

    void TerrainComplete()
    {
        changesMade = true;
    }

}
