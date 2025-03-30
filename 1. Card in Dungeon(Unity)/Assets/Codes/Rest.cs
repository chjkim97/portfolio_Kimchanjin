using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class Rest : MonoBehaviour
{
    private int currentStarLevel; //현재 별 레벨
    private int recoveryAmount=0;
    public TextMeshProUGUI recoveryText; // 체력 회복량을 표시할 TextMeshPro 객체

    void Start()
    {
        Button restBtn = GameObject.Find("RestButton").GetComponent<Button>();
        restBtn.onClick.AddListener(TaskOnClickRest);

        Button cardBtn = GameObject.Find("CardButton").GetComponent<Button>();
        cardBtn.onClick.AddListener(TaskOnClickCard);
        
        currentStarLevel = PlayerPrefs.GetInt("CurrentStarLevel", 0);

        recoveryText = GameObject.Find("RecoveryText").GetComponent<TextMeshProUGUI>(); 
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        
        recoveryAmount = (int)(playerStats.maxHealth * 0.3 + playerStats.restrelic * 10);
        if(currentStarLevel>=12)
        {
            recoveryAmount-=10;
        }
        recoveryText.text = $"체력을 {recoveryAmount} 회복한다."; // 회복량 출력

    }

    void TaskOnClickRest()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        
        playerStats.currentHealth += recoveryAmount;

        if (playerStats.currentHealth > playerStats.maxHealth)
        {
            playerStats.currentHealth = playerStats.maxHealth;
        }


        RandomMap(playerStats.Rand_Map_Gene);
    }

    void TaskOnClickCard()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        
        Debug.Log("카드 보상을 얻는다");
        playerStats.enemytype = 0;
        SceneManager.LoadScene("BattleReward");

        //RandomMap(playerStats.Rand_Map_Gene);
    }

    public void RandomMap(int number)
    {
        /*
        if(number == 1)
        {
            SceneManager.LoadScene("Map0");
        }
        else if (number == 2)
        {
            SceneManager.LoadScene("Map1");
        }
        else if (number == 3)
        {
            SceneManager.LoadScene("Map2");
        }
        else if (number == 4)
        {
            SceneManager.LoadScene("Map3");
        }
        else 
        {
            SceneManager.LoadScene("Map4");
        }
        */
                   SceneManager.LoadScene("TestRandomMap");

        Debug.Log("Go to randommap");
    }
}
