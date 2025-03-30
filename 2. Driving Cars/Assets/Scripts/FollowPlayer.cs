using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    int CameraNumber;
   
    Quaternion NormalCameraRotation;
    public void CameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (CameraNumber == 2) { CameraNumber = 0; }
            else { CameraNumber++; }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        NormalCameraRotation = Quaternion.Euler(10, 0, 0);
        CameraNumber = 0;
    }

    // Update is called once per frame
    void LateUpdate() // LateUpdate -> 모든 스크립트의 UPdate가 실행된 이후 실행된다.
    {
        CameraSwitch();
        
        if (CameraNumber == 0)
        {
            Vector3 offset;
            Vector3 lookAtPosition = Player.transform.position + new Vector3(0,2.5f,0);
            offset = -Player.transform.forward * 11f;
            offset.y += 10f;

            transform.position = Player.transform.position + offset ;
            transform.LookAt(lookAtPosition);
        }
        else if (CameraNumber == 1)
        {
            
            Vector3 offset;
            Vector3 lookAtPosition = Player.transform.position + new Vector3(0, 3f, 0);
            offset = -Player.transform.forward;
            offset.y += 3f;
            
            transform.position = Player.transform.position + offset;
            transform.LookAt(lookAtPosition);
        }
        else if (CameraNumber ==2)
        {
            Vector3 offset;
            offset = -Player.transform.forward;
            offset.y += 30f;
            transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = Player.transform.position + offset;
            transform.LookAt(Player.transform);

        }
    }
}


