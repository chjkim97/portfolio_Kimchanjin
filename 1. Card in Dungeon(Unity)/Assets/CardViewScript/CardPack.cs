using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardPack : MonoBehaviour
{
    public static CardPack instance { get; private set; }
    private void Awake() => instance = this;
    //[SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefabs;
    public List<Card> PlayerCards;
    //[SerializeField] List<Card> Deck;
    
    public List<Card> Deck;

    // List<Item> itemBuffer = UIManager.Instance.GetItemBuffer();
    Card selectCard;
    Card selectCardDelete;

    public Button MyCardView;
    public Button DeleteButton;
    bool MyCardViewClick = true;

    [SerializeField] GameObject uicardPrefabs;
    private GameObject currentCardObject;
    public Canvas canvas;
    private CardList cardList;


    public void CardMouseOver(Card card)
    {

        if (card.IsFront() && !card.selectedCard)
        {
            selectCard = card;
            EnlargeCard(true, card);
        }

    }

    public void CardMouseExit(Card card)
    {
        if (card.IsFront() && !card.selectedCard)
        {
            EnlargeCard(false, card);
        }
    }

    public void CardMouseDown(Card card)
    {
        if (selectCardDelete == null || card != selectCardDelete)
        {
            if (selectCardDelete != null)
            {
                EnlargeCard(false, selectCardDelete);
                selectCardDelete.selectedCard = false; // 이전에 선택된 카드의 선택여부 해제 
            }
            selectCardDelete = card; // 새로운 카드로 선택 업데이트
        }
        card.selectedCard = true;     
        Debug.Log($"Card selected: {card.name}");
        PopUpCard(card);
        DeleteButton.gameObject.SetActive(true);
    }


    public void EnlargeCard(bool isEnlarge, Card card) 
    {
       
        if (isEnlarge)
        {
            // 현재 카드의 위치(currentPRS)를 기준으로 확대 위치 계산
            Vector3 enlargePos = new Vector3(card.currentPRS.pos.x, card.currentPRS.pos.y + 25f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 60f), false);
            card.PlayCardEffect();
            
            var mainModule = card.cardEffectParticle.main;
            mainModule.startSize = 1.7f;
            if (card.cardEffectParticle2 != null)
            {
                var mainModule2 = card.cardEffectParticle2.main;
                mainModule2.startSize = 1.7f;
            }
            if (card.cardEffectParticle3 != null)
            {
                var mainModule3 = card.cardEffectParticle3.main;
                mainModule3.startSize = 1.7f;
            }
            if (card.cardEffectParticle4 != null)
            {
                var mainModule4 = card.cardEffectParticle4.main;
                mainModule4.startSize = 1.7f;
            }

        }
        else
        {
            // 현재 위치로 돌아가도록 설정
            card.MoveTransform(card.currentPRS, false);
            
            var mainModule = card.cardEffectParticle.main;
            mainModule.startSize = 1.0f;
            if (card.cardEffectParticle2 != null)
            {
                var mainModule2 = card.cardEffectParticle2.main;
                mainModule2.startSize = 1.0f;
            }
            if (card.cardEffectParticle3 != null)
            {
                var mainModule3 = card.cardEffectParticle3.main;
                mainModule3.startSize = 1.0f;
            }
            if (card.cardEffectParticle4 != null)
            {
                var mainModule4 = card.cardEffectParticle4.main;
                mainModule4.startSize = 1.0f;
            }
        }
    }

    public void PopUpCard(Card card)
    {
        if (currentCardObject != null)
        {
            Destroy(currentCardObject);
        }

        currentCardObject = Instantiate(uicardPrefabs, new Vector3(410f, -610f, 0f), Utils.QI);

        currentCardObject.transform.SetParent(canvas.transform, false);

        var Uicard = currentCardObject.GetComponent<UICard>();
        currentCardObject.transform.localScale = Vector3.one * 2.5f;
        Uicard.Setup(cardList.MakeCardToItem(selectCard.cardNumber));
    }


    public void EnlargeMyCardslist(bool isEnlarge, List<Card> cards)
    {
        if (isEnlarge)
        {
            // 시작 위치 초기화
            Vector3 startPosition = new Vector3(890f, 830f, 0f);
            // 카드 사이의 간격 설정
            float cardOffsetX = 190f;
            float cardOffsetY = 300f;
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
                    cards[cardIndex].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 40f), false);
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

    public void DeckViewButtonClick()
    {
        Debug.Log("DeckViewClick");
        if (MyCardViewClick == true)
        {
            EnlargeMyCardslist(MyCardViewClick, Deck);
            MyCardViewClick = false;
        }
        else if (MyCardViewClick == false)
        {
            EnlargeMyCardslist(MyCardViewClick, Deck);
            MyCardViewClick = true;
            DeleteButton.gameObject.SetActive(false);
        }

    }

    //아래는 선택된 카드 삭제 관련 함수들

    void DeleteSelectedCard()
    {
        if (selectCardDelete != null)
        {
            // 선택된 카드를 게임 씬에서 제거
            Destroy(selectCardDelete.gameObject);
            Destroy(currentCardObject);

            // Deck 리스트에서 해당 카드를 제거
            Deck.Remove(selectCardDelete);
            Debug.Log($"Card removed: {selectCardDelete.name}");
            UIManager.Instance.itemSO.RemoveItem(selectCardDelete.item);

            // 선택된 카드 초기화
            selectCardDelete = null;
        }
        DeleteButton.gameObject.SetActive(false);
                    SceneManager.LoadScene("TestRandomMap");

    }

    // Start is called before the first frame update
    void Start()
    {
        // SetupItemBuffer();
        // DeckSetup();
        Deck = UIManager.Instance.GetDeck();
        MyCardView.onClick.AddListener(DeckViewButtonClick);
        DeleteButton.onClick.AddListener(DeleteSelectedCard);
        cardList = FindObjectOfType<CardList>();
    }

        // Update is called once per frame
    void Update()
    {

    }
}

