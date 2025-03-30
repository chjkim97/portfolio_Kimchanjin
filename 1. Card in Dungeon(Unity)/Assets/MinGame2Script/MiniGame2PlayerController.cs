using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame2PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float horizontal;
    private float vertical;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }


    void PlayerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * Time.deltaTime * speed);

        vertical = Input.GetAxis("Vertical");
        transform.Translate(Vector2.up * vertical * Time.deltaTime * speed);

    }
}
