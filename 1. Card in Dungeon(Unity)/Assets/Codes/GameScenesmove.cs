using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameScenesmove : MonoBehaviour
{

    public void GameScenesCtrl()
    {
        SceneManager.LoadScene("CharacterChoice");
        Debug.Log("Game Scenes Go");
    }
  
}
