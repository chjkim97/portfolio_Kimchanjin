using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playersprite : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject class1; // 전사
    public GameObject class2; // 도적
    public GameObject class3; // 마법사
    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        
        class1.SetActive(playerStats.character == 0); // 전사
        class2.SetActive(playerStats.character == 1); // 도적
        class3.SetActive(playerStats.character == 2); // 마법사
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
