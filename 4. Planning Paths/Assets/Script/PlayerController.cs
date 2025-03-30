using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.5f;
    private Animator playerAnim;
    public MapGraph mapgraph;
    private List<Node> path;
    private int currentbezierPoint = 0;
   
    public List<Vector3> groundPath = new List<Vector3>();
    private List<Vector3> bezierPoints = new List<Vector3>();


    public void Move()
    {
        if (currentbezierPoint < bezierPoints.Count)
        {
            Vector3 nextPath = bezierPoints[currentbezierPoint];

            transform.LookAt(nextPath);
            transform.position = Vector3.MoveTowards(transform.position, nextPath, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPath) < 0.01f)
            {
                currentbezierPoint++;
            }
        }
    }
    void GenerateBezierPoints()
    {
        int groundPathPoint = 0;
        while (groundPathPoint <= groundPath.Count-4) 
        {
            Vector3 p0 = groundPath[groundPathPoint];
            Vector3 p1 = groundPath[groundPathPoint + 1];
            Vector3 p2 = groundPath[groundPathPoint + 2];
            Vector3 p3 = groundPath[groundPathPoint + 3];

            for (int i = 0; i < 20; i++)
            {
                float segment = (0.05f * i + 0.05f);
                Vector3 bezierPoint = BezierCurve.GetPoint(p0, p1, p2, p3, segment);
                bezierPoints.Add(bezierPoint);
            }
            groundPathPoint += 3;
        }

        while(groundPathPoint < groundPath.Count - 1)
        {
            bezierPoints.Add(groundPath[groundPathPoint+1]);
            groundPathPoint++;
        }
    }
    void OnGround()
    {
        for (int i = 0; i < path.Count; i++)
        {
            groundPath.Add(path[i].transform.position + new Vector3(0, 1, 0)); 
        }
    }

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        mapgraph = GameObject.Find("GridManager").GetComponent<MapGraph>();
        path = mapgraph.path;

        OnGround();
        GenerateBezierPoints();
    }
    
    void Update()
    {
        Move();
        if (Mathf.Abs(transform.position.x - bezierPoints[bezierPoints.Count - 1].x) < 0.01f && Mathf.Abs(transform.position.z - bezierPoints[bezierPoints.Count - 1].z) < 0.01f)
        {
            playerAnim.SetFloat("Speed_f", 0.0f);
        }

    }
}

