using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniGame2MonsterController : MonoBehaviour
{

    public float monsterSpeed = 10.0f;
    public float monsterTrunSpeed = 40.0f;
    
    private float randomDirection;
    public float directionChangeInterval = 3.0f;
    public float nextDirectionChangeInterval = 1.0f;

    public GameObject[] rewardPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextDirectionChangeInterval)
        {
            RandomDirection();
            nextDirectionChangeInterval = directionChangeInterval + Time.time;
        }

        MonsterMove();
    }

    void MonsterMove()
    {
        Vector3 moveDirection;

        transform.Rotate(Vector3.forward, Time.deltaTime * monsterTrunSpeed * randomDirection);
        moveDirection = transform.up;  // 2D에서는 transform.up이 전방을 의미
        Debug.DrawRay(transform.position, moveDirection * 2f, Color.red); // 전방벡터 표시
        transform.Translate(moveDirection * Time.deltaTime * monsterSpeed);
    }

    public void RandomDirection()
    {
        randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
    }

    void OnDestroy()
    {
        if (rewardPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, rewardPrefabs.Length);
            Instantiate(rewardPrefabs[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
