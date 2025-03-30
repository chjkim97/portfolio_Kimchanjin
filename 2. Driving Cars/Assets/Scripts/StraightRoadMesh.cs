using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightRoadMesh : BaseMakeMesh
{
    protected override void SetVertices()
    {
        vertices.Add(new Vector3(-0,0f,hsize));
        vertices.Add(new Vector3(size, 0f, hsize));
        vertices.Add(new Vector3(size, 0f, -0));
        vertices.Add(new Vector3(-0, 0f, -0));

        
    }
    protected override void SetNormals()
    {
        normals.Add(new Vector3(0f, -1f, 0f));
        normals.Add(new Vector3(0f, -1f, 0f));
        normals.Add(new Vector3(0f, -1f, 0f));
        normals.Add(new Vector3(0f, -1f, 0f));
        
    }

    protected override void SetUV()
    {
        uv.Add(new Vector2(0, 0));
        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(1, 1));
        uv.Add(new Vector2(0, 1));
    }

    protected override void SetTriangles()
    {
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);
    }

}
