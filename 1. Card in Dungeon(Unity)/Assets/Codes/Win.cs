using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class Win : MonoBehaviour
{
    public Button exitButton;
    public Button retryButton;

    private int currentStarLevel;
  

    void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
        retryButton.onClick.AddListener(RetryGame);
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        relicManager.RemoveAllPlayerRelics();
        DestroyAllDontDestroyOnLoadObjects();
        
    }

    void ExitGame()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    }
    public static void DestroyAllDontDestroyOnLoadObjects()
    {
        // DontDestroyOnLoad 씬에 있는 모든 오브젝트를 담을 리스트
        List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

        // 모든 GameObject를 찾아 리스트에 추가
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                dontDestroyOnLoadObjects.Add(obj);
            }
        }

        // 모든 DontDestroyOnLoad 오브젝트를 제거
        foreach (GameObject obj in dontDestroyOnLoadObjects)
        {
            Destroy(obj);
        }
    }
    void RetryGame()
    {
        SceneManager.LoadScene("GameStart");
    }
}
