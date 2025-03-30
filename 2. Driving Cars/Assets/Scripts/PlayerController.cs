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

        //transform �� �� ��ũ��Ʈ�� ����� ��ü�� ��ȯ�ǹ�(��ġ,ȸ�� ���) Vector3.foward �� (0,0,1) �ǹ�
        // Time.deltaTime�� ��� ��, ���Ǽ��迡�� ���� �귯�� �ð��� ����Ͽ� �����ϰ� ���ư����� ��
        // Rotate�Լ��� ���� ������ -> ù���� ���� ȸ����, �ι�° ���� ȸ���� up���ʹ� y�຤��
        transform.Translate(Vector3.forward*Time.deltaTime*speed * forwardInput);
        transform.Rotate(Vector3.up, Time.deltaTime * turnspeed* horizontalInput);
    }
}
