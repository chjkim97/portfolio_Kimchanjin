using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 동물이 음식 먹으면, foodCount 줄어들게 해야댐
public class FoodSpawn : MonoBehaviour
{

    public GameObject[] foodPrefabs;
    Vector3 spawnPoint;

    public float spawnInterval = 3.0f;
    public float nextSpawnTime;
    public int foodCount = 0;
    public int maxFoodCount = 25;
    bool isSpawning = true;
    // Start is called before the first frame update
    public void SpawnFood()
    {
        if (foodCount < maxFoodCount)
        {
            float randomX = UnityEngine.Random.Range(-22.0f, 22.0f);
            float randomZ = UnityEngine.Random.Range(-11.0f, 30.0f);
            
            int foodIndex = UnityEngine.Random.Range(0, foodPrefabs.Length);

            Vector3 spawnPoint = new Vector3(randomX, 0.2f, randomZ);
            
            Instantiate(foodPrefabs[foodIndex], spawnPoint, foodPrefabs[foodIndex].transform.rotation);
            foodCount++;
        }
        else 
        { 
            CancelInvoke("SpawnFood");
            isSpawning = false;
        }
    }
    
    
    void Start()
    {
        InvokeRepeating("SpawnFood", 2, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (foodCount < maxFoodCount && !isSpawning)
        {
            InvokeRepeating("SpawnFood", 0, 0.5f);
            isSpawning = true;
        }
    }
}
