using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [SerializeField]  Image card;
    public Image character;
    public TMP_Text nameTMP;
    public TMP_Text effectTMP;
    public TMP_Text costTMP;
    public TMP_Text TypeTMP;
    public int cardNumber;

    public Item item;
    public string Type2;
    // Start is called before the first frame update


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
    }


    public Item GetItem()
    {
        return item; // 혹은 카드의 다른 속성으로부터 Item을 만들어 반환
    }

}
