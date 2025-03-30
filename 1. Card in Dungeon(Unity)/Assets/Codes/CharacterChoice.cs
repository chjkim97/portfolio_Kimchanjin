using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterChoice : MonoBehaviour
{
    private RelicManager relicManager;

    public int job = 0;
    //0 전사, 1 도적, 2 마법사
    public Button[] characterButtons;
    public TextMeshProUGUI characterDescriptionText;
    public Button DecideButton;
    public Image mainImage;
    public Sprite[] buttonSprites;
    public GameObject class1; // 전사
    public GameObject class2; // 도적
    public GameObject class3; // 마법사
    
    private string[] characterDescriptions = {
        "전사는 체력을 소모하여 강력한 효과를 사용합니다. 방어도를 쉽게 올릴 수 있습니다.",
        "도적은 카드 순환을 통해 적을 제압합니다.",
        "마법사는 강력한 마법을 사용합니다. 마나의 최대치를 늘릴 수 있습니다."
        //설명은 추후 추가
    };

    private int selectedCharacterIndex = 0;

    private void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        relicManager = RelicManager.Instance;
        // 각 캐릭터 버튼에 클릭 이벤트 추가
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int characterIndex = i;
            characterButtons[i].onClick.AddListener(() => ShowCharacterDescription(characterIndex));
            //characterButtons[i].onClick.AddListener(() => UpdateMainImage(characterIndex));
        }

        //  버튼 클릭 이벤트 추가
        DecideButton.onClick.AddListener(() =>
        {
            SelectCharacter();
            DecideCharacter();
        });

        // 초기 상태에서 캐릭터 1의 설명을 출력
        ShowCharacterDescription(selectedCharacterIndex);
    }

    private void ShowCharacterDescription(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < characterDescriptions.Length)
        {
            characterDescriptionText.text = characterDescriptions[characterIndex];
            selectedCharacterIndex = characterIndex;
        }
        else
        {
            Debug.LogError("Invalid character index.");
        }
        UpdateClassVisibility();
    }

    private void SelectCharacter()
    {
        Debug.Log("Selected Character: " + (selectedCharacterIndex + 1));
        job = selectedCharacterIndex;



    }

    private void DecideCharacter()
    {
        // 게임 시작 또는 다음 단계로 넘어가는 동작을 추가
        Debug.Log("캐릭터 " + job.ToString() + "선택");
        Debug.Log("Deciding...");
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.character = job;
        // 플레이어의 유물 리스트에 유물 추가
        RelicManager.Instance.AddRelicToPlayer(job + 1);
        //유물테스트
        // RelicManager.Instance.AddRelicToPlayer(15);
        // RelicManager.Instance.AddRelicToPlayer(30);
        // RelicManager.Instance.AddRelicToPlayer(38);



        // 플레이어의 유물 리스트를 저장
        RelicManager.Instance.SavePlayerRelics();
        relicManager.AcquiredRelicEffect(job + 1);
        CardList cardList = FindObjectOfType<CardList>();
        cardList.ClearItems();
        for (int i = 0; i < 4; i++)
        {
           //cardList.AddCardToPlayer(1);
           //cardList.AddCardToPlayer(3);
        }
        if (job == 0) //전사
        {
            cardList.AddCardToPlayer(4);
            cardList.AddCardToPlayer(5);
        }
        else if (job == 1)//도적
        {
            cardList.AddCardToPlayer(1);
            cardList.AddCardToPlayer(11);
            cardList.AddCardToPlayer(21);
            cardList.AddCardToPlayer(8);
            cardList.AddCardToPlayer(1);
            cardList.AddCardToPlayer(212);
            cardList.AddCardToPlayer(305);
            cardList.AddCardToPlayer(208);
            cardList.AddCardToPlayer(4);
            cardList.AddCardToPlayer(5);
            cardList.AddCardToPlayer(309);
            cardList.AddCardToPlayer(312);
            cardList.AddCardToPlayer(102);
            cardList.AddCardToPlayer(312);

        }
        else if (job == 2)//마법사
        {
            cardList.AddCardToPlayer(309);
            cardList.AddCardToPlayer(312);
            cardList.AddCardToPlayer(301);
            cardList.AddCardToPlayer(310);
            cardList.AddCardToPlayer(315);
        }
        int randomRelicLevel = PlayerPrefs.GetInt("RandomRelicLevel", 0);
        if (randomRelicLevel >= 1)
        {
            for (int reliccount = 0; reliccount < randomRelicLevel; reliccount++)
                relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
        }
        
        SceneManager.LoadScene("StartReward");
    }
    private void UpdateClassVisibility()
    {
        class1.SetActive(selectedCharacterIndex == 0); // 전사
        class2.SetActive(selectedCharacterIndex == 1); // 도적
        class3.SetActive(selectedCharacterIndex == 2); // 마법사
    }
    private void UpdateMainImage(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < buttonSprites.Length)
        {
            mainImage.sprite = buttonSprites[characterIndex];
        }
        else
        {
            Debug.LogError("Invalid character index for sprite.");
        }
    }
}
