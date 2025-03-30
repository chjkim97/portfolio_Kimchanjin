using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public GameObject Player;
    int CameraNumber = 0;
    Vector3 topView;
    Quaternion topRotation;

    public void CameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CameraNumber = (CameraNumber == 0) ? 1 : 0;
        }
    }
    public void CameraType()
    {
        if (CameraNumber == 1)
        {
            Vector3 offset1;
            Vector3 offset2;
            Vector3 lookAtPosition;
            offset1 = Player.transform.forward;
            offset2 = Player.transform.forward*2;
            offset1.y += 2.0f;
            offset2.y += 1.0f;
            lookAtPosition = Player.transform.position + 2 * offset2;

            transform.position = Player.transform.position + offset1;
            transform.LookAt(lookAtPosition);
        }
        else if (CameraNumber == 0) 
        {

            transform.position = topView;
            transform.rotation = topRotation;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        topView = new Vector3(0, 60, 10);
        topRotation = Quaternion.Euler(90, 90, 90);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraSwitch();
        CameraType();

    }
}
