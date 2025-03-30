using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 
//using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI JobText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI DiaText;
    public Image sprite1;  // 첫 번째 스프라이트 이미지
    public Image sprite2;  // 두 번째 스프라이트 이미지
    public GameObject mainImage; 
    public Button backMainButton; 
    public Button yesButton; 
    public Button noButton; 
    public string job;
    private static UIManager _instance;
    public Button KeywardBook;
    public Image KeywardText1;  // KeywardText1 추가
    public Image KeywardText2;  // KeywardText2 추가

    // public Button RelicButton;
    public Image RelicImage;
    public TextMeshProUGUI RelicImageText;

    public Button NextButton;  // Next 버튼
    public Button PreviousButton; // Previous 버튼
    public List<Image> KeywardTexts; // 여러 개의 텍스트를 리스트로 저장
    private int currentKeywardIndex = 0; // 현재 보여지는 텍스트의 인덱스
    public GameObject relicPrefab; // Unity Inspector에서 유물 프리팹을 할당.
    public Transform relicParent;  // 유물이 생성될 부모 오브젝트 (예: RelicText).

    public ItemSO itemSO;
    List<Item> itemBuffer;
    public Button MyCards;
    [SerializeField] GameObject cardPrefabs;
    [SerializeField] GameObject uicardPrefabs;

    [SerializeField] List<Card> Deck;
    Card selectCard;
    // bool MyCardViewClick = true;

    [SerializeField] Transform cardSpawnPoint;

    Startreward rewardMenu;
    GameObject rewardMenuUI;

    public Button backToMenu;
    public GameObject BackToMenuUI;

    public GameObject mycardsInven;
    public RectTransform scrollContent;

    public bool invenClicked = false;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // UIManager 자체를 유지

            // RelicText의 부모 오브젝트를 DontDestroyOnLoad로 설정하여 자식 오브젝트도 유지
            if (relicParent != null)
            {
                DontDestroyOnLoad(relicParent.transform.parent.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        rewardMenu = FindObjectOfType<Startreward>();
        if (rewardMenu != null)
        {
            rewardMenuUI = rewardMenu.RewardMenu;
            Debug.Log("StartReward found and ready to use.");
        }

        MyCards.onClick.AddListener(() => MyCardsListButton());
        KeywardBook.onClick.AddListener(() => ShowKeywardBook());



        // RelicButton.onClick.AddListener(() => ToggleRelicImage());
        backToMenu.onClick.AddListener(() => BacktoMenu());

        NextButton.onClick.AddListener(ShowNextKeywardText); // Next 버튼 클릭 리스너
        PreviousButton.onClick.AddListener(ShowPreviousKeywardText); // Previous 버튼 클릭 리스너

        SetupItemBuffer();
        DeckSetup();
        DisplayRelics();
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        switch (playerStats.character)
        {
            case 0:
                job = "전사";
                break;
            case 1:
                job = "도적";
                break;
            case 2:
                job = "마법사";
                break;
            default:
                job = "알 수 없음";
                break;
        }
        InvokeRepeating("MyFunction", 0, 0.2f);
        RelicManager relicManager = FindObjectOfType<RelicManager>();
    if (relicManager != null)
    {
        relicManager.OnRelicAdded += DisplayRelics;  // 유물 추가 이벤트 구독
    }
     backMainButton.onClick.AddListener(ShowConfirmation);
        yesButton.onClick.AddListener(GoToMainScene);
        noButton.onClick.AddListener(HideConfirmation);
        
       
        mainImage.SetActive(false);
    }
public void DisplayRelics()
{
    // 기존의 유물 오브젝트를 모두 삭제.
    foreach (Transform child in relicParent)
    {
        Destroy(child.gameObject);
    }

    RelicManager relicManager = FindObjectOfType<RelicManager>();

    if (relicManager == null)
    {
        Debug.LogError("RelicManager not found in the scene.");
        return;
    }

    if (relicManager.playerRelics == null)
    {
        Debug.LogError("playerRelics is null in RelicManager.");
        return;
    }

    if (relicManager.playerRelics.Count == 0)
    {
        Debug.LogWarning("Player has no relics.");
        return;
    }

    // 플레이어가 보유한 각 유물에 대해 프리팹을 생성.
    foreach (var relic in relicManager.playerRelics)
    {
        GameObject relicObject = Instantiate(relicPrefab, relicParent);

        // 유물 스프라이트 설정 (Image 컴포넌트)
        Sprite relicSprite = Resources.Load<Sprite>($"Relic/relic{relic.number}");
        if (relicSprite == null)
        {
            Debug.LogError($"Sprite for relic{relic.number} not found in Resources/Relic.");
            continue;
        }

        // 유물 스프라이트 설정 (첫 번째 Image)
        Image relicImage = relicObject.GetComponent<Image>();
        if (relicImage != null)
        {
            relicImage.sprite = relicSprite; // 유물 스프라이트 설정
        }
        else
        {
            Debug.LogError("Image component for the relic sprite is missing.");
        }

        // 배경 스프라이트 설정 (두 번째 Image)
        Image backgroundImage = relicObject.transform.GetChild(0).GetComponent<Image>();
        if (backgroundImage == null)
        {
            Debug.LogError("Background Image component is missing in the relic prefab.");
        }

        // 유물 설명 설정
        TextMeshProUGUI descriptionText = backgroundImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = $"{relic.name} : {relic.description}";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component for relic description is missing.");
        }

        // RelicMouse 스크립트 추가 및 연결
        RelicMouse relicMouse = relicObject.GetComponent<RelicMouse>();
        if (relicMouse == null)
        {
            relicMouse = relicObject.AddComponent<RelicMouse>();
        }
        
        relicMouse.backgroundSprite = backgroundImage.gameObject;
        relicMouse.descriptionText = descriptionText;
    }
}


// public void DisplayRelics()
// {
//     // 기존의 유물 오브젝트를 모두 삭제.
//     foreach (Transform child in relicParent)
//     {
//         Destroy(child.gameObject);
//     }

//     RelicManager relicManager = FindObjectOfType<RelicManager>();

//     if (relicManager == null)
//     {
//         Debug.LogError("RelicManager not found in the scene.");
//         return;
//     }

//     if (relicManager.playerRelics == null)
//     {
//         Debug.LogError("playerRelics is null in RelicManager.");
//         return;
//     }

//     if (relicManager.playerRelics.Count == 0)
//     {
//         Debug.LogWarning("Player has no relics.");
//         return;
//     }

//     // 플레이어가 보유한 각 유물에 대해 프리팹을 생성.
//     foreach (var relic in relicManager.playerRelics)
//     {
//         GameObject relicObject = Instantiate(relicPrefab, relicParent);

//         // 유물 스프라이트 설정 (SpriteRenderer)
//         Sprite relicSprite = Resources.Load<Sprite>($"Relic/relic{relic.number}");
//         if (relicSprite == null)
//         {
//             Debug.LogError($"Sprite for relic{relic.number} not found in Resources/Relic.");
//             continue;
//         }

//         // 유물 스프라이트 설정 (첫 번째 SpriteRenderer)
//         SpriteRenderer relicSpriteRenderer = relicObject.GetComponent<SpriteRenderer>();
//         if (relicSpriteRenderer != null)
//         {
//             relicSpriteRenderer.sprite = relicSprite; // 유물 스프라이트 설정
//         }
//         else
//         {
//             Debug.LogError("SpriteRenderer component for the relic sprite is missing.");
//         }

//         // 배경 스프라이트 설정 (두 번째 SpriteRenderer)
//         SpriteRenderer backgroundSpriteRenderer = relicObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
//         if (backgroundSpriteRenderer == null)
//         {
//             Debug.LogError("Background SpriteRenderer component is missing in the relic prefab.");
//         }

//         // 유물 설명 설정
//         TextMeshProUGUI descriptionText = backgroundSpriteRenderer.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
//         if (descriptionText != null)
//         {
//             descriptionText.text = $"{relic.name} : {relic.description}";
//         }
//         else
//         {
//             Debug.LogError("TextMeshProUGUI component for relic description is missing.");
//         }

//         // RelicMouse 스크립트 추가 및 연결
//         RelicMouse relicMouse = relicObject.AddComponent<RelicMouse>();
//         relicMouse.backgroundSprite = backgroundSpriteRenderer.gameObject;
//         relicMouse.descriptionText = descriptionText;

//         // 배경 스프라이트와 설명 텍스트 비활성화 초기화
//         relicMouse.backgroundSprite.SetActive(false);
//         relicMouse.descriptionText.gameObject.SetActive(false);
//     }
// }



    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded 호출됨");
        // 곧바로 카메라 연결이 계속 실패하여, 씬이 로드된 후 약간 지연시간 후에 카메라 찾음
        UpdateUI();
        SetupItemBuffer();
        DeckSetup();
    }


    private void Update()
    {

    }
    public List<Item> GetItemBuffer()
    {
        return itemBuffer;
    }

    public List<Card> GetDeck()
    {
        return Deck;
    }
    // Next 버튼 기능
    void ShowKeywardBook()
    {
        if (KeywardText1.gameObject.activeSelf || KeywardText2.gameObject.activeSelf)
        {
            KeywardText1.gameObject.SetActive(false);
            KeywardText2.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(false);
            PreviousButton.gameObject.SetActive(false);
        }
        else
        {
            KeywardText1.gameObject.SetActive(true);
            KeywardText2.gameObject.SetActive(false);

            if (KeywardText1.gameObject.activeSelf)
            {
                NextButton.gameObject.SetActive(true);
            }
            PreviousButton.gameObject.SetActive(false);
        }
    }
    void ShowNextKeywardText()
    {
        if (KeywardText1.gameObject.activeSelf)
        {
            KeywardText1.gameObject.SetActive(false);
            KeywardText2.gameObject.SetActive(true);
            PreviousButton.gameObject.SetActive(true);
            NextButton.gameObject.SetActive(false);

        }
    }

    void ShowPreviousKeywardText()
    {
        if (KeywardText2.gameObject.activeSelf)
        {
            KeywardText2.gameObject.SetActive(false);
            KeywardText1.gameObject.SetActive(true);
            PreviousButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
        }
    }


    void MyCardsListButton()
    {
        if(invenClicked == false)
        {
            mycardsInven.SetActive(true);
            SetupItemBuffer();
            PopulateCardsInInventory();
            invenClicked = true;
        }
        else
        {
            mycardsInven.SetActive(false);
            invenClicked = false;
        }
       
    }
    void PopulateCardsInInventory()
    {
        // 패널 안에 기존 카드 제거
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        // ItemBuffer에 있는 아이템을 카드로 생성하여 패널에 추가
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            var cardObject = Instantiate(uicardPrefabs, scrollContent); // scrollContent를 부모로 설정
            var card = cardObject.GetComponent<UICard>(); // UICard 컴포넌트를 가져옴

            cardObject.transform.localScale = Vector3.one * 1.6f; // 카드 크기 조정
            card.Setup(itemBuffer[i]); // UICard의 Setup 메서드 호출하여 카드 세팅
        }
    }
    void BacktoMenu()
    {

        if (rewardMenuUI != null)
        {
            rewardMenuUI.SetActive(true);
        }
        BackToMenuUI.SetActive(false);
        EnlargeMyCards(false, Deck);
    }
    void ToggleImage(Image image)
    {
        image.gameObject.SetActive(!image.gameObject.activeSelf);
    }

    // void ToggleRelicImage()
    // {
    //     RelicImage.gameObject.SetActive(!RelicImage.gameObject.activeSelf);
    //     if (RelicImage.gameObject.activeSelf)
    //     {
    //         UpdateRelicImageText();
    //     }
    // }

    void UpdateRelicImageText()
    {
        RelicManager relicManager = FindObjectOfType<RelicManager>();

        // 유물 리스트에서 이름과 설명을 추출하여 문자열로 결합
        string relicDetails = string.Join("\n", relicManager.playerRelics.ConvertAll(relic => $"{relic.name}: {relic.description}"));

        RelicImageText.text = relicDetails;
    }



    void MyFunction()
    {
        // 지속적으로 UI 업데이트
        UpdateUI();
    }

    // 정보를 업데이트하는 메서드
    public void UpdateUI()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        // 유물 리스트에서 이름들을 추출하여 문자열로 결합
        //DisplayRelics();

        DiaText.text = $"{GameData.Instance.diamonds}";
        GoldText.text = $"{playerStats.gold}";
        switch (playerStats.character)
        {
            case 0:
                job = "전사";
                break;
            case 1:
                job = "도적";
                break;
            case 2:
                job = "마법사";
                break;
            default:
                job = "알 수 없음";
                break;
        }
        JobText.text = $"{job}";
        HealthText.text = $"{playerStats.currentHealth}/{playerStats.maxHealth}";
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
    // 아래는 카드 관련 함수
    // itemBuffer를 UI매니저에서 셋업하여, 이후 모든 씬에서 UI매니저에 생성되어 있는 itemBuffer를 사용한다.
    void SetupItemBuffer()
    {
        itemBuffer = new List<Item>();
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            itemBuffer.Add(item);
        }
    }

    void DeckSetup()
    {
        Deck.Clear();
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI);
            var card = cardObject.GetComponent<Card>();

            cardObject.transform.localScale = Vector3.one * 40f;

            card.Setup(itemBuffer[i]);
            Deck.Add(card);
        }
    }

    public void EnlargeMyCards(bool isEnlarge, List<Card> cards)
    {
        if (isEnlarge)
        {
            // 시작 위치 초기화
            Vector3 startPosition = new Vector3(5800f, 250f, 0f);
            // 카드 사이의 간격 설정
            float cardOffsetX = 190f;
            float cardOffsetY = 240f;
            int cardIndex = 0;
            // 모든 카드에 대해서 반복하여 처리

            for (int i = 0; i <= cards.Count / 7; i++)
            {

                for (int j = 0; j < 7; j++)
                {
                    if (cardIndex > (cards.Count - 1) || cards.Count == 0) { break; }
                    Vector3 enlargePos = startPosition + new Vector3(j * cardOffsetX, -i * cardOffsetY, 0f);
                    // 확대된 카드의 위치, 회전 및 크기 설정
                    // 아래를 통해, 덱 버튼을 클릭하여 팝업된 카드에 마우스를 올리면 사라지는 현상 없앤다. 카드의 현재 위치정보를 currentPRS에 새롭게 저장
                    cards[cardIndex].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 50f), false);
                    cards[cardIndex].currentPRS = new PRS(cards[cardIndex].transform.position,
                                                      cards[cardIndex].transform.rotation,
                                                      cards[cardIndex].transform.localScale);
                    cardIndex++;

                }
            }

        }
        else
        {
            // 모든 카드에 대해서 반복하여 처리
            foreach (var card in cards)
            {
                // 카드의 원래 위치와 크기로 되돌림
                card.MoveTransform(card.originPRS, false);

                card.currentPRS = new PRS(card.transform.position,
                                      card.transform.rotation,
                                      card.transform.localScale);
            }
        }
    }

    public void CardMouseOver(Card card)
    {

        if (card.IsFront())
        {
            selectCard = card;
            EnlargeCard(true, card);
            print("CardMouseOver");
        }

    }

    public void CardMouseExit(Card card)
    {
        if (card.IsFront())
        {
            EnlargeCard(false, card);
            print("CardMouseExit");
        }
    }

    public void EnlargeCard(bool isEnlarge, Card card)
    {

        if (isEnlarge)
        {
            // 현재 카드의 위치(currentPRS)를 기준으로 확대 위치 계산
            Vector3 enlargePos = new Vector3(card.currentPRS.pos.x, card.currentPRS.pos.y + 25f, -5f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 65f), false);
        }
        else
        {
            // 현재 위치로 돌아가도록 설정
            card.MoveTransform(card.currentPRS, false);
        }
    }
 void ShowConfirmation()
    {
        // 확인 이미지 활성화
        mainImage.SetActive(true);
    }

    void GoToMainScene()
    {
        
        DestroyAllDontDestroyOnLoadObjects();
        SceneManager.LoadScene("GameStart"); // "MainScene"은 실제 씬 이름으로 변경
    }

    void HideConfirmation()
    {
        // 확인 이미지 비활성화
        mainImage.SetActive(false);
    }



}
