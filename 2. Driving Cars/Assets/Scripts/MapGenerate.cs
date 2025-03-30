using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapGenerate : MonoBehaviour
{

    [Serializable]
    public struct RoadData
    {
        public Vector3 start;
        public Vector3 end;
        public Vector3[] curvePoints;
        public Vector3 center;
        public int degree;
        public double radians;
        

        public RoadData(Vector3 s, Vector3 e)
        {
            degree = 0;
            radians = 4.5 * Math.PI / 180.0;
            start = s;
            end = e;
            curvePoints = null;
            center = Vector3.zero;
        }

        public RoadData( Vector3 o, int d)
        {
            degree = d;
            radians = 4.5 * Math.PI / 180.0;
            start = Vector3.zero;
            end = Vector3.zero;
            curvePoints = new Vector3[19];
            center = o;
            CurvePoint(center, curvePoints, degree, radians);

        }

        public void CurvePoint(Vector3 o, Vector3[] points, int d, double rad)
        {
            if (d == 0)
            {
                start = o + new Vector3(-30f, 0f, 0f);
                end = o + new Vector3(0f, 0f, 30f);
                for(int i = 1; i < 20; i++)
                {
                    points[i - 1] = o + new Vector3(-(float)Math.Cos(rad * i) * 30, 0f, (float)Math.Sin(rad * i) * 30);
                }
            }
            else if (d == 90)
            {
                start = o + new Vector3(0f, 0f, 30f);
                end = o + new Vector3(30f, 0f, 0f);
                for (int i = 1; i < 20; i++)
                {
                    points[i - 1] = o + new Vector3((float)Math.Sin(rad * i) * 30, 0f, (float)Math.Cos(rad * i) * 30);
                }
            }
            else if (d == 180)
            {
                start = o + new Vector3(30f, 0f, 0f);
                end = o + new Vector3(0f, 0f, -30f);
                for (int i = 1; i < 20; i++)
                {
                    points[i - 1] = o + new Vector3((float)Math.Cos(rad * i) * 30, 0f, -(float)Math.Sin(rad * i) * 30);
                }
            }
            else if (d == 270)
            {
                start = o + new Vector3(0f, 0f, -30f);
                end = o + new Vector3(-30f, 0f, 0f);
                for (int i = 1; i < 20; i++)
                {
                    points[i - 1] = o + new Vector3(-(float)Math.Sin(rad * i) * 30, 0f, -(float)Math.Cos(rad * i) * 30);
                }
            }
            else if (d == 275)
            {
                start = o + new Vector3(-30f, 0f, 0f);
                end = o + new Vector3(0f, 0f, -30f);
                for (int i = 1; i < 20; i++)
                {
                    points[i - 1] = o + new Vector3(-(float)Math.Cos(rad * i) * 30, 0f, -(float)Math.Sin(rad * i) * 30);
                }
            }
            else if (d == 280)
            {
                start = o + new Vector3(0f, 0f, -30f);
                end = o + new Vector3(30f, 0f, 0f);
                for (int i = 1; i < 20; i++)
                {
                    points[i - 1] = o + new Vector3((float)Math.Sin(rad * i) * 30, 0f, -(float)Math.Cos(rad * i) * 30);
                }
            }
        }
    }

    

    public GameObject[] roadLayout;
    public List<RoadData> roadDataList = new List<RoadData>();


    private void H_Road(int numberOfWideRoad, int VectorX, int VectorZ)
    {
        for (int i = 0; i < numberOfWideRoad; i++)
        {
            Vector3 spawnPos = new Vector3(VectorX + (39 * i), 0.01f, VectorZ);
            int Index = 0;
            Instantiate(roadLayout[Index], spawnPos, roadLayout[Index].transform.rotation);
        }
    }

    private void V_Road(int numberOfWideRoad, int VectorX, int VectorZ)
    {
        for (int i = 0; i < numberOfWideRoad; i++)
        {
            Vector3 spawnPos = new Vector3(VectorX , 0.01f, VectorZ + (39 * i));
            int Index = 1;
            Instantiate(roadLayout[Index], spawnPos, roadLayout[Index].transform.rotation);
        }
    }

    private void CurveRoad(float Rotate, int VectorX, int VectorZ)
    {
        Quaternion rotation = Quaternion.Euler(0, Rotate, 0);
        Vector3 spawnPos = new Vector3(VectorX, 0.01f, VectorZ);
        int Index = 2;
        Instantiate(roadLayout[Index], spawnPos, rotation);
    }

    
    
       void Start()
    {
        Resources.UnloadUnusedAssets();
       
        CurveRoad(90, 95, 40);
        CurveRoad(-90, 17, 232);
        H_Road(5, -100, 1);
        CurveRoad(0, 95, 232);
        V_Road(5, 115, 76);
        V_Road(2, -23,193);
        V_Road(3, -141, 78);

        CurveRoad(270, -102, 155);
        CurveRoad(90, -43, 155);
        CurveRoad(-180, -43, 155);
        CurveRoad(0, -102, 155);

        H_Road(2, 17, 252);
        CurveRoad(-180, -100, 41);
        
        roadDataList.Add(new RoadData(new Vector3(-13f, 0.01f, 154f), new Vector3(-13f, 0.01f, 232f)));
        roadDataList.Add(new RoadData(new Vector3(17f, 0.01f, 232f), 0));
        roadDataList.Add(new RoadData(new Vector3(17.5f, 0.01f, 262f), new Vector3(94f, 0.01f, 262f)));
        roadDataList.Add(new RoadData(new Vector3(94f, 0.01f, 232f), 90));
        roadDataList.Add(new RoadData(new Vector3(123f, 0.01f, 232f), new Vector3(123f, 0.01f, 41f)));
        roadDataList.Add(new RoadData(new Vector3(95f, 0.01f, 41f), 180));
        roadDataList.Add(new RoadData(new Vector3(95f, 0.01f, 11f), new Vector3(-100f, 0.01f, 11f)));
        roadDataList.Add(new RoadData(new Vector3(-100f, 0.01f, 41f), 270));
        roadDataList.Add(new RoadData(new Vector3(-130f, 0.01f, 41f), new Vector3(-130f, 0.01f, 153f)));
        roadDataList.Add(new RoadData(new Vector3(-98.5f, 0.01f, 153f), 0));
        roadDataList.Add(new RoadData(new Vector3(-98.5f, 0.01f, 153f), 90));
        roadDataList.Add(new RoadData(new Vector3(-41f, 0.01f, 152f), 275));
        roadDataList.Add(new RoadData(new Vector3(-41f, 0.01f, 152f), 280));
    }

    
    void Update()
    {
        Resources.UnloadUnusedAssets();
    }
}
