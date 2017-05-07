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
       /* if(changesMade == true && (lastRescan + rescanRate < Time.time))
        {
            Debug.Log("Scanning graphs");
            RecastGraph graph = (RecastGraph)pathfinding.graphs[0];

      if(graph.batchTileUpdate)
      {
        changesMade = false;
        lastRescan = Time.time;
        return;
      }
      Debug.Log("Scanning graphs");
      graph.SnapForceBoundsToScene();
      //graph.StartBatchTileUpdate();
            pathfinding.Scan();
            Debug.Log("Scan complete");
            changesMade = false;
            lastRescan = Time.time;
        }*/

    if(lastRescan + rescanRate < Time.time)
    {
      GridGraph graph = (GridGraph)pathfinding.graphs[0];

      Region reg = terrain.data.enclosingRegion;
      float nodeSize = 0.5f;

      graph.Width = (int)(reg.upperCorner.x - reg.lowerCorner.x) * (int)(1 / nodeSize);
      graph.Depth = (int)(reg.upperCorner.z - reg.lowerCorner.z) * (int)(1 / nodeSize);
      graph.center = new Vector3(graph.Width/(2 * 1/nodeSize), -10f, graph.Depth/(2 * 1/nodeSize));
      graph.nodeSize = nodeSize;

      graph.UpdateSizeFromWidthDepth();

      pathfinding.Scan();

      lastRescan = Time.time;
    }
	}

    void TerrainComplete()
    {
        changesMade = true;
    }

}
