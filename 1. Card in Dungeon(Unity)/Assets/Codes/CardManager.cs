using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.UIElements;
using UnityEngine.Video;
//using static UnityEditor.Progress;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class CardManager : MonoBehaviour
{
    public static CardManager instance { get; private set; }
    private void Awake() => instance = this;
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefabs;
    [SerializeField] GameObject uicardPrefabs;
    public GameObject panel;
    public RectTransform scrollContent;

    [SerializeField] List<Card> UseCards;
    public List<Card> Deck;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform useSpawnPoint;
    [SerializeField] Transform deckSpawnPoint;
    [SerializeField] Transform cardUseEffectPoint;
    [SerializeField] Transform cardUseEffectPoint2;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Sprite backCardSprite;
    [SerializeField] List<Card> AppliedEnemy_one;
    [SerializeField] List<Card> AppliedEnemy_two;
    public List<Card> PlayerCards;
    public List<Card> UseCardTpye_Cont;
    public List<Card> UseCardTpye_Once;
    public Button DeckView;
    public Button UseCardView;
    public int enemynumber = 0;
    List<Item> itemBuffer;
    Card selectCard;
    bool isMyCardDrag;
    bool onMyCardArea;
    bool EnemyArea_one;
    bool EnemyArea_two;
    bool DeckViewClick = true;
    bool UseCardViewClick = true;
    bool isDeckEnlarged;
    int myPutCount;
    int startcount = 0;

    public Card lastUsedCard;
    public int usecardcounter = 1; //카드를 실제로 사용할 때 1, 카드를 버릴 때 0
    public int throwcard = 0; // 이 수치만큼 카드를 버림

    private bool canInteract = true;
    bool deckClicked = false;
    private SoundManager soundManager;

    public CardList cardList;

    private GameObject currentCardObject;

    public Canvas canvas;


    void FillDeck()
    {
        if (Deck.Count == 0)
        {
            Deck.Clear();
            if (startcount == 0)
            {
                for (int i = 0; i < itemBuffer.Count; i++)
                {
                    var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI);
                    var card = cardObject.GetComponent<Card>();

                    cardObject.transform.localScale = Vector3.one * 40f;

                    card.Setup(itemBuffer[i]);
                    Deck.Add(card);
                    startcount++;
                }
            }

            else if (startcount != 0)
            {
                foreach (var usedCard in UseCards)
                {
                    Deck.Add(usedCard);
                }
                UseCards.Clear();


                for (int i = 0; i < Deck.Count; i++)
                {
                    int rand = Random.Range(i, Deck.Count);
                    Card temp = Deck[i];
                    Deck[i] = Deck[rand];
                    Deck[rand] = temp;
                }
            }
        }
    }

    public Item UseItem()
    {
        if (PlayerCards.Count > 0)
        {
            Card card = PlayerCards[0];
            PlayerCards.RemoveAt(0);
            DestroyImmediate(card.gameObject);
            return card.GetItem();
        }
        else
        {
            return null;
        }
    }


    void SetupItemBuffer()
    {
        itemBuffer = new List<Item>();
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            itemBuffer.Add(item);
        }
    }

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        cardList = FindObjectOfType<CardList>();
        SetupItemBuffer();
        FillDeck();
        DeckList();

        print("start : 5 card draw");
        AddCard(5);
        

        DeckView.onClick.AddListener(DeckViewButtonClick);
        UseCardView.onClick.AddListener(UseCardViewButtonClick);

    }

    void Update()
    {

        //relic33효과
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        if (relicManager.playerRelics.Exists(relic => relic.number == 33) && PlayerCards.Count == 0)
        {
            AddCard(1);
        }

        if (isMyCardDrag)
        {
            CardDrag();
        }

        // DetectCardArea();
        UseCardArea();

        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            print("DrawCard");
            AddCard(1);
        }

        else if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            usecardcounter = 2;
            Item usedItem = UseItem();
            if (usedItem != null)
            {
                print("Used item: " + usedItem.name);
                UseCard(usedItem);
            }
            else
            {
                print("No card to use.");
            }
            usecardcounter = 1;
        }
        else if (throwcard > 0) //throwcard의 수치만큼 카드 버림
        {
            usecardcounter = 2;

            Item usedItem = UseItem();
            if (usedItem != null)
            {
                print("Used item: " + usedItem.name);
                UseCard(usedItem);
            }
            else
            {
                print("No card to use.");
            }
            usecardcounter = 1;
            throwcard--;
        }
        // CardType_ContOrOnce();
    }


    public void AddCard(int drawnum)
    {
        for (int i = 0; i < drawnum; i++)
        {
            if (PlayerCards.Count < 10)
            {
                var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI);
                var card = cardObject.GetComponent<Card>();

                cardObject.transform.localScale = Vector3.one * 40f;

                card.Setup(PopCardFromDeck());
                PlayerCards.Add(card);
                SetOriginOrder();
                CardAlignment();

            }
        }
        if (soundManager != null)
        {
            soundManager.PlaySound(0);
        }
    }

    Item PopCardFromDeck()
    {
        if (Deck.Count == 0)
        {
            FillDeck();
        }

        Card card = Deck[0];
        Item item = card.GetItem();
        Deck.RemoveAt(0);
        return item;
    }

    public void UseCard(Item item)
    {

        // UseSpawnPoint에 카드 생성
        var cardObject = Instantiate(cardPrefabs, useSpawnPoint.position, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        //card.SetBackSprite(backCardSprite);
        card.Setup(item);
        // UseCards 리스트에 카드 추가


        UseCards.Add(card);

        CardAlignment();
        // usecardcounter가 1일 때만 카드 효과 발동
        if (usecardcounter == 1)
        {
            // 현재 scene에서 EnemyStats 클래스의 인스턴스를 찾기
            CardEffect cardEffect = FindObjectOfType<CardEffect>();
            EnemyStats[] enemyStatsArray = FindObjectsOfType<EnemyStats>();
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            TurnManager turnManager = FindObjectOfType<TurnManager>();
            turnManager.NumberOfCard += 1;

            playerStats.shield += 4 * playerStats.shieldcounter; //카드를 사용할 때마다 내가 사용한 '방어 태세' 1장 당 실드 +4 
            playerStats.shield += playerStats.cardarmor; //카드를 사용할 때마다 내가 사용한 '천 덧대기' 만큼 실드 획득
            RelicManager relicManager = FindObjectOfType<RelicManager>();

            //relic29가 있을 때 일회용, 지속 카드를 내면 카드를 1장 뽑음
            if (relicManager.playerRelics.Exists(relic => relic.number == 29) && card.TypeTMP.text == "Once")
            {
                AddCard(1);
            }

            lastUsedCard = UseCards[UseCards.Count - 1];
            if (enemyStatsArray.Length > 0)
            {
                // 적이 여러 명인 경우, 모든 적에게 효과를 적용하도록 수정 가능
                EnemyStats firstEnemy = enemyStatsArray[0];
                switch (item.usetype)
                {
                    case 1:
                        // 플레이어에게 효과를 적용
                        cardEffect.ApplyPlayerEffect();
                        break;
                    case 2:
                        cardEffect.ApplySingleEnemyEffect(enemynumber);
                        break;
                    case 3:
                        // 모든 적에게 효과를 적용
                        cardEffect.ApplyAllEnemiesEffect();
                        break;
                    default:
                        Debug.LogWarning("지원되지 않는 카드 종류입니다: " + item.name);
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Scene에 EnemyStats 클래스의 인스턴스가 없습니다.");
            }
            UseCards.RemoveAll(card => card.TypeTMP.text == "Once");
        }
        // Debug.Log("Used card number: " + Mathf.RoundToInt(item.number));
    }





    void DeckList()
    {
        foreach (var card in Deck)
        {
            var cardObject = Instantiate(cardPrefabs, deckSpawnPoint.position, Utils.QI);
            var cardComponent = cardObject.GetComponent<Card>();
            cardComponent.SetBackSprite(backCardSprite);

            cardComponent.Setup(card.GetItem());
        }
    }
    void SetOriginOrder()
    {
        int count = PlayerCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = PlayerCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void CardAlignment()
    {

        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, PlayerCards.Count, 0.5f, Vector3.one * 40f);
        var targetCards = PlayerCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;

            case 2: objLerps = new float[] { 0.4f, 0.6f }; break;

            case 3: objLerps = new float[] { 0.3f, 0.5f, 0.7f }; break;

            case 4: objLerps = new float[] { 0.2f, 0.4f, 0.6f, 0.8f }; break;

            case 5: objLerps = new float[] { 0.1f, 0.3f, 0.5f, 0.7f, 0.9f }; break;

            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if (objCount >= 6)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    void UseCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layerMyCard = LayerMask.NameToLayer("MyCardArea");
        int layerEnemy1 = LayerMask.NameToLayer("EnemyArea");
        int layerEnemy2 = LayerMask.NameToLayer("EnemyArea2");

        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layerMyCard);

        if (onMyCardArea)
        {
            EnemyArea_one = false;
            EnemyArea_two = false;
            return;
        }

        EnemyArea_one = Array.Exists(hits, x => x.collider.gameObject.layer == layerEnemy1);
        EnemyArea_two = Array.Exists(hits, x => x.collider.gameObject.layer == layerEnemy2);

        if (EnemyArea_one)
        {
            foreach (var hit in hits)
            {
                Card card = hit.collider.GetComponent<Card>();
                if (card != null && !AppliedEnemy_one.Contains(card))
                {

                }
            }
        }
        else if (EnemyArea_two)
        {
            foreach (var hit in hits)
            {
                Card card = hit.collider.GetComponent<Card>();
                if (card != null && !AppliedEnemy_two.Contains(card))
                {

                }
            }
        }

    }




    #region PlayerCard

    public void CardMouseOver(Card card)
    {
        if (!canInteract) return;
        if (card.IsFront())
        {
            selectCard = card;
            EnlargeCard(true, card);
            if(isMyCardDrag != true)
            {  
               UICardShow(); 
            }
        }
    }

    public void CardMouseExit(Card card)
    {
        if (!canInteract) return;
        if (card.IsFront())
        {
            EnlargeCard(false, card);
            if (currentCardObject != null)
            {
                Destroy(currentCardObject); 
                currentCardObject = null;  
            }
        }
    }

    public void CardMouseDown()
    {
        if (!canInteract) return;
        if (deckClicked) return;

        isMyCardDrag = true;

        if (currentCardObject != null)
        {
            Destroy(currentCardObject);
            currentCardObject = null;
        }
    }

    /*  -> 혹시 모를 오류때문에, 기존 CardMouseUp 함수 남겨둠
    public void CardMouseUp()
    {
        
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        isMyCardDrag = false;

        if (selectCard != null)
        {
            // UseCardEnlargeEffect를 먼저 실행
            UseCardEnlargeEffect(selectCard); 
        }

        if (EnemyArea_one && selectCard != null)
        {
            Item usedItem = selectCard.GetItem();
            if (playerStats.currentMana >= usedItem.cost)
            {
                playerStats.currentMana -= usedItem.cost;

                if (usedItem != null)
                {
                    print("Used item: " + usedItem.name);
                    enemynumber = 0;
                    UseCard(usedItem);
                    AppliedEnemy_one.Add(selectCard);
                }
                else
                {
                    print("No card to use.");
                }
                PlayerCards.Remove(selectCard);
                selectCard.gameObject.SetActive(false);

                CardAlignment();
                return;
            }
        }
        else if (EnemyArea_two && selectCard != null)
        {
            Item usedItem = selectCard.GetItem();
            if (playerStats.currentMana >= usedItem.cost)
            {
                playerStats.currentMana -= usedItem.cost;
                if (usedItem != null)
                {
                    print("Used item: " + usedItem.name);
                    enemynumber = 1;
                    UseCard(usedItem);
                    AppliedEnemy_two.Add(selectCard);
                }
                else
                {
                    print("No card to use.");
                }
                PlayerCards.Remove(selectCard);
                selectCard.gameObject.SetActive(false);

                CardAlignment();
                return;
            }
        }
        else
        {
            //selectCard.MoveTransform(selectCard.originPRS, false);
            Debug.Log("카드 이상한곳에 뒀을 때, 위치갱신");
            CardAlignment();
        }
    }
    */
    public IEnumerator CardMouseUp()
    {
        if (deckClicked) yield break;
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        isMyCardDrag = false;

        canInteract = false;
        Card card = FindObjectOfType<Card>(); //메모리 누수시 수정
        // EnemyArea_one에서 카드 사용
        if (EnemyArea_one && selectCard != null)
        {
            Item usedItem = selectCard.GetItem();
            if (playerStats.currentMana >= usedItem.cost)
            {
                
                
                card.CardUseSoundController(usedItem.number);
                yield return StartCoroutine(PlayEffectAndHideCard(selectCard));
                playerStats.currentMana -= usedItem.cost;

                if (usedItem != null)
                {
                    print("Used item: " + usedItem.name);
                    enemynumber = 0;
                    UseCard(usedItem);
                    AppliedEnemy_one.Add(selectCard);
                }
                else
                {
                    print("No card to use.");
                }
                PlayerCards.Remove(selectCard);
                selectCard.gameObject.SetActive(false);
                //yield break;
            }
        }
        // EnemyArea_two에서 카드 사용
        else if (EnemyArea_two && selectCard != null)
        {
            Item usedItem = selectCard.GetItem();
            if (playerStats.currentMana >= usedItem.cost)
            {
                card.CardUseSoundController(usedItem.number);
                yield return StartCoroutine(PlayEffectAndHideCard(selectCard));
                playerStats.currentMana -= usedItem.cost;
                if (usedItem != null)
                {
                    print("Used item: " + usedItem.name);
                    enemynumber = 1;
                    UseCard(usedItem);
                    AppliedEnemy_two.Add(selectCard);
                }
                else
                {
                    print("No card to use.");
                }
                PlayerCards.Remove(selectCard);
                selectCard.gameObject.SetActive(false);
                //yield break;
            }
        }
        // 카드가 사용되지 않은 경우 원래 위치로 되돌림
        CardAlignment();
        canInteract = true;
    }


    void CardDrag()
    {
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);

        }
    }


    IEnumerator PlayEffectAndHideCard(Card card)
    {
        var spriteRenderer = card.GetComponent<SpriteRenderer>();
        int originalOrder = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = 1000;

        var characterRenderer = card.character.GetComponent<SpriteRenderer>();
        int characteroriginalOrder = characterRenderer.sortingOrder;
        characterRenderer.sortingOrder = 900;

        var nameRenderer = card.nameTMP.GetComponent<MeshRenderer>();
        int nameoriginalOrder = nameRenderer.sortingOrder;
        nameRenderer.sortingOrder = 1100;

        var effectRenderer = card.effectTMP.GetComponent<MeshRenderer>();
        int effectoriginalOrder = effectRenderer.sortingOrder;
        effectRenderer.sortingOrder = 1100;

        var costRenderer = card.costTMP.GetComponent<MeshRenderer>();
        int costoriginalOrder = costRenderer.sortingOrder;
        costRenderer.sortingOrder = 1100;

        var typeRenderer = card.TypeTMP.GetComponent<MeshRenderer>();
        int typeoriginalOrder = typeRenderer.sortingOrder;
        typeRenderer.sortingOrder = 1100;

        //파티클 이펙트 렌더링
        var particleSystems = new List<ParticleSystem>
        {
            card.cardEffectUseParticle1,
            card.cardEffectUseParticle2,
            card.cardEffectUseParticle3,
            card.cardEffectUseParticle4,
            card.cardEffectUseParticle5,
            card.cardEffectUseParticle6,
            card.cardEffectUseParticle7,
            card.cardEffectUseParticle8
        };

        var particleOriginalOrders = new List<int>();
        foreach (var particleSystem in particleSystems)
        {
            if (particleSystem != null)
            {
                var particleRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                particleOriginalOrders.Add(particleRenderer.sortingOrder);
                particleRenderer.sortingOrder = originalOrder + 1000; // 카드보다 1000 높게 설정
            }
            else
            {
                particleOriginalOrders.Add(0); // null인 경우( 이페그트 없으면) 기본값
            }
        }

        Vector3 enlargePos = cardUseEffectPoint.position;
        card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 180f), false);

        card.PlayCardUseEffect();

        float multiplier = 5.0f; // 크기 조정 비율

        // 파티클 시스템들의 원래 크기를 저장할 리스트
        var originalStartSizes = new List<(float min, float max)>();

        // 각 파티클 시스템에 대해 크기를 5배로 조정
        foreach (var particleSystem in particleSystems)
        {
            if (particleSystem != null)
            {
                var mainModule = particleSystem.main;

                // 현재 startSize 값을 가져옴
                var startSize = mainModule.startSize;

                // 원래 크기 저장
                originalStartSizes.Add((startSize.constantMin, startSize.constantMax));

                startSize.constantMin *= multiplier;
                startSize.constantMax *= multiplier;
                mainModule.startSize = startSize;
            }
            else
            {
                // 만약 null인 경우 기본값을 저장
                originalStartSizes.Add((0, 0));
            }
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < particleSystems.Count; i++)
        {
            if (particleSystems[i] != null)
            {
                var mainModule = particleSystems[i].main;

                var originalSize = originalStartSizes[i];
                var startSize = mainModule.startSize;
                startSize.constantMin = originalSize.min;
                startSize.constantMax = originalSize.max;
                mainModule.startSize = startSize;
            }
        }

        spriteRenderer.sortingOrder = originalOrder;
        characterRenderer.sortingOrder = characteroriginalOrder;
        nameRenderer.sortingOrder = nameoriginalOrder;
        effectRenderer.sortingOrder = effectoriginalOrder;
        costRenderer.sortingOrder = costoriginalOrder;
        typeRenderer.sortingOrder = typeoriginalOrder;

        for (int i = 0; i < particleSystems.Count; i++)
        {
            if (particleSystems[i] != null)
            {
                var particleRenderer = particleSystems[i].GetComponent<ParticleSystemRenderer>();
                particleRenderer.sortingOrder = particleOriginalOrders[i];
            }
        }

        // 이펙트 후 카드의 위치 변경
        Vector3 returnPos = cardUseEffectPoint2.position;
        card.MoveTransform(new PRS(returnPos, Utils.QI, Vector3.one * 70f), false);
    }

    void EnlargeCard(bool isEnlarge, Card card) // onMyCardArea영역 밖의 카드에 마우스를 올릴 경우, 카드가 사라지지 않고 그 자리에서 팝업됨
    {
        if (onMyCardArea)
        {
            if (isEnlarge)
            {
                Vector3 enlargePos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y + 35f, -10f);
                card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 70f), false);

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
                card.MoveTransform(card.originPRS, false);
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

            card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);

        }
        else
        {
            if (isEnlarge)
            {
                // 현재 카드의 위치(currentPRS)를 기준으로 확대 위치 계산
                Vector3 enlargePos = new Vector3(card.currentPRS.pos.x, card.currentPRS.pos.y + 25f, -10f);
                card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 90f), false);

                card.PlayCardEffect();

                var mainModule = card.cardEffectParticle.main;
                mainModule.startSize = isMyCardDrag ? 1.0f : 2.5f;

                if (card.cardEffectParticle2 != null)
                {
                    var mainModule2 = card.cardEffectParticle2.main;
                    mainModule2.startSize = isMyCardDrag ? 1.0f : 2.5f;  // 드래그 여부에 따라 크기 설정
                }
                if (card.cardEffectParticle3 != null)
                {
                    var mainModule3 = card.cardEffectParticle3.main;
                    mainModule3.startSize = isMyCardDrag ? 1.0f : 2.5f;
                }
                if (card.cardEffectParticle4 != null)
                {
                    var mainModule4 = card.cardEffectParticle4.main;
                    mainModule4.startSize = isMyCardDrag ? 1.0f : 2.5f;
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
    }

    #endregion

    /*
    void EnlargeDeckTomb(bool isEnlarge, List<Card> cards)
    {
        if (isEnlarge)
        {
            // 시작 위치 초기화
            Vector3 startPosition = new Vector3(630f, 840f, 0f);
            // 카드 사이의 간격 설정
            float cardOffsetX = 250f;
            float cardOffsetY = 300f;
            int cardIndex = 0;
            // 모든 카드에 대해서 반복하여 처리

            for (int i = 0; i <= cards.Count / 5; i++)
            {

                for (int j = 0; j < 5; j++)
                {
                    if (cardIndex > (cards.Count - 1) || cards.Count == 0) { break; }
                    Vector3 enlargePos = startPosition + new Vector3(j * cardOffsetX, -i * cardOffsetY, 0f);
                    // 확대된 카드의 위치, 회전 및 크기 설정
                    // 아래를 통해, 덱 버튼을 클릭하여 팝업된 카드에 마우스를 올리면 사라지는 현상 없앤다. 카드의 현재 위치정보를 currentPRS에 새롭게 저장
                    cards[cardIndex].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 70f), false);
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
    */
    void CardType_ContOrOnce() // 안쓰임
    {
        for (int i = 0; i < AppliedEnemy_one.Count; i++)
        {
            if (AppliedEnemy_one[i].TypeTMP.text == "Once")
            {
                UseCardTpye_Once.Add(AppliedEnemy_one[i]);
            }
            else if (AppliedEnemy_one[i].TypeTMP.text == "Cont")
            {
                UseCardTpye_Cont.Add(AppliedEnemy_one[i]);
            }
        }

        for (int i = 0; i < AppliedEnemy_two.Count; i++)
        {
            if (AppliedEnemy_two[i].TypeTMP.text == "Once")
            {
                UseCardTpye_Once.Add(AppliedEnemy_two[i]);
            }
            else if (AppliedEnemy_two[i].TypeTMP.text == "Cont")
            {
                UseCardTpye_Cont.Add(AppliedEnemy_two[i]);
            }
        }

        AppliedEnemy_one.RemoveAll(card => card.TypeTMP.text == "Once");
        AppliedEnemy_one.RemoveAll(card => card.TypeTMP.text == "Cont");
        AppliedEnemy_two.RemoveAll(card => card.TypeTMP.text == "Once");
        AppliedEnemy_two.RemoveAll(card => card.TypeTMP.text == "Cont");
    }

    //아래는 방해카드가 현재 플레이어 패에 들어가는 함수(이 함수를 적이 호출하면, 인자로 입력한 카드(disturbCard)가 플레이어카드에 들어감.-> 덱으로
    public void AddDisturbCard(int disturbCardNumber)
    {
        var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();

        cardObject.transform.localScale = Vector3.one * 40f;

        card.Setup(cardList.MakeCardToItem(disturbCardNumber));
        Deck.Add(card);
        CardAlignment();

    }

    public void DeckViewButtonClick()
    {
        Debug.Log("DeckViewClick");

        if (DeckViewClick == true)
        {
            //EnlargeDeckTomb(DeckViewClick, Deck);
            MakeDeckList();
            panel.SetActive(true);
            DeckViewClick = false;
            deckClicked = true;

        }
        else if (DeckViewClick == false)
        {
            //EnlargeDeckTomb(DeckViewClick, Deck);
            panel.SetActive(false);
            DeckViewClick = true;
            deckClicked = false;
        }


    }
    public void UseCardViewButtonClick()
    {
        Debug.Log("UseCardViewClick");
        if (UseCardViewClick == true)
        {
            //EnlargeDeckTomb(UseCardViewClick, UseCards);
            MakeUseCardsList();
            panel.SetActive(true);
            UseCardViewClick = false;
        }
        else if (UseCardViewClick == false)
        {
            //EnlargeDeckTomb(UseCardViewClick, UseCards);
            panel.SetActive(false);
            UseCardViewClick = true;
        }

    }
    void MakeDeckList()
    {

        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < Deck.Count; i++)
        {
            var cardObject = Instantiate(uicardPrefabs, scrollContent);
            var card = cardObject.GetComponent<UICard>();

            cardObject.transform.localScale = Vector3.one * 1.6f;
            card.Setup(cardList.MakeCardToItem(Deck[i].cardNumber));
        }
    }

    void MakeUseCardsList()
    {
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < UseCards.Count; i++)
        {
            var cardObject = Instantiate(uicardPrefabs, scrollContent);
            var card = cardObject.GetComponent<UICard>();

            cardObject.transform.localScale = Vector3.one * 1.5f;
            card.Setup(cardList.MakeCardToItem(UseCards[i].cardNumber));
        }
    }

    public void UICardShow()
    {
        
        if (currentCardObject != null)
        {
            Destroy(currentCardObject);
        }

        currentCardObject = Instantiate(uicardPrefabs, new Vector3(1150f, -400f, -10f), Utils.QI);

        currentCardObject.transform.SetParent(canvas.transform, false);

        var card = currentCardObject.GetComponent<UICard>();
        currentCardObject.transform.localScale = Vector3.one * 2.5f;
        card.Setup(cardList.MakeCardToItem(selectCard.cardNumber));
    }

}
