using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class RetryButton : MonoBehaviour
{
    public Button exitButton;
    public Button retryButton;

    private int currentStarLevel;
    public int startGoldLevel = 0;
    public int maxHealthLevel=0;

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
    // 모든 DontDestroyOnLoad 오브젝트 찾기
        
        // PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        // RelicManager relicManager = FindObjectOfType<RelicManager>();
        // relicManager.RemoveAllPlayerRelics();
        // playerStats.maxHealth = 70;
        // playerStats.currentHealth = 70;
        // playerStats.gold = 100;
        // playerStats.revive = 0;
        // playerStats.character = 0;
        // playerStats.maxMana = 3;
        // playerStats.currentMana = 3;
        // playerStats.shield = 0;
        // playerStats.shieldcounter = 0;
        // playerStats.basiccardarmor = 0;
        // playerStats.cardarmor = 0;
        // playerStats.enemynumber = 0;
        // playerStats.restrelic = 0;
        // playerStats.shoprelic = 0;
        // playerStats.power = 0;
        // playerStats.powercounter = 0;
        // playerStats.thorn = 0;
        // playerStats.shieldmanager = 0;
        // playerStats.extramana = 0;
        // playerStats.extrashield = 0;
        // playerStats.firesea = 0;
        // playerStats.weaken = 0;
        // playerStats.corrosion = 0;
        // playerStats.vulnerable = 0;
        // playerStats.nodraw = 0;
        // playerStats.burn = 0;
        // playerStats.mdraw = 0;
        // playerStats.attackcountrelic = 0;
        // playerStats.attackcount = 0;
        // currentStarLevel = PlayerPrefs.GetInt("CurrentStarLevel", 0);
        // Debug.Log("Current Star Level: " + currentStarLevel);
        
        // if(currentStarLevel>=9)
        // {
        //     playerStats.gold-=50;
        // }
        // if(currentStarLevel>=10)
        // {
        //     playerStats.maxHealth-=3;
        // }
        // startGoldLevel = PlayerPrefs.GetInt("StartGoldLevel", 0);
        // playerStats.gold+=startGoldLevel*50;
        // playerStats.maxHealthLevel = PlayerPrefs.GetInt("MaxHealthLevel", 0);
        // playerStats.maxHealth+=maxHealthLevel*3;
        // playerStats.currentHealth=playerStats.maxHealth;

        // if(currentStarLevel>=7)
        // {
        //     playerStats.currentHealth-=5;
        // }
        SceneManager.LoadScene("GameStart");
    }
}
