using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SpawnManager : MonoBehaviour
{

    public GameObject[] carPrefabs;
    Vector3 spawnPoint;

    public float spawnInterval = 6.0f;
    public float nextSpawnTime;
    int count = 0;
    int maxCarCount = 16;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector3(-13f, 0.01f, 144);
        nextSpawnTime = Time.time + spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCar(); 
            nextSpawnTime = Time.time + spawnInterval; 
        }
    }

    void SpawnCar()
    {  
        if(count < maxCarCount)
        {
            int carIndex = UnityEngine.Random.Range(0, carPrefabs.Length);
            Instantiate(carPrefabs[carIndex], spawnPoint, carPrefabs[carIndex].transform.rotation);
            count++;
        }
        return;
    }
}
