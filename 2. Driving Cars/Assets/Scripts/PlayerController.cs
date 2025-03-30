using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
    public float speed = 20;
    public float turnspeed = 60;
    public float forwardInput;
    public float horizontalInput;
    Vector3 RevivalPosition;
    Vector3 Revivalpoint1;
    
    Quaternion RevivalRotation;
    float fallPoint = -5.0f;
    

    public void Revival_Point1()
    {
        if (transform.position.y < 0 && transform.position.y> -0.05f )
        {
            RevivalPosition = transform.forward;
            Revivalpoint1 = transform.position + new Vector3(0, 0.3f, 0);
            RevivalRotation = transform.rotation;
            Debug.Log("Revival_Point1");
        }
    }
    public void Revival()
    {
        if (transform.position.y < -5.1f) 
        {
            RevivalRotation.x = 0f;
            RevivalRotation.z = 0f;
            transform.position = Revivalpoint1 - RevivalPosition*10;
            transform.rotation = RevivalRotation;
            
            Debug.Log("Revival_Point3");
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        RevivalPosition = new Vector3();
        RevivalRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        Revival_Point1();
        Revival();
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        //transform 은 이 스크립트가 연결된 객체의 변환의미(위치,회전 등등) Vector3.foward 는 (0,0,1) 의미
        // Time.deltaTime은 재생 후, 현실세계에서 실제 흘러간 시간을 고려하여 일정하게 나아가도록 함
        // Rotate함수는 축을 돌린다 -> 첫번쨰 인자 회전축, 두번째 인자 회전각 up벡터는 y축벡터
        transform.Translate(Vector3.forward*Time.deltaTime*speed * forwardInput);
        transform.Rotate(Vector3.up, Time.deltaTime * turnspeed* horizontalInput);
    }
}
