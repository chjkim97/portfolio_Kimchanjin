using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehaivor : MonoBehaviour
{

    public GameObject hungerBar;
    Vector3 hungerBarPos;
    Vector3 hungerBarOffset = new Vector3(0, 3.0f, 0);
    public HungerBarController hungerBarController;

    public FoodSpawn foodSpawn;

    public float animalSpeed = 3.0f;
    public float animalTrunSpeed = 70.0f;
    private float randomDirection;
    public float directionChangeInterval = 3.0f;
    public float nextDirectionChangeInterval = 1.0f;
    public float zRangeUp = 30.0f;
    public float zRangeDown = -11.0f;
    public float xRange = 22.0f;

    float dotProductAnimal;
    public Rigidbody rb;
    bool isColliding;
   

    float detectionRange = 5.0f;
    Transform targetFood;
    Vector3 direction;

    public AnimalSpawn animalSpawn;
  

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
            transform.Translate(Vector3.forward * Time.deltaTime * animalSpeed);

           
        }
        else
        {
            transform.Rotate(Vector3.up, Time.deltaTime * animalTrunSpeed * randomDirection);
            transform.Translate(Vector3.forward * Time.deltaTime * animalSpeed);
        
        }

        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
           
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.z < zRangeDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeDown);
        }
        if (transform.position.z > zRangeUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeUp);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            foodSpawn.foodCount--;
            if(hungerBarController.hungerBarNumber > 0) 
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
            dotProductAnimal = Vector3.Dot(thisFoward,otherFoward);
           
            if(dotProductAnimal < 0 && dotProductAnimal > -1)
            {
                Debug.Log("dotproduct1");
                Vector3 oppositeDirection = -transform.forward;
                Quaternion newRotation = Quaternion.LookRotation(oppositeDirection);
                transform.rotation = newRotation;
             }
          
             else if(dotProductAnimal >= 0)
            {
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
    public void StopMovement()
    {
        Debug.Log("Stopping movement.");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void AnimalDie()
    {
        if(hungerBarController.hungerBarNumber == hungerBarController.segmentRenderers.Length)
        {
            Destroy(gameObject);
            Destroy(hungerBar);
            animalSpawn.currentAnimalCount--;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hungerBar = Instantiate(hungerBar, transform.position + hungerBarOffset, Quaternion.identity);
        hungerBarController = hungerBar.GetComponent<HungerBarController>();
        foodSpawn = GameObject.Find("FoodSpawnManager").GetComponent<FoodSpawn>();
        animalSpawn = GameObject.Find("AnimalSpawnManager").GetComponent<AnimalSpawn>();
       
        rb = GetComponent<Rigidbody>();
     
        if (rb == null) { Debug.Log("Rigidnull"); }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerBar();
        if(Time.time >= nextDirectionChangeInterval)
        {
            RandomDirection();
            nextDirectionChangeInterval = directionChangeInterval + Time.time;
        }


        if (!isColliding)
        {
            AnimalMoving();
        }
        AnimalDie();
    }
}
