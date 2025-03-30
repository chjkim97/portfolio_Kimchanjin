using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleReward : MonoBehaviour
{

    [SerializeField] GameObject cardPrefabs;
    public List<Card> battleRewardCard;
    public TextMeshProUGUI relicText;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button skipbutton;
    public Button cardRefreshbutton;
    public Button relicRefreshbutton;
    public TextMeshProUGUI cardRefreshButtonText;
    public TextMeshProUGUI relicRefreshButtonText;
    private RelicMouse relicMouse1;

    public GameObject relicObject1;

    private CardList cardList;
    private PlayerStats playerStats;
    private RelicManager relicManager;
    public int cardrefreshgold = 0;
    public int relicrefreshgold = 0;
    int relicNumber = 0;
    bool firstcardrefresh = false;
    void Start()
    {
        battleRewardCard = new List<Card>();
        cardRefreshButtonText = cardRefreshbutton.GetComponentInChildren<TextMeshProUGUI>();
        relicRefreshButtonText = relicRefreshbutton.GetComponentInChildren<TextMeshProUGUI>();
        relicManager = FindObjectOfType<RelicManager>();
        cardList = FindObjectOfType<CardList>();
        playerStats = FindObjectOfType<PlayerStats>();

        if (relicManager.playerRelics.Exists(relic => relic.number == 36))
            firstcardrefresh = true;
        relicMouse1 = relicObject1.GetComponent<RelicMouse>();

        cardrefresh();
        relicrefresh();

        cardRefreshbutton.onClick.AddListener(cardrefresh);
        relicRefreshbutton.onClick.AddListener(relicrefresh);
    }

    void relicrefresh()
    {
        if (playerStats.gold >= relicrefreshgold)
        {
            playerStats.gold -= relicrefreshgold;

            if (playerStats.enemytype == 0)
            {
                relicObject1.SetActive(false);
                relicRefreshbutton.gameObject.SetActive(false);
            }
            else if (playerStats.enemytype > 0)
            {
                relicObject1.SetActive(true);
                relicRefreshbutton.gameObject.SetActive(true);
                relicNumber = relicManager.ChoiceRanRelic(); // 무작위 유물 선택

                // 유물 정보를 가져와서 UI 업데이트
                foreach (Relic relic in relicManager.allRelics)
                {
                    if (relic.number == relicNumber)
                    {
                        relicText.text = $"{relic.name} : {relic.description}";

                        // 스프라이트 경로 설정
                        string spritePath = $"Relic/relic{relicNumber}";
                        Sprite relicSprite = Resources.Load<Sprite>(spritePath);
                        if (relicSprite != null)
                        {
                             Image imageComponent = relicObject1.GetComponent<Image>();
                            if (imageComponent != null)
                            {
                                imageComponent.sprite = relicSprite;
                            }
                            else
                            {
                                Debug.LogWarning("스프라이트 랜더러를 찾을 수 없습니다");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"스프라이트를 찾을 수 없습니다: {spritePath}");
                        }
                        break; // 유물 찾으면 반복 종료
                    }
                }

                if (relicManager.playerRelics.Exists(relic => relic.number == 36))
                {
                    relicrefreshgold = 100;
                }
                else
                    relicrefreshgold += 100; //비용 100 증가

                relicRefreshButtonText.text = $"유물 새로고침 비용\n: {relicrefreshgold}";
            }
        }
    }
    void cardrefresh()
    {
        if (playerStats.gold >= cardrefreshgold)
        {
            foreach (var card in battleRewardCard)
            {
                Destroy(card.gameObject);
            }
        }

        battleRewardCard.Clear();
        cardList.InitializeCardList();

        //직업카드+공용카드
        int character = playerStats.character;
        int[] numbers;
        if (playerStats.gold >= cardrefreshgold) //새로고침 비용이 있다면 비용을 지불하고 새로고침
        {
            playerStats.gold -= cardrefreshgold;

            if (character == 0)
            {
                numbers = new int[] { 6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,21,22,
                101, 102, 103, 104, 105, 106, 107, 108, 109, 110 };
            }
            else if (character == 1)
            {
                numbers = new int[] { 6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,22,
                201, 202, 203, 204, 205, 206, 207, 209, 210,
                212, 213, 214, 215, 216, 217, 218, 219, 220};
            }
            else
            {
                numbers = new int[] {  6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,22,
                301, 302, 303, 304, 305, 306, 307, 308, 310,
                311, 313, 314, 315, 316, 317, 318, 319, 320 };
            }

            // 리스트를 만들어 배열의 숫자를 복사합니다.
            List<int> remainingNumbers = new List<int>(numbers);

            // 중복되지 않는 무작위 숫자 3개를 선택합니다.
            List<int> selectedNumbers = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, remainingNumbers.Count);
                selectedNumbers.Add(remainingNumbers[index]);
                remainingNumbers.RemoveAt(index);
            }

            // 선택된 숫자를 각각의 변수에 대입합니다.
            int cardnumber1 = selectedNumbers[0];
            int cardnumber2 = selectedNumbers[1];
            int cardnumber3 = selectedNumbers[2];

            for (int i = 0; i < selectedNumbers.Count; i++)
            {
                var cardObject = Instantiate(cardPrefabs);
                var card = cardObject.GetComponent<Card>();

                cardObject.transform.localScale = Vector3.one * 40f;
                card.Setup(cardList.MakeCardToItem(selectedNumbers[i]));
                battleRewardCard.Add(card);
            }

            ShowCardList();
            button1.onClick.RemoveAllListeners();
            button2.onClick.RemoveAllListeners();
            button3.onClick.RemoveAllListeners();
            skipbutton.onClick.RemoveAllListeners();
            button1.onClick.AddListener(() => ButtonClicked(cardnumber1));
            button2.onClick.AddListener(() => ButtonClicked(cardnumber2));
            button3.onClick.AddListener(() => ButtonClicked(cardnumber3));
            skipbutton.onClick.AddListener(() => skipButtonClicked());
            if (relicManager.playerRelics.Exists(relic => relic.number == 35))
            {
                cardrefreshgold = 50;
            }
            else
                cardrefreshgold += 50; //비용 50 증가
            if (firstcardrefresh)
            {
                cardrefreshgold = 0;
                firstcardrefresh = false;
            }
            cardRefreshButtonText.text = $"카드 새로고침 비용 : {cardrefreshgold}";
        }
    }

    void ButtonClicked(int cardnumber)
    {
        cardList.AddCardToPlayer(cardnumber);
        relicManager.AddRelicToPlayer(relicNumber);
        // switch (playerStats.Rand_Map_Gene)
        // {
        //     case 1:
        //         SceneManager.LoadScene("Map0");
        //         break;
        //     case 2:
        //         SceneManager.LoadScene("Map1");
        //         break;
        //     case 3:
        //         SceneManager.LoadScene("Map2");
        //         break;
        //     case 4:
        //         SceneManager.LoadScene("Map3");
        //         break;
        //     case 5:
        //         SceneManager.LoadScene("Map4");
        //         break;
        //     default:
        //         Debug.LogError("Invalid Rand_Map_Gene value: " + playerStats.Rand_Map_Gene);
        //         break;
        // }
        SceneManager.LoadScene("TestRandomMap");


    }
    void skipButtonClicked()
    {
        SceneManager.LoadScene("TestRandomMap");

    }
    void DisplayCardInfoByNumber(int cardNumber1, int cardNumber2, int cardNumber3)
    {
        /*
        // 전사 카드 정보 가져오기
        CardList.CardInfo selectedCard1 = cardList.GetCardByNumber(cardNumber1);
        nameTexts[0].text = "이름: " + selectedCard1.name;
        costTexts[0].text = "비용: " + selectedCard1.cost.ToString();
        typeTexts[0].text = "타입: " + selectedCard1.type;
        descriptionTexts[0].text = selectedCard1.description;

        // 도적 카드 정보 가져오기
        CardList.CardInfo selectedCard2 = cardList.GetCardByNumber(cardNumber2);
        nameTexts[1].text = "이름: " + selectedCard2.name;
        costTexts[1].text = "비용: " + selectedCard2.cost.ToString();
        typeTexts[1].text = "타입: " + selectedCard2.type;
        descriptionTexts[1].text = selectedCard2.description;

        // 법사 카드 정보 가져오기
        CardList.CardInfo selectedCard3 = cardList.GetCardByNumber(cardNumber3);
        nameTexts[2].text = "이름: " + selectedCard3.name;
        costTexts[2].text = "비용: " + selectedCard3.cost.ToString();
        typeTexts[2].text = "타입: " + selectedCard3.type;
        descriptionTexts[2].text = selectedCard3.description;
        */
    }

    void ShowCardList()
    {
        Vector3 startPosition = new Vector3(350f, 550f, 0f);
        // 카드 사이의 간격 설정
        float cardOffsetX = 600f;
        float cardOffsetY = 400f;
        int cardIndex = 0;
        // 모든 카드에 대해서 반복하여 처리

        for (int i = 0; i <= battleRewardCard.Count / 3; i++)
        {

            for (int j = 0; j < 3; j++)
            {
                if (cardIndex > (battleRewardCard.Count - 1) || battleRewardCard.Count == 0) { break; }
                Vector3 enlargePos = startPosition + new Vector3(j * cardOffsetX, -i * cardOffsetY, 0f);
                // 확대된 카드의 위치, 회전 및 크기 설정
                // 아래를 통해, 덱 버튼을 클릭하여 팝업된 카드에 마우스를 올리면 사라지는 현상 없앤다. 카드의 현재 위치정보를 currentPRS에 새롭게 저장
                battleRewardCard[cardIndex].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 80f), false);
                battleRewardCard[cardIndex].currentPRS = new PRS(battleRewardCard[cardIndex].transform.position,
                                                  battleRewardCard[cardIndex].transform.rotation,
                                                  battleRewardCard[cardIndex].transform.localScale);
                cardIndex++;

            }
        }
    }
}
