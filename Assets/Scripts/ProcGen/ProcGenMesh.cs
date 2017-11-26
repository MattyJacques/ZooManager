// Title        : ProcGenMesh.cs
// Purpose      : Creates the actual procedurally generated meshes used for the pavement system.  See 'MeshBuilder.cs'.
// Author       : Tony Haggar
// Date         : 16/08/2017

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Pathfinding;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ProcGenMesh : MonoBehaviour
{
    MeshFilter mf;
    MeshBuilder mb;
    float heightOffset;

    public void SetHeightOffset(float offset)
    {
        heightOffset = offset;
    }


    // Use this for initialization
    void Start()
    {
        //Debug.Log("procgenmesh start called");
        this.transform.localPosition = new Vector3(0, 0.1f + heightOffset, 0);
    }

    //Build a smooth rounded corner where the paths intersect.
    public void BuildCornerCurve(List<Vector3> curvePositions, Vector3 origin, bool clockWiseTurn, int pathNo)
    {
        //Debug.Log("Building curved intersection corner, origin: " + origin);

        List<Vector3> bottomPoints = new List<Vector3>();

        //Create the bottom vertex points.
        //Debug.Log("Building bottom points");

        for (int i = 0; i < curvePositions.Count; i++)
        {
            //Debug.Log("Next curve position: " + curvePositions[i]);
            Vector3 nextPosition = new Vector3(curvePositions[i].x, curvePositions[i].y - 1f, curvePositions[i].z);
            //Debug.Log("Next position: " + nextPosition);
            bottomPoints.Add(nextPosition);
            //GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //test.transform.localScale = Vector3.one / 5.0f;
            //test.transform.position = nextPosition;
        }


        if (clockWiseTurn)
        {
            for (int i = 0; i < curvePositions.Count - 1; i++)
            {
                //Build the top faces.
                mb.BuildTriangle(origin, curvePositions[i], curvePositions[i + 1], pathNo - 1);
                //Build the side faces.
                mb.BuildTriangle(curvePositions[i], bottomPoints[i], bottomPoints[i + 1], pathNo - 1);
                mb.BuildTriangle(bottomPoints[i + 1], curvePositions[i + 1], curvePositions[i], pathNo - 1);

            }
        }
        else
        {
            for (int i = 0; i < curvePositions.Count - 1; i++)
            {
                mb.BuildTriangle(curvePositions[i], origin, curvePositions[i + 1], pathNo - 1);
                //Build the side faces.
                mb.BuildTriangle(bottomPoints[i + 1], bottomPoints[i], curvePositions[i], pathNo - 1);
                mb.BuildTriangle(curvePositions[i], curvePositions[i + 1], bottomPoints[i + 1], pathNo - 1);

            }

        }

        mf.mesh = mb.CreateMesh();
        UpdateUVs();
    }


    //Build a sharp corner where the paths intersect.
    public void BuildIntersectionCorner(Vector3[] pathEndPositions, Vector3 cornerIntersectionPosition, bool clockwiseTurn, int pathNo)
    {
        if (clockwiseTurn)
        {
            if (pathNo == 1)
            {
                mb.BuildTriangle(pathEndPositions[0], cornerIntersectionPosition, pathEndPositions[1], 0);
            }
            else
            {
                mb.BuildTriangle(pathEndPositions[1], cornerIntersectionPosition, pathEndPositions[0], 0);
            }
        }
        else
        {
            if (pathNo == 1)
            {
                mb.BuildTriangle(pathEndPositions[1], cornerIntersectionPosition, pathEndPositions[2], 0);
            }
            else
            {
                mb.BuildTriangle(pathEndPositions[1], pathEndPositions[2], cornerIntersectionPosition, 0);
            }


        }

        mf.mesh = mb.CreateMesh();

        UpdateUVs();
    }

    //Generate the mesh for the path.
    public void BuildProceduralPath(List<Vector3[]> pathPoints, int subMesh)
    {

        //Duplicate the path points so that some geometry can be generated for the sides of the path.
        List<Vector3[]> underneathPathPoints = new List<Vector3[]>();
        Vector3[] bottomPoints;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            bottomPoints = new Vector3[3];
            for (int j = 0; j < 3; j++)
            {
                Vector3 nextPosition = new Vector3(pathPoints[i][j].x, pathPoints[i][j].y - 1.0f, pathPoints[i][j].z);
                //GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //test.transform.localScale = Vector3.one / 5.0f;
                //test.transform.position = nextPosition;
                bottomPoints[j] = nextPosition;

            }
            underneathPathPoints.Add(bottomPoints);
        }

        //Debug.Log("Pathpoints[0] + " + pathPoints[0][0] + " underneath[0]: " + underneathPathPoints[0][0]);

        //Debug.Log("Submesh no is: " + subMesh);

        mf = GetComponent<MeshFilter>();
        mb = new MeshBuilder(2);
        //Debug.Log("Drawing triangles");
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {

            mb.BuildTriangle(pathPoints[i][0], pathPoints[i + 1][0], pathPoints[i][2], subMesh);
            mb.BuildTriangle(pathPoints[i + 1][0], pathPoints[i + 1][2], pathPoints[i][2], subMesh);

            //Build the sides of the path
            //Left hand side
            mb.BuildTriangle(underneathPathPoints[i][0], pathPoints[i + 1][0], pathPoints[i][0], subMesh);
            mb.BuildTriangle(underneathPathPoints[i + 1][0], pathPoints[i + 1][0], underneathPathPoints[i][0], subMesh);

            //Right hand side
            mb.BuildTriangle(underneathPathPoints[i][2], pathPoints[i][2], pathPoints[i + 1][2], subMesh);
            mb.BuildTriangle(underneathPathPoints[i + 1][2], underneathPathPoints[i][2], pathPoints[i + 1][2], subMesh);

        }

        //Build front
        mb.BuildTriangle(underneathPathPoints[0][0], pathPoints[0][0], pathPoints[0][2], subMesh);
        mb.BuildTriangle(pathPoints[0][2], underneathPathPoints[0][2], underneathPathPoints[0][0], subMesh);

        mf.mesh = mb.CreateMesh();

        AstarPath.active.UpdateGraphs(new GraphUpdateObject {shape =  new GraphUpdateShape{convex = true, points = mf.mesh.vertices},modifyTag = true, bounds = mf.mesh.bounds, setTag = PathfindingConstants.VisitorPath});

        UpdateUVs();

    }

    private void UpdateUVs()
    {
        Debug.Log("Updating UVs");
        Vector3[] vertices = mf.mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        Bounds bounds = mf.mesh.bounds;
        int loop = 0;
        while (loop < uvs.Length)
        {
            uvs[loop] = new Vector2(vertices[loop].x / bounds.size.x, vertices[loop].z / bounds.size.x);
            loop++;
        }
        mf.mesh.uv = uvs;
    }
}
