using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMapCameraController : MonoBehaviour
{

    private float horizontal;
    public float speed = 6.0f;
    public float xRangeleft = 6f;
    public float xRangeright = 48f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * Time.deltaTime * speed);

        if(transform.position.x < xRangeleft)
        {
            transform.position = new Vector3(xRangeleft, transform.position.y,transform.position.z);
        }
        if(transform.position.x > xRangeright) 
        {
            transform.position = new Vector3(xRangeright, transform.position.y, transform.position.z);
        }
    }
}
