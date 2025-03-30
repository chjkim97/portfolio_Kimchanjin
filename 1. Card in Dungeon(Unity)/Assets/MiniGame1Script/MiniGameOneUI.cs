using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using Unity.VisualScripting;


public class MiniGameOneUI : MonoBehaviour
{
    public TextMeshProUGUI animalNumberText;
    public TextMeshProUGUI foodNumberText;

    public Button animalBuyButton;
    public Button foodBuyButton;

    private int animalBuyCost = 100;
    private int foodBuyCost = 100;

    private const int maxNumber = 10;


    // 아래 코드를 위해서, GameData에 animal/foodNumber 추가해야 함
    void UpdateUI()
    {
       /*
        animalNumberText.text = GameData.Instance.animalNumber;
        foodNumberText.text = GameData.Instance.foodNumber;

        animalBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "동물 구매비용: " + animalBuyCost;
        foodBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "먹이 구매비용: " + foodBuyCost;

        animalBuyButton.interactable = GameData.Instance.animalNumber < maxNumber;
        startGoldUpgradeButton.interactable = GameData.Instance.foodNumber < maxNumber;
        */

    }
    

    void UpgradeAbility(string Type)
    {
        int currentLevel = 0;
        int upgradeCost = 0;
        /*
        switch (Type)
        {
            case "Animal":
                currentLevel = GameData.Instance.animalNumber;
                upgradeCost = animalBuyCost;
                break;
            case "Food":
                currentLevel = GameData.Instance.foodNumber;
                upgradeCost = foodBuyCost;
                break;
        }

        if (currentLevel >= maxNumber)
        {
            Debug.Log(Type + " is already Full.");
            return;
        }
        GameData.Instance.SaveData();
        UpdateUI();
        */

     }


        // Start is called before the first frame update
     void Start()
     {
         UpdateUI();
        animalBuyButton.onClick.AddListener(() => UpgradeAbility("Animal"));
        foodBuyButton.onClick.AddListener(() => UpgradeAbility("Food"));


    }

    // Update is called once per frame
    void Update()
     {

     }
 }

