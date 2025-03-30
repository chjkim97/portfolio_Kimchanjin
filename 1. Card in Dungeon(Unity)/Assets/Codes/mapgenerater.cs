// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Rendering.Universal;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class mapgenerater : MonoBehaviour
// {
    
//     public Button Rand_Map_Gene_Button;

//     void Start()
//     {
//         PlayerStats playerStats = FindObjectOfType<PlayerStats>();

//         Rand_Map_Gene_Button = GameObject.Find("MapGenerate").GetComponent<Button>();
//         Rand_Map_Gene_Button.onClick.AddListener(() => RandomMap(playerStats.Rand_Map_Gene));

//     }
//     public void RandomMap(int number)
//     {
//         if(number == 1)
//         {
//             SceneManager.LoadScene("Map0");
//         }
//         else if (number == 2)
//         {
//             SceneManager.LoadScene("randommap 1");
//         }
//         else if (number == 3)
//         {
//             SceneManager.LoadScene("randommap 2");
//         }
//         else if (number == 4)
//         {
//             SceneManager.LoadScene("randommap 3");
//         }
//         else 
//         {
//             SceneManager.LoadScene("randommap 4");
//         }
//         Debug.Log("Go to randommap");
//     }
// }
