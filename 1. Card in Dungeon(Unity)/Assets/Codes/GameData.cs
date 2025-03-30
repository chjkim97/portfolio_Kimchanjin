using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int startGoldLevel = 1;
    public int maxHealthLevel = 1;
    public int randomRelicLevel = 1;
    public int powerLevel = 1;
    public int diamondGainLevel = 1;

    public int diamonds = 0;

    public int buyCard = 0;
    public int bosscount = 0;
    public int itemCount = 0;

    public float elapsedTime = 0f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;  
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("StartGoldLevel", startGoldLevel);
        PlayerPrefs.SetInt("MaxHealthLevel", maxHealthLevel);
        PlayerPrefs.SetInt("RandomRelicLevel", randomRelicLevel);
        PlayerPrefs.SetInt("powerLevel", powerLevel);
        PlayerPrefs.SetInt("DiamondGainLevel", diamondGainLevel);
        PlayerPrefs.SetInt("Diamonds", diamonds);
        PlayerPrefs.SetInt("BuyCard", buyCard);
        PlayerPrefs.SetInt("Bosscount", bosscount);
        PlayerPrefs.SetInt("ItemCount", itemCount);
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
    }
    public void LoadData()
    {
        startGoldLevel = PlayerPrefs.GetInt("StartGoldLevel", 0);
        maxHealthLevel = PlayerPrefs.GetInt("MaxHealthLevel", 0);
        randomRelicLevel = PlayerPrefs.GetInt("RandomRelicLevel", 0);
        powerLevel = PlayerPrefs.GetInt("powerLevel", 0);
        diamondGainLevel = PlayerPrefs.GetInt("DiamondGainLevel", 0);
        diamonds = PlayerPrefs.GetInt("Diamonds", 0);
        buyCard = PlayerPrefs.GetInt("BuyCard", 0);
        bosscount = PlayerPrefs.GetInt("Bosscount", 0);
        itemCount = PlayerPrefs.GetInt("ItemCount", 0);
        elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0);
    }

    public void AddDiamonds(int num)
    {
        float multiplier = 1 + (diamondGainLevel * 0.2f);
        int totalDiamonds = Mathf.RoundToInt(num * multiplier);

        diamonds += totalDiamonds;

        SaveData();

        Debug.Log($"Added {totalDiamonds} diamonds. New total: {diamonds}");
    }

    public void BuyCard()
    {
        buyCard++;

        SaveData();

        Debug.Log($"Added 1 card. 현재까지 구입한 카드 수: {buyCard}");
    }
    public void Bosscount()
    {
        bosscount++;

        SaveData();

        Debug.Log($"보스카운트: {bosscount}");
    }
    public void ItemCount()
    {
        itemCount = 1;
    }


}
