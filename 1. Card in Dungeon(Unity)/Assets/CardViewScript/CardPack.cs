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
                selectCardDelete.selectedCard = false; // ������ ���õ� ī���� ���ÿ��� ���� 
            }
            selectCardDelete = card; // ���ο� ī��� ���� ������Ʈ
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
            // ���� ī���� ��ġ(currentPRS)�� �������� Ȯ�� ��ġ ���
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
            // ���� ��ġ�� ���ư����� ����
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
            // ���� ��ġ �ʱ�ȭ
            Vector3 startPosition = new Vector3(890f, 830f, 0f);
            // ī�� ������ ���� ����
            float cardOffsetX = 190f;
            float cardOffsetY = 300f;
            int cardIndex = 0;
            // ��� ī�忡 ���ؼ� �ݺ��Ͽ� ó��

            for (int i = 0; i <= cards.Count / 7; i++)
            {

                for (int j = 0; j < 7; j++)
                {
                    if (cardIndex > (cards.Count - 1) || cards.Count == 0) { break; }
                    Vector3 enlargePos = startPosition + new Vector3(j * cardOffsetX, -i * cardOffsetY, 0f);
                    // Ȯ��� ī���� ��ġ, ȸ�� �� ũ�� ����
                    // �Ʒ��� ����, �� ��ư�� Ŭ���Ͽ� �˾��� ī�忡 ���콺�� �ø��� ������� ���� ���ش�. ī���� ���� ��ġ������ currentPRS�� ���Ӱ� ����
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
            // ��� ī�忡 ���ؼ� �ݺ��Ͽ� ó��
            foreach (var card in cards)
            {
                // ī���� ���� ��ġ�� ũ��� �ǵ���
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

    //�Ʒ��� ���õ� ī�� ���� ���� �Լ���

    void DeleteSelectedCard()
    {
        if (selectCardDelete != null)
        {
            // ���õ� ī�带 ���� ������ ����
            Destroy(selectCardDelete.gameObject);
            Destroy(currentCardObject);

            // Deck ����Ʈ���� �ش� ī�带 ����
            Deck.Remove(selectCardDelete);
            Debug.Log($"Card removed: {selectCardDelete.name}");
            UIManager.Instance.itemSO.RemoveItem(selectCardDelete.item);

            // ���õ� ī�� �ʱ�ȭ
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

