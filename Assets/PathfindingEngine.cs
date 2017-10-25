using Cubiquity;
using Pathfinding;
using UnityEngine;

namespace Assets
{
    public class PathfindingEngine : MonoBehaviour
    {
        public AstarPath pathfinding;
        public TerrainVolume terrain;

        private void Start ()
        {
            terrain.OnMeshSyncComplete += TerrainComplete;
        }

        private void TerrainComplete()
        {
            RegenerateGraph();
        }

        private void RegenerateGraph()
        {
            GridGraph graph = (GridGraph)pathfinding.graphs[0];

            Region reg = terrain.data.enclosingRegion;
            float nodeSize = 0.5f;

            graph.Width = (reg.upperCorner.x - reg.lowerCorner.x) * (int)(1 / nodeSize);
            graph.Depth = (reg.upperCorner.z - reg.lowerCorner.z) * (int)(1 / nodeSize);
            graph.center = new Vector3(graph.Width / (2 * 1 / nodeSize), -10f, graph.Depth / (2 * 1 / nodeSize));
            graph.nodeSize = nodeSize;

            graph.UpdateSizeFromWidthDepth();

            pathfinding.Scan();
        }
    }
}
