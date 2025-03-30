using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    public SpriteRenderer character;
    public TMP_Text nameTMP;
    public TMP_Text effectTMP;
    public TMP_Text costTMP;
    [SerializeField] Sprite cardFront;
    [SerializeField] Sprite cardBack;
    public TMP_Text TypeTMP;
    public int cardNumber;

    public ParticleSystem cardEffectParticle;
    public ParticleSystem cardEffectParticle2;
    public ParticleSystem cardEffectParticle3;
    public ParticleSystem cardEffectParticle4;
    public ParticleSystem cardEffectParticle5;

    public ParticleSystem cardEffectUseParticle1;
    public ParticleSystem cardEffectUseParticle2;
    public ParticleSystem cardEffectUseParticle3;
    public ParticleSystem cardEffectUseParticle4;
    public ParticleSystem cardEffectUseParticle5;
    public ParticleSystem cardEffectUseParticle6;
    public ParticleSystem cardEffectUseParticle7;
    public ParticleSystem cardEffectUseParticle8;

    public Item item;
    public PRS originPRS;
    public PRS currentPRS;
    public string Type2;

    public bool selectedCard = false;
    public void MoveTransform(PRS prs, bool useDotweem, float dotweenTime = 0)
    {
        if (useDotweem)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    public void Setup(Item item)
    {
        this.item = item;

        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        effectTMP.text = this.item.effect.ToString();
        costTMP.text = this.item.cost.ToString();
        TypeTMP.text = this.item.type.ToString();
        Type2 = this.item.type2.ToString();

        cardNumber = this.item.number;
        if (IsFront())
        {
            card.sprite = cardFront;
        }
    }


    public void SetBackSprite(Sprite backSprite)
    {
        // 카드의 뒷면 스프라이트 설정
        card.sprite = backSprite;

        Transform character = transform.Find("Character");
        Transform Effect = transform.Find("EffectTMP");
        Transform Name = transform.Find("NameTMP");
        Transform Cost = transform.Find("CostTMP");
        Transform Type = transform.Find("TypeTMP");


        if (character != null)
        {
            character.gameObject.SetActive(false);
        }
        if (Effect != null)
        {
            Effect.gameObject.SetActive(false);
        }
        if (Name != null)
        {
            Name.gameObject.SetActive(false);
        }
        if (Cost != null)
        {
            Cost.gameObject.SetActive(false);
        }
        if (Type != null)
        {
            Type.gameObject.SetActive(false);
        }
    }

    public bool IsFront()
    {
        return card.sprite == cardFront;
    }
    public Item GetItem()
    {
        return item; // 혹은 카드의 다른 속성으로부터 Item을 만들어 반환
    }

    public void PlayCardEffect()
    {
        if (cardEffectParticle.isStopped)
        {
            cardEffectParticle.Play();
            Debug.Log("particle play");
        }
        if (cardEffectParticle2 != null)
        {
            if (cardEffectParticle2.isStopped)
            {
                cardEffectParticle2.Play();
                Debug.Log("particle2 play");
            }
        }
        if (cardEffectParticle3 != null)
        {
            if (cardEffectParticle3.isStopped)
            {
                cardEffectParticle3.Play();
                Debug.Log("particle3 play");
            }
        }
        if (cardEffectParticle4 != null)
        {
            if (cardEffectParticle4.isStopped)
            {
                cardEffectParticle4.Play();
                Debug.Log("particle4 play");
            }
        }
    }

    public void StopCardEffect()
    {
        if (cardEffectParticle.isPlaying)
        {
            cardEffectParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Debug.Log("Particle effect turned off");
        }
        if (cardEffectParticle2 != null)
        {
            if (cardEffectParticle2.isPlaying)
            {
                cardEffectParticle2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                Debug.Log("Particle2 effect turned off");
            }
        }
        if (cardEffectParticle3 != null)
        {
            if (cardEffectParticle3.isPlaying)
            {
                cardEffectParticle3.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                Debug.Log("Particle3 effect turned off");
            }
        }
        if (cardEffectParticle4 != null)
        {
            if (cardEffectParticle4.isPlaying)
            {
                cardEffectParticle4.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                Debug.Log("Particle4 effect turned off");
            }
        }
    }

    public void PlayCardUseEffect()
    {
        if (cardEffectUseParticle1.isStopped)
        {
            cardEffectUseParticle1.Play();
            Debug.Log("Use particle play");
        }
        if (cardEffectUseParticle2 != null)
        {
            if (cardEffectUseParticle2.isStopped)
            {
                cardEffectUseParticle2.Play();
                Debug.Log("Use particle2 play");
            }
        }
        if (cardEffectUseParticle3 != null)
        {
            if (cardEffectUseParticle3.isStopped)
            {
                cardEffectUseParticle3.Play();
                Debug.Log("Use particle3 play");
            }
        }
        if (cardEffectUseParticle4 != null)
        {
            if (cardEffectUseParticle4.isStopped)
            {
                cardEffectUseParticle4.Play();
                Debug.Log("Use particle4 play");
            }
        }
        if (cardEffectUseParticle5 != null)
        {
            if (cardEffectUseParticle5.isStopped)
            {
                cardEffectUseParticle5.Play();
                Debug.Log("Use particle5 play");
            }
        }
        if (cardEffectUseParticle6 != null)
        {
            if (cardEffectUseParticle6.isStopped)
            {
                cardEffectUseParticle6.Play();
                Debug.Log("Use particle6 play");
            }
        }
        if (cardEffectUseParticle7 != null)
        {
            if (cardEffectUseParticle7.isStopped)
            {
                cardEffectUseParticle7.Play();
                Debug.Log("Use particle7 play");
            }
        }
        if (cardEffectUseParticle8 != null)
        {
            if (cardEffectUseParticle8.isStopped)
            {
                cardEffectUseParticle8.Play();
                Debug.Log("Use particle8 play");
            }
        }
    }



    void OnMouseOver()
    {

        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            CardManager.instance.CardMouseOver(this);
        }
        else if (SceneManager.GetActiveScene().name == "CardView")
        {
            CardPack.instance.CardMouseOver(this);
        }
        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            var shop = FindObjectOfType<Shop>();
            if (shop != null)
            {
                shop.CardMouseOver(this);
            }
        }
        else { }
    }

    void OnMouseExit()
    {
        StopCardEffect();

        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            CardManager.instance.CardMouseExit(this);
        }
        else if (SceneManager.GetActiveScene().name == "CardView")
        {
            CardPack.instance.CardMouseExit(this);
        }
        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            var shop = FindObjectOfType<Shop>();
            if (shop != null)
            {
                shop.CardMouseExit(this);
            }
        }
        else { }

    }

    void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            CardManager.instance.CardMouseDown();
        }
        else if (SceneManager.GetActiveScene().name == "CardView")
        {
            CardPack.instance.CardMouseDown(this);
        }
        else { }
    }

    void OnMouseUp()
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            CardManager.instance.StartCoroutine(CardManager.instance.CardMouseUp());
        }
        else { }
    }
    void CardEffectController(int cardNumber)
    {
        // 디폴트 이펙트
        cardEffectParticle = transform.Find("CardEffectDefault/CrossGlow").GetComponent<ParticleSystem>();
        cardEffectParticle2 = null;
        cardEffectParticle3 = null;
        cardEffectParticle4 = null;

        switch (cardNumber)
        {
            // flame 이펙트
            case 301:
            case 305:
            case 310:
            case 311:
            case 313:
            case 314:
            case 316:
            case 317:
                cardEffectParticle = transform.Find("CardEffectFlame/Trail1").GetComponent<ParticleSystem>();
                cardEffectParticle2 = transform.Find("CardEffectFlame/Trail2").GetComponent<ParticleSystem>();
                break;

            // Web 이펙트
            case 212:
            case 213:
            case 214:
            case 219:
            case 220:
                cardEffectParticle = transform.Find("CardEffectWeb/WebEffect1").GetComponent<ParticleSystem>();
                cardEffectParticle2 = transform.Find("CardEffectWeb/WebEffect2").GetComponent<ParticleSystem>();
                break;

            // Bright 이펙트
            case 21:
            case 22:
                cardEffectParticle = transform.Find("CardEffectBright/Swirly1").GetComponent<ParticleSystem>();
                cardEffectParticle2 = transform.Find("CardEffectBright/Swirly2").GetComponent<ParticleSystem>();
                break;

            // Glow 이펙트
            case 102:
                cardEffectParticle = transform.Find("CardEffectGlow/BackgroundGlow1").GetComponent<ParticleSystem>();
                cardEffectParticle2 = transform.Find("CardEffectGlow/BackgroundGlow2").GetComponent<ParticleSystem>();
                cardEffectParticle3 = transform.Find("CardEffectGlow/BackgroundGlow3").GetComponent<ParticleSystem>();
                cardEffectParticle4 = transform.Find("CardEffectGlow/BackgroundGlow4").GetComponent<ParticleSystem>();
                break;

            // Heal 이펙트
            case 8:
                cardEffectParticle = transform.Find("CardEffectHeal/SceneLightsBG1").GetComponent<ParticleSystem>();
                cardEffectParticle2 = transform.Find("CardEffectHeal/SceneLightsBG2").GetComponent<ParticleSystem>();
                break;

            // Smoke 이펙트
            case 211:
            case 215:
            case 216:
            case 217:
                cardEffectParticle = transform.Find("CardEffectSmoke/SmokeEffect").GetComponent<ParticleSystem>();
                break;

            // Storm 이펙트
            case 11:
                cardEffectParticle = transform.Find("CardEffectStorm/SquareColorFalmes1").GetComponent<ParticleSystem>();
                cardEffectParticle2 = transform.Find("CardEffectStorm/SquareColorFalmes2").GetComponent<ParticleSystem>();
                break;
        }
    }

    void CardUseEffectController(int cardNumber)
    {
        //디폴트
        cardEffectUseParticle1 = transform.Find("CardUseEffectSlice/SliceEffect").GetComponent<ParticleSystem>();
        cardEffectUseParticle2 = null;
        cardEffectUseParticle3 = null;
        cardEffectUseParticle4 = null;
        cardEffectUseParticle5 = null;
        cardEffectUseParticle6 = null;
        cardEffectUseParticle7 = null;
        cardEffectUseParticle8 = null;

        switch (cardNumber)
        {
            //Slice 이펙트
            case 1:
            case 201:
            case 202:
                cardEffectUseParticle1 = transform.Find("CardUseEffectSlice/SliceEffect").GetComponent<ParticleSystem>();
                break;

            //Claw 한번 이펙트
            case 103:
            case 5:
                cardEffectUseParticle1 = transform.Find("CardUseEffectClaw/ClawEffect").GetComponent<ParticleSystem>();
                break;

            //Claw 두번 이펙트
            case 11:
            case 109:
            case 309:
                cardEffectUseParticle1 = transform.Find("CardUseEffectDoubleClaw/ClawEffect1").GetComponent<ParticleSystem>();
                cardEffectUseParticle2 = transform.Find("CardUseEffectDoubleClaw/ClawEffect2").GetComponent<ParticleSystem>();
                break;

            //Heal 이펙트
            case 8:
                cardEffectUseParticle1 = transform.Find("CardUseEffectHeal/Stars").GetComponent<ParticleSystem>();
                cardEffectUseParticle2 = transform.Find("CardUseEffectHeal/Plus").GetComponent<ParticleSystem>();
                cardEffectUseParticle3 = transform.Find("CardUseEffectHeal/Symbol").GetComponent<ParticleSystem>();
                break;

            //Gorund Hit 이펙트
            case 4:
            case 102:
            case 310:
            case 311:
            case 313:
            case 312:
            case 314:
            case 316:
            case 317:
                cardEffectUseParticle1 = transform.Find("CardUseEffectGroundHit/SquareFire").GetComponent<ParticleSystem>();
                cardEffectUseParticle2 = transform.Find("CardUseEffectGroundHit/CircleExplosion").GetComponent<ParticleSystem>();
                break;

            //Fire Ball 이펙트
            case 301:
                cardEffectUseParticle1 = transform.Find("CardUseEffectFireBall/FireCircle").GetComponent<ParticleSystem>();
                cardEffectUseParticle2 = transform.Find("CardUseEffectFireBall/FireCircle2").GetComponent<ParticleSystem>();
                break;

            //Fire Ball, Ground Hit 이펙트
            case 305:
                cardEffectUseParticle1 = transform.Find("CardUseEffectGroundHit/SquareFire").GetComponent<ParticleSystem>();
                cardEffectUseParticle2 = transform.Find("CardUseEffectGroundHit/CircleExplosion").GetComponent<ParticleSystem>();
                cardEffectUseParticle3 = transform.Find("CardUseEffectFireBall/FireCircle").GetComponent<ParticleSystem>();
                cardEffectUseParticle4 = transform.Find("CardUseEffectFireBall/FireCircle2").GetComponent<ParticleSystem>();
                break;

            //Wing 이펙트
            case 21:
            case 22:
            case 208:
                cardEffectUseParticle1 = transform.Find("CardUseEffectWing/WingLinesEffect").GetComponent<ParticleSystem>();
                cardEffectUseParticle2 = transform.Find("CardUseEffectWing/TrailEffect").GetComponent<ParticleSystem>();
                break;

            //Lightning Attack 이펙트
            case 212:
            case 213:
            case 214:
            case 219:
            case 220:
                cardEffectUseParticle1 = transform.Find("CardUseEffectLightning/Electrical").GetComponent<ParticleSystem>(); //오류
                cardEffectUseParticle2 = transform.Find("CardUseEffectLightning/Electrical2").GetComponent<ParticleSystem>();
                cardEffectUseParticle3 = transform.Find("CardUseEffectLightning/Effect1/Lightning1").GetComponent<ParticleSystem>();
                cardEffectUseParticle4 = transform.Find("CardUseEffectLightning/Effect1/Sparks").GetComponent<ParticleSystem>();
                cardEffectUseParticle5 = transform.Find("CardUseEffectLightning/Effect2/Lightning2").GetComponent<ParticleSystem>();
                cardEffectUseParticle6 = transform.Find("CardUseEffectLightning/Effect2/Sparks2").GetComponent<ParticleSystem>();
                cardEffectUseParticle7 = transform.Find("CardUseEffectLightning/Effect3/Lightning3").GetComponent<ParticleSystem>();
                cardEffectUseParticle8 = transform.Find("CardUseEffectLightning/Effect4/Lightning4").GetComponent<ParticleSystem>();
                break;

        }
    }
    public void CardUseSoundController(int cardNumber)
    {
        SoundManager soundManager = FindObjectOfType<SoundManager>();


        switch (cardNumber)
        {
            case 1:
            case 103:
            case 201:
            case 202:
                if (soundManager != null)
                {
                    soundManager.PlaySound(7);
                }
                break;
            case 109:
                if (soundManager != null)
                {
                    soundManager.PlaySound(7);
                    soundManager.PlaySound(7);
                }
                break;
            case 8:
                if (soundManager != null)
                {
                    soundManager.PlaySound(4);
                }
                break;
            case 310:
            case 311:
            case 313:
            case 314:
            case 316:
            case 317:
                if (soundManager != null)
                {
                    soundManager.PlaySound(8);
                }
                break;
            case 301:
                if (soundManager != null)
                {
                    soundManager.PlaySound(2);
                }
                break;
            case 305:
                if (soundManager != null)
                {
                    soundManager.PlaySound(9);
                }
                break;
            case 21:
            case 22:
                if (soundManager != null)
                {
                    soundManager.PlaySound(10);
                }
                break;
            case 212:
            case 213:
            case 214:
            case 219:
            case 220:
                if (soundManager != null)
                {
                    soundManager.PlaySound(1);
                }
                break;
            default:
                if (soundManager != null)
                {
                    soundManager.PlaySound(11);
                }
                return;
        }
    }


    void Start()
    {
        Debug.Log("Start called");

        CardEffectController(cardNumber);
        CardUseEffectController(cardNumber);



        if (cardEffectParticle == null)
        {
            Debug.LogError("CardEffect 오브젝트에서 ParticleSystem을 찾을 수 없습니다.");
        }
        else
        {
            Debug.Log("Particle system successfully found");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
