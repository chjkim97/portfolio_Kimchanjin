using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public abstract class BaseMakeMesh : MonoBehaviour
{
    public Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    
    protected List<Vector3> vertices;
    protected List<Vector2> uv;
    protected List<Vector3> normals;
    protected List<int> triangles;
    
    /// <summary>
    /// number of points on the curve
    /// </summary>
    [SerializeField, Range(0f, 100f)] protected int dense = 20;
    /// <summary>
    /// length of road
    /// </summary>
    [SerializeField, Range(0f, 100f)] protected float size = 40f;
    /// <summary>
    /// half the length of the road
    /// </summary>
    protected float hsize = 10f;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
       
    }

    // Update is called once per frame
    void Update()
    {
        mesh = new Mesh();

        SetMesh();
        
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        
    }

    private void SetMesh()
    {
        hsize = 0.5f * size;
        
        vertices = new List<Vector3>();
        uv = new List<Vector2>();
        normals = new List<Vector3>();
        triangles = new List<int>();
        
        SetVertices();
        SetUV();
        SetNormals();
        SetTriangles();
    }

    /// <summary>
    ///   <para>assigns a new vertex positions array.</para>
    /// </summary>
    protected abstract void SetVertices();
    
    /// <summary>
    /// Uvs are  Set UVs using dense
    /// </summary>
    protected abstract void SetUV();

    /// <summary>
    /// Normals are a list of normals for each vertices.
    /// </summary>
    protected abstract void SetNormals();

    /// <summary>
    /// Set triangles using dense
    /// </summary>
    protected abstract void SetTriangles();
}
