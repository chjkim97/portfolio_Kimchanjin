using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawn : MonoBehaviour
{
   
    public GameObject[] animalPrefabs;
    Vector3 spawnPoint;
    public float spawnInterval = 3.0f;
    public float nextSpawnTime;
    int start=0;
    int count;
    public int currentAnimalCount = 2;
    int maxAnimalCount = 20;

    public void SpawnAnimal()
    {

        if (start ==0)
        {
            for (int i = 0; i < currentAnimalCount; i++)
            {
                RandomSpawn();
            }
            start++;
        }
        else 
        {
            count = IncreasingProportion(currentAnimalCount);
            for (int i = 0; i < count; i++)
            {
                RandomSpawn();
            }

            currentAnimalCount += count;
        }
        Debug.Log(currentAnimalCount);
    }

    public int IncreasingProportion(int n)
    {
        float proportion = 0.5f;
        return Mathf.FloorToInt(n * proportion);
    }

    public void RandomSpawn()
    {
        float randomX = UnityEngine.Random.Range(-22.0f, 22.0f);
        float randomZ = UnityEngine.Random.Range(-11.0f, 30.0f);
        Vector3 spawnPoint = new Vector3(randomX, 0f, randomZ);
        int animalIndex = UnityEngine.Random.Range(0, animalPrefabs.Length);

        Instantiate(animalPrefabs[animalIndex], spawnPoint, animalPrefabs[animalIndex].transform.rotation);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAnimalCount < maxAnimalCount) {
            if (Time.time >= nextSpawnTime)
            {
                SpawnAnimal();
                nextSpawnTime = Time.time + spawnInterval;
            }
        }
    }
}
