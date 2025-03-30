using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurvedRoadMesh : BaseMakeMesh
{

    double degrees = 4.5f;
    double radians;



    protected override void SetVertices()
    {
        radians = degrees * Math.PI / 180.0;
        for(int i = 0; i< 21; i++) 
        {
            vertices.Add(new Vector3((float)Math.Cos(radians*i) * hsize, 0f, (float)Math.Sin(radians*i) * hsize));
            vertices.Add(new Vector3((float)Math.Cos(radians * i) * hsize*2, 0f, (float)Math.Sin(radians * i) * hsize*2));
        }
    }
    protected override void SetNormals()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            normals.Add(new Vector3(0f, -1f, 0f));
        }
    }

    protected override void SetUV()
    {

          float uvInterval = 1f / (vertices.Count / 2 - 1); 
          for (int i = 0; i < vertices.Count / 2; i++)
          {
              float u = i * uvInterval;
              uv.Add(new Vector2(u, 0f)); 
              uv.Add(new Vector2(u, 1f)); 
          }
    }

    protected override void SetTriangles()
    {
        for(int i = 0; i < 20; i++)
        {
            triangles.Add(2 * i);
            triangles.Add(2 * i + 3);
            triangles.Add(2 * i + 1);
        }

        for (int i = 0; i < 20; i++)
        {
            triangles.Add(2 * i);
            triangles.Add(2 * i + 2);
            triangles.Add(2 * i + 3);
        }
    }
}