using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapGenerate;

public class AutoMoveFoward : MonoBehaviour
{
    public MapGenerate mapGenerator;
    private List<Vector3> waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 10.0f;
 
    void Start()
    {
        mapGenerator = GameObject.FindObjectOfType<MapGenerate>();
        waypoints = new List<Vector3>();

        for (int i = 0; i < mapGenerator.roadDataList.Count; i++) 
        {
            RoadData roadData = mapGenerator.roadDataList[i];
            waypoints.Add(roadData.start);
            if (roadData.curvePoints != null)
            {
                foreach (Vector3 point in roadData.curvePoints)
                {
                    waypoints.Add(point);
                }
            }
            waypoints.Add(roadData.end);
        }
        
    }
    void AutoMove()
    {
        if (waypoints != null && waypoints.Count > 1)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = 0;
                }
                transform.LookAt(waypoints[currentWaypointIndex]);
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        AutoMove();
    }
}
