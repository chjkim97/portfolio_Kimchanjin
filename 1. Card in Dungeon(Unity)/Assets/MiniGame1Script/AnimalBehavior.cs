using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    public GameObject hungerBar;
    Vector3 hungerBarPos;
    Vector3 hungerBarOffset = new Vector3(-1.0f,10.0f,-1.0f);
    public HungerBarController hungerBarController;

    public float animalSpeed = 5.0f;
    public float animalTrunSpeed = 70.0f;
    
    private float randomDirection;
    public float directionChangeInterval = 3.0f;
    public float nextDirectionChangeInterval = 1.0f;


    public float xRangeLeft = -73.0f;
    public float xRangeRight = 98.0f;

    public float yRangeUp = 25.0f;
    public float yRangeDown = -48.0f;

    float dotProductAnimal;
    public Rigidbody2D rb;
    bool isColliding = false;


    float detectionRange = 5.0f;
    Transform targetFood;
    Vector3 direction;


    public void UpdateHungerBar()
    {
        hungerBar.transform.position = transform.position + hungerBarOffset;
    }
    public void RandomDirection()
    {
        randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
    }

    public void AnimalMoving()
    {

        Vector3 moveDirection;

        Collider[] foodObjects = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Food"));
        if (foodObjects.Length > 0)
        {
            targetFood = foodObjects[0].transform;
            float closestDistance = Vector3.Distance(transform.position, targetFood.position);

            foreach (Collider foodObject in foodObjects)
            {
                float distance = Vector3.Distance(transform.position, foodObject.transform.position);
                if (distance < closestDistance)
                {
                    targetFood = foodObject.transform;
                    closestDistance = distance;
                }
            }
            Vector3 direction = targetFood.position - transform.position;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * animalSpeed);
            moveDirection = transform.up;
            Debug.DrawRay(transform.position, moveDirection * 2f, Color.red); // 전방벡터 표시
            transform.Translate(moveDirection * Time.deltaTime * animalSpeed);


        }
        
        else
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * animalTrunSpeed * randomDirection);
            moveDirection = transform.up;  // 2D에서는 transform.up이 전방을 의미
            Debug.DrawRay(transform.position, moveDirection * 2f, Color.red); // 전방벡터 표시
            transform.Translate(moveDirection * Time.deltaTime * animalSpeed);
        }
        
        if (transform.position.x < xRangeLeft)
        {
            transform.position = new Vector3(xRangeLeft, transform.position.y, 0);

        }
        if (transform.position.x > xRangeRight)
        {
            transform.position = new Vector3(xRangeRight, transform.position.y, 0);
        }

        if (transform.position.y < yRangeDown)
        {
            transform.position = new Vector3(transform.position.x, yRangeDown, 0);
        }
        if (transform.position.y > yRangeUp)
        {
            transform.position = new Vector3(transform.position.x, yRangeUp, 0);
        }
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            
            if (hungerBarController.hungerBarNumber > 0)
            {
                hungerBarController.segmentRenderers[hungerBarController.hungerBarNumber].material.color = Color.green;
                hungerBarController.hungerBarNumber--;
                Debug.Log("Hb1");
            }
            if (hungerBarController.hungerBarNumber > 5)
            {
                hungerBarController.segmentRenderers[hungerBarController.hungerBarNumber].material.color = Color.green;
                hungerBarController.hungerBarNumber--;
                Debug.Log("Hb2");
            }

        }
        if (other.CompareTag("Animal"))
        {
            Vector3 thisFoward = transform.forward;
            Vector3 otherFoward = other.transform.forward;
            thisFoward.y = 0.0f;
            otherFoward.y = 0.0f;
            dotProductAnimal = Vector3.Dot(thisFoward, otherFoward);

            if (dotProductAnimal < 0 && dotProductAnimal > -1)
            {
                Debug.Log("dotproduct1");
                Vector3 oppositeDirection = -transform.forward;
                Quaternion newRotation = Quaternion.LookRotation(oppositeDirection);
                transform.rotation = newRotation;
            }

            else if (dotProductAnimal >= 0)
            {
                // 아래는 수정중
                /* isColliding = true;
                 Debug.Log("dotproduct2");
                 //transform.position += Vector3.zero;

                 StopMovement();*/
                Quaternion newRotation = Quaternion.Euler(0, 30, 0) * transform.rotation;
                transform.rotation = newRotation;
                Debug.Log("dotproduct2");
            }
        }
    }



    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            isColliding = false;
        }
    }

    public void AnimalGroggy()
    {
        if (hungerBarController.hungerBarNumber == hungerBarController.segmentRenderers.Length)
        {
            rb.velocity = Vector2.zero;
        }
    }




    void Start()
    {
        hungerBar = Instantiate(hungerBar, transform.position + hungerBarOffset, Quaternion.identity);
        hungerBarController = hungerBar.GetComponent<HungerBarController>();
       

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerBar();
        if (Time.time >= nextDirectionChangeInterval)
        {
            RandomDirection();
            nextDirectionChangeInterval = directionChangeInterval + Time.time;
        }


        if (!isColliding)
        {
            AnimalMoving();
        }
        AnimalGroggy(); 
    }


}
