using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 사용하기 위해 추가
using UnityEngine.UI;
public class REvent1 : MonoBehaviour
{
    int ranevent = 1;
    public SpriteRenderer eventSpriteRenderer; // SpriteRenderer로 변경

    // TextMeshPro 변수 선언
    public TextMeshProUGUI textMeshPro1;
    public TextMeshProUGUI textMeshPro2;

    void Start()
    {
        ranevent = Random.Range(1, 7); 

        // 버튼 이벤트 연결
        Button restBtn = GameObject.Find("Button1").GetComponent<Button>();
        restBtn.onClick.AddListener(OnclickButton1);

        Button cardBtn = GameObject.Find("Button2").GetComponent<Button>();
        cardBtn.onClick.AddListener(OnclickButton2);

        // ranevent 값에 따라 버튼 텍스트 및 TextMeshPro 텍스트 변경
        UpdateButtonTexts(restBtn, cardBtn);
        UpdateTextMeshProTexts();

        // ranevent 값에 따라 이미지 변경
        UpdateEventImage();
    }

    void UpdateEventImage()
    {
        string spritePath = "";

        switch (ranevent)
        {
            case 1:
                spritePath = "Sceneimg/REvent1";
                break;
            case 2:
                spritePath = "Sceneimg/REvent2";
                break;
            case 3:
                spritePath = "Sceneimg/REvent3";
                break;
            case 4:
                spritePath = "Sceneimg/REvent4";
                break;
            case 5:
                spritePath = "Sceneimg/REvent5";
                break;
            case 6:
                spritePath = "Sceneimg/REvent6";
                break;
            default:
                Debug.LogError("Invalid ranevent value: " + ranevent);
                return;
        }

        Sprite eventSprite = Resources.Load<Sprite>(spritePath);
        if (eventSprite != null)
        {
            eventSpriteRenderer.sprite = eventSprite; // SpriteRenderer에 스프라이트 설정
        }
        else
        {
            Debug.LogWarning("Sprite not found at path: " + spritePath);
        }
    }

    void UpdateTextMeshProTexts()
    {
        // ranevent 값에 따라 두 개의 TextMeshPro 텍스트 변경
        switch (ranevent)
        {
            case 1:
                textMeshPro1.text = "악마와의 거래";
                textMeshPro2.text = "당신은 악마를 조우했습니다. 당신의 피로 악마와 거래할 수 있습니다.";
                break;
            case 2:
                textMeshPro1.text = "신의축복";
                textMeshPro2.text = "당신은 신의 축복을 받습니다. 당신에게 이로운 기운이 느껴집니다.";
                break;
            case 3:
                textMeshPro1.text = "신의 가호";
                textMeshPro2.text = "당신은 신의 가호를 받았습니다. 신성한 기운이 당신의 주위를 맴돕니다.";
                break;
            case 4:
                textMeshPro1.text = "축복받은 샘물";
                textMeshPro2.text = "당신은 샘물을 발견했습니다. 당신의 발걸음이 가벼워집니다.";
                break;
            case 5:
                textMeshPro1.text = "악마의 영토";
                textMeshPro2.text = "당신은 악마의 영토에 발을 들였습니다. 보물을 챙길 수 있지만 욕심을 부리면 저주를 받을 수 있습니다.";
                break;
            case 6:
                textMeshPro1.text = "저주받은 땅";
                textMeshPro2.text = "당신은 저주받은 땅에 도착했습니다. 카드를 한장 제물로 바치면 탈출할 수 있을 듯 합니다.";
                break;
            default:
                Debug.LogError("Invalid ranevent value: " + ranevent);
                break;
        }
    }

    void UpdateButtonTexts(Button restBtn, Button cardBtn)
    {
        // TextMeshPro 컴포넌트를 찾고 ranevent 값에 따라 텍스트 변경
        TextMeshProUGUI restBtnText = restBtn.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI cardBtnText = cardBtn.GetComponentInChildren<TextMeshProUGUI>();

        switch (ranevent)
        {
            case 1:
                restBtnText.text = "체력을 10 잃고 무작위 유물을 얻는다.";
                cardBtnText.text = "최대 체력을 5 잃고 무작위 유물을 얻는다.";
                break;
            case 2:
                restBtnText.text = "체력을 20 회복합니다.";
                cardBtnText.text = "최대 체력을 8 증가합니다.";
                break;
            case 3:
                restBtnText.text = "카드 보상을 얻습니다.";
                cardBtnText.text = "무작위 유물을 얻습니다.";
                break;
            case 4:
                restBtnText.text = "카드를 1장 제거합니다.";
                cardBtnText.text = "체력을 15 회복합니다.";
                break;
            case 5:
                restBtnText.text = "골드를 100 획득한다.";
                cardBtnText.text = "체력을 10 잃고 골드를 250 획득한다.";
                break;
            case 6:
                restBtnText.text = "카드를 1장 제거합니다.";
                cardBtnText.text = "체력을 5 잃습니다.";
                break;
            default:
                Debug.LogError("Invalid ranevent value: " + ranevent);
                break;
        }
    }

    void OnclickButton1()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        RelicManager relicManager = FindObjectOfType<RelicManager>();

        switch (ranevent)
        {
            case 1:
                playerStats.currentHealth -= 10;
                relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic()); // 랜덤 유물 얻는 함수
                break;
            case 2:
                playerStats.currentHealth += 20;
                if (playerStats.currentHealth > playerStats.maxHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                break;
            case 3:
                playerStats.enemytype = 0;
                SceneManager.LoadScene("BattleReward");
                break;
            case 4:
                SceneManager.LoadScene("CardView");
                break;
            case 5:
                playerStats.gold += 100;
                break;
            case 6:
                SceneManager.LoadScene("CardView");
                break;
            default:
                Debug.LogError("Invalid ranevent value: " + ranevent);
                break;
        }
        if (ranevent != 6 && ranevent != 3 && ranevent != 4)
                       SceneManager.LoadScene("TestRandomMap");

    }

    void OnclickButton2()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        switch (ranevent)
        {
            case 1:
                playerStats.maxHealth -= 5;
                if (playerStats.currentHealth > playerStats.maxHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic()); // 랜덤 유물 얻는 함수
                break;
            case 2:
                playerStats.maxHealth += 8;
                playerStats.currentHealth += 8;
                break;
            case 3:
                relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
                break;
            case 4:
                playerStats.currentHealth += 15;
                if (playerStats.currentHealth > playerStats.maxHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                break;
            case 5:
                playerStats.currentHealth -= 10;
                playerStats.gold += 250;
                break;
            case 6:
                playerStats.currentHealth -= 5;
                break;
            default:
                Debug.LogError("Invalid ranevent value: " + ranevent);
                break;
        }

                  SceneManager.LoadScene("TestRandomMap");

    }
}
