using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;
using JetBrains.Annotations;

public class Shop : MonoBehaviour
{
    public GameObject relicObject1;
    public GameObject relicObject2;
    public GameObject relicObject3;
     private RelicMouse relicMouse1;
    private RelicMouse relicMouse2;
    private RelicMouse relicMouse3;
    public List<Card> shopCard;
    [SerializeField] GameObject cardPrefabs;
    [SerializeField] GameObject uicardPrefabs;

    public TextMeshProUGUI[] relicDescriptionTexts; // 총 3개의 유물 설명 텍스트
    public Dictionary<int, int> relicNumberToIndexMap = new Dictionary<int, int>(); // 유물 번호와 인덱스를 매핑하는 딕셔너리
    public Dictionary<int, int> cardNumberToIndexMap = new Dictionary<int, int>(); // 카드 번호와 인덱스를 매핑하는 딕셔너리
    
    public Button[] cardButtons; // 총 6개의 카드 버튼
    
    public Button[] relicButtons; // 총 3개의 유물 버튼
    public Button backButton;
   

    private CardList cardList;
    private PlayerStats playerStats;
    private RelicManager relicManager;
    private CardPack cardPack;

    public int cardrefreshgold = 0;
    public int relicrefreshgold = 0;
    public int relicNumber = 0;

    private GameObject currentCardObject;
    Card selectCard;
    public Canvas canvas;
    public SpriteRenderer[] relicSpriteRenderers;

    void Start()
    {

        shopCard = new List<Card>();
        relicManager = FindObjectOfType<RelicManager>();
        cardList = FindObjectOfType<CardList>();
        playerStats = FindObjectOfType<PlayerStats>();
        cardPack = FindObjectOfType<CardPack>();
        backButton.onClick.AddListener(() => BackButtonClicked());
        relicMouse1 = relicObject1.GetComponent<RelicMouse>();
        relicMouse2 = relicObject2.GetComponent<RelicMouse>();
        relicMouse3 = relicObject3.GetComponent<RelicMouse>();
        cardrefresh();
        foreach (var renderer in relicSpriteRenderers)
        {
            renderer.gameObject.SetActive(false);
        }
        relicrefresh();
       
    }

     
     void relicrefresh()
{
    List<int> selectedRelics = relicManager.GetRandomRelicNumbers(relicButtons.Length);
    
    for (int i = 0; i < relicButtons.Length; i++)
    {
        relicNumber = selectedRelics[i];
        Relic relic = relicManager.allRelics.Find(r => r.number == relicNumber);

        relicDescriptionTexts[i].text = $"{relic.name} : {relic.description}";

        // 유물 스프라이트 설정
        string spritePath = $"Relic/relic{relicNumber}"; // 스프라이트 경로 설정
        Sprite relicSprite = Resources.Load<Sprite>(spritePath);
        
        if (relicSprite != null)
        {
            // 각 유물 오브젝트에 대해 Image 컴포넌트에 스프라이트 적용
            GameObject currentRelicObject = (i == 0) ? relicObject1 :
                                             (i == 1) ? relicObject2 : relicObject3;

            // Image 컴포넌트를 가져옵니다.
            Image imageComponent = currentRelicObject.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = relicSprite; // 스프라이트 설정
            }
            else
            {
                Debug.LogWarning($"Image 컴포넌트를 찾을 수 없습니다: {currentRelicObject.name}");
            }

            // RelicMouse의 descriptionText를 설정
            RelicMouse relicMouse = currentRelicObject.GetComponent<RelicMouse>();
            if (relicMouse != null)
            {
                relicMouse.descriptionText.text = $"{relic.name} : {relic.description}"; // 설명 텍스트 설정
            }
        }
        else
        {
            Debug.LogWarning($"스프라이트를 찾을 수 없습니다: {spritePath}");
        }
        
        relicNumberToIndexMap[relicNumber] = i; // 유물 번호와 인덱스를 매핑

        // 유물 번호를 로컬 변수로 캡처
        int tempRelicNumber = relicNumber;
        relicButtons[i].onClick.RemoveAllListeners();
        relicButtons[i].onClick.AddListener(() => relicButtonClicked(tempRelicNumber)); // 유물 번호를 사용
    }
}


    int relicbuttonindex(int clickedRelicNumber)
    {
        int clickedRelicIndex;
        if (relicNumberToIndexMap.TryGetValue(clickedRelicNumber, out clickedRelicIndex))
        {
            return clickedRelicIndex;  // 유물 번호에 해당하는 인덱스를 가져옴
        }
        return -1;  // 유효하지 않은 경우 -1 반환
    }

    int cardbuttonindex(int clickedcardNumber)
    {
        int clickedCardIndex;
        if (cardNumberToIndexMap.TryGetValue(clickedcardNumber, out clickedCardIndex))
        {
            return clickedCardIndex;  // 카드 번호에 해당하는 인덱스를 가져옴
        }
        return -1;  // 유효하지 않은 경우 -1 반환
    }

    void cardrefresh()
    {
        cardList.InitializeCardList();
        
        int character = playerStats.character;
        int[] numbers;

        if (character == 0)
        {
            numbers = new int[] { 3, 4, 5, 6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                    101, 102, 103, 104, 105, 106, 107, 108, 109, 110 };
        }
        else if (character == 1)
        {
            numbers = new int[] { 3, 4, 5, 6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                    201, 202, 203, 204, 205, 206, 207, 208, 209, 210,
                    211, 212, 213, 214, 215, 216, 217, 218, 219, 220 };
        }
        else
        {
            numbers = new int[] { 3, 4, 5, 6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                    301, 302, 303, 304, 305, 306, 307, 308, 309, 310,
                    311, 312, 313, 314, 315, 316, 317, 318, 319, 320 };
        }
        
        List<int> remainingNumbers = new List<int>(numbers);
        List<int> selectedNumbers = new List<int>();
        
        for (int i = 0; i < 6; i++)
        {
            int index = Random.Range(0, remainingNumbers.Count);
            selectedNumbers.Add(remainingNumbers[index]);
            remainingNumbers.RemoveAt(index);
        }
        
        /*
        selectedNumbers.Add(301);
        selectedNumbers.Add(302);
        selectedNumbers.Add(303);
        selectedNumbers.Add(307);
        selectedNumbers.Add(218);
        selectedNumbers.Add(309);
        */
        for (int i = 0; i < selectedNumbers.Count; i++)
        {
            var cardObject = Instantiate(cardPrefabs);
            var card = cardObject.GetComponent<Card>();

            cardObject.transform.localScale = Vector3.one * 40f;
            card.Setup(cardList.MakeCardToItem(selectedNumbers[i]));
            shopCard.Add(card);
        }

        ShowCardList();

        for (int i = 0; i < cardButtons.Length; i++)
        {
            int cardNumber = selectedNumbers[i];
            cardNumberToIndexMap[cardNumber] = i;
            cardButtons[i].onClick.RemoveAllListeners();
            int tempCardNumber = cardNumber; // 현재 카드 번호를 로컬 변수에 저장
            cardButtons[i].onClick.AddListener(() => ButtonClicked(tempCardNumber));
        }

    }



    void ShowCardList()
    {

        Vector3 startPosition = new Vector3(230f, 700f, 0f);
        // 카드 사이의 간격 설정
        float cardOffsetX = 300f;
        float cardOffsetY = 400f;
        int cardIndex = 0;
        // 모든 카드에 대해서 반복하여 처리

        for (int i = 0; i <= shopCard.Count / 3; i++)
        {

            for (int j = 0; j < 3; j++)
            {
                if (cardIndex > (shopCard.Count - 1) || shopCard.Count == 0) { break; }
                Vector3 enlargePos = startPosition + new Vector3(j * cardOffsetX, -i * cardOffsetY, 0f);
                // 확대된 카드의 위치, 회전 및 크기 설정
                // 아래를 통해, 덱 버튼을 클릭하여 팝업된 카드에 마우스를 올리면 사라지는 현상 없앤다. 카드의 현재 위치정보를 currentPRS에 새롭게 저장
                shopCard[cardIndex].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 50f), false);
                shopCard[cardIndex].currentPRS = new PRS(shopCard[cardIndex].transform.position,
                                                  shopCard[cardIndex].transform.rotation,
                                                  shopCard[cardIndex].transform.localScale);
                cardIndex++;

            }
        }
    }

    void relicButtonClicked(int relicnum)
    {
        if (playerStats.gold >= 200)
        {
            playerStats.gold -= 200;
            relicManager.AddRelicToPlayer(relicnum);

            int index = relicbuttonindex(relicnum);
            if (index != -1)
            {
                relicButtons[index].interactable = false; // 버튼을 비활성화
            }
            if (index >= 0 && index < relicSpriteRenderers.Length)
            {
                relicSpriteRenderers[index].gameObject.SetActive(true);
            }
        }
    }

    void ButtonClicked(int cardnumber)
    {
        Debug.LogWarning("버튼진입");
        if (playerStats.gold >= 100)
        {
            playerStats.gold -= 100;
            cardList.AddCardToPlayer(cardnumber);
            GameData.Instance.BuyCard();

            Debug.LogWarning($"shopCard 리스트에 포함된 카드 개수: {shopCard.Count}");
            foreach (var card in shopCard)
            {
                Debug.LogWarning("진입");
                if (card.cardNumber == cardnumber)
                {                 
                    Transform shaderTransform = card.transform.Find("Shader");
                    Transform soldOut = card.transform.Find("SoldOut");

                    if (shaderTransform != null && soldOut != null)
                    {
                        shaderTransform.gameObject.SetActive(true);
                        soldOut.gameObject.SetActive(true);

                    }
                    else
                    {
                        Debug.LogWarning("Shader없음");
                    }
                }
            }

            int index = cardbuttonindex(cardnumber);
            if (index != -1)
            {
                Debug.LogWarning("버튼비활성");
                cardButtons[index].interactable = false; // 버튼을 비활성화
            }
        }
    }

    void BackButtonClicked()
    {
        foreach (var card in shopCard)
        {
            Transform shaderTransform = card.transform.Find("Shader");
            Transform soldOut = card.transform.Find("SoldOut");
            shaderTransform.gameObject.SetActive(false);
            soldOut.gameObject.SetActive(false);
        }
                   SceneManager.LoadScene("TestRandomMap");

    }
    public void UICardShow()
    {

        if (currentCardObject != null)
        {
            Destroy(currentCardObject);
        }

        currentCardObject = Instantiate(uicardPrefabs, new Vector3(1220f, -600f, 0f), Utils.QI);

        currentCardObject.transform.SetParent(canvas.transform, false);

        var card = currentCardObject.GetComponent<UICard>();
        currentCardObject.transform.localScale = Vector3.one * 2.5f;
        card.Setup(cardList.MakeCardToItem(selectCard.cardNumber));
    }

    public void CardMouseOver(Card card)
    {
        if (card.IsFront())
        {
            selectCard = card;
            UICardShow();
        }
    }
    
       public void CardMouseExit(Card card)
    {
        if (card.IsFront())
        {
            if (currentCardObject != null)
            {
                Destroy(currentCardObject);
                currentCardObject = null;
            }
        }
    }

}
