using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Startreward : MonoBehaviour
{
    public Button[] myButtons; // 여러 개의 버튼을 배열로 받을 수 있도록 수정
    public TextMeshProUGUI[] textFields; // TextMeshProUGUI로 변경
    private RelicManager relicManager;

    public GameObject RewardMenu;
    private void Start()
    {
        // 텍스트 옵션 리스트
        List<string> textOptions = new List<string> { "최대 체력을 10 잃고 무작위 유물 2개 획득", "최대 체력을 10 잃고 골드 360 획득", "최대 체력 +7", "체력을 5 잃고 카드 한 장 제거", "카드 보상 획득", "무작위 유물 획득", "골드 +100" };
        //List<string> textOptions = new List<string> { "카드 보상 획득", "카드 보상 획득", "카드 보상 획득", "카드 보상 획득", "카드 보상 획득" }; //-> 디버깅시 사용
        relicManager = RelicManager.Instance;
        // 각 버튼에 대해 무작위로 텍스트 선택 및 설정
        for (int i = 0; i < myButtons.Length; i++)
        {
            if (textOptions.Count > 0)
            {
                int randomIndex = Random.Range(0, textOptions.Count);
                string randomText = textOptions[randomIndex];
                UpdateButtonWithRandomText(myButtons[i], textFields[i], randomText);
                textOptions.RemoveAt(randomIndex);

                // 각 버튼에 대해 클릭 이벤트 추가
                AddButtonClickEvent(myButtons[i], randomText);
            }
            else
            {
                Debug.LogWarning("Not enough unique text options for all buttons.");
                break;
            }
        }


    }

    private void UpdateButtonWithRandomText(Button button, TextMeshProUGUI textField, string newText)
    {
        if (button != null && textField != null)
        {
            textField.text = newText;
        }
        else
        {
            Debug.LogError("Button or TextMeshProUGUI is not assigned in the inspector.");
        }
    }

    private void AddButtonClickEvent(Button button, string buttonText)
    {
        if (button != null)
        {
            button.onClick.AddListener(() => HandleButtonClick(buttonText));
        }
        else
        {
            Debug.LogError("Button is null.");
        }
    }

    private void HandleButtonClick(string buttonText)
    {
        Debug.Log("Button Clicked! Text: " + buttonText);

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (buttonText == "최대 체력 +7")
        {
            if (playerStats != null)
            {
                playerStats.maxHealth += 7;
                playerStats.currentHealth += 7;
            }
            else
            {
                Debug.LogError("PlayerStats not found in the scene.");
            }
        }
        else if (buttonText == "체력을 5 잃고 카드 한 장 제거")
        {
            if (playerStats != null)
            {
                playerStats.currentHealth -= 5;
                SceneManager.LoadScene("CardView");
            }
            else
            {
                Debug.LogError("PlayerStats not found in the scene.");
            }
        }
        else if (buttonText == "카드 보상 획득")
        {
            if (playerStats != null)
            {
                Debug.Log("카드 보상 획득");
                playerStats.enemytype = 0;
                SceneManager.LoadScene("BattleReward");
                return;
            }
            else
            {
                Debug.LogError("PlayerStats not found in the scene.");
            }
        }
        else if (buttonText == "무작위 유물 획득")
        {
            if (playerStats != null)
            {
                Debug.Log("무작위 유물 획득");
                relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
            }
            else
            {
                Debug.LogError("PlayerStats not found in the scene.");
            }
        }
        else if (buttonText == "골드 +100")
        {
            playerStats.gold += 100;
        }
        else if (buttonText == "최대 체력을 10 잃고 골드 360 획득")
        {
            playerStats.maxHealth -= 10;
            playerStats.currentHealth -= 10;
            playerStats.gold += 360;
        }
        else if (buttonText == "최대 체력을 10 잃고 무작위 유물 2개 획득")
        {
            playerStats.maxHealth -= 10;
            playerStats.currentHealth -= 10;
            relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
            relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
        }

        /* 이 부분은 삭제해도 무방하지만, 혹시 몰라서 남겨뒀음
        switch (playerStats.Rand_Map_Gene)
        {
            case 1:
                SceneManager.LoadScene("Map0");

                break;
            case 2:
                SceneManager.LoadScene("Map1");

                break;
            case 3:
                SceneManager.LoadScene("Map2");

                break;
            case 4:
                SceneManager.LoadScene("Map3");

                break;
            case 5:
                SceneManager.LoadScene("Map4");

                break;
        }
        */

        if (buttonText != "체력을 5 잃고 카드 한 장 제거")
            SceneManager.LoadScene("TestRandomMap");
    }
}