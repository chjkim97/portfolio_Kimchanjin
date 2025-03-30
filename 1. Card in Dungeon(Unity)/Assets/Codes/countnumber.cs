using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class countnumber : MonoBehaviour
{
  
    public TextMeshProUGUI SpinNumber;
    int count = 5;
    public void ButtonPressed()
    {
        Debug.Log("Wheel Spin");
        count--;
        SpinNumber.text = count + "";

    }



    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
