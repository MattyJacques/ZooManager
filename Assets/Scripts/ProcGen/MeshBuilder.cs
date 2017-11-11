// Title        : MeshBuilder.cs
// Purpose      : Generates triangles for use with procedurally generated meshes.
// Author       : Tony Haggar
// Date         : 15/08/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{

    //Hold all of the vertices on the mesh.
    private List<Vector3> vertices = new List<Vector3>();

    //The index for any given point.
    private List<int> indices = new List<int>();

    //Hold all the normals for each vertex.
    private List<Vector3> normals = new List<Vector3>();

    //UV information, for texturing.
    private List<Vector2> uvs = new List<Vector2>();

    //An array of lists, storing the submesh numbers, for applying materials to individual faces.
    private List<int>[] submeshIndices = new List<int>[] { };

    //Constructor
    public MeshBuilder(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];
        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }

    }

    //If we don't care about the normals, just generate them from the points
    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int submesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        BuildTriangle(p0, p1, p2, normal, submesh);
    }

    //Build a triangle from the three specified verts, point the normals the right way, and determine how to apply materials.
    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int submesh)
    {
        //Index zero for point zero, and so on.
        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        //Add the point indexes to the indicies list, so they can be referenced by index.
        indices.Add(p0Index);
        indices.Add(p1Index);
        indices.Add(p2Index);

        //Also allow the submeshes to be referenced by index.  So given a submesh, get the right point.
        submeshIndices[submesh].Add(p0Index);
        submeshIndices[submesh].Add(p1Index);
        submeshIndices[submesh].Add(p2Index);

        //The three Vector3's that comprise the three points on the triangle.
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);

        //All the normals will be pointing the same way.
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);

        //UV space is between 0 and 1
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }

    //Use the triangles created above to actually build a mesh.
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if (submeshIndices[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        return mesh;
    }


}
