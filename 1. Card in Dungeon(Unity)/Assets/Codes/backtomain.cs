using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class backtomain : MonoBehaviour
{

    public void BackToMain()
    {
        SceneManager.LoadScene("GameStart");
        Debug.Log("Back to main");
    }
}
