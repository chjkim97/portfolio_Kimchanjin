using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ability : MonoBehaviour
{
    public TextMeshProUGUI startGoldLevelText;
    public TextMeshProUGUI maxHealthLevelText;
    public TextMeshProUGUI randomRelicLevelText;
    public TextMeshProUGUI powerLevelText;
    public TextMeshProUGUI diamondGainLevelText;

    public TextMeshProUGUI startGoldEffectText; // 추가된 텍스트
    public TextMeshProUGUI maxHealthEffectText; // 추가된 텍스트
    public TextMeshProUGUI randomRelicEffectText; // 추가된 텍스트
    public TextMeshProUGUI powerEffectText; // 추가된 텍스트
    public TextMeshProUGUI diamondGainEffectText; // 추가된 텍스트
    public Button startGoldUpgradeButton;
    public Button maxHealthUpgradeButton;
    public Button randomRelicUpgradeButton;
    public Button powerUpgradeButton;
    public Button diamondGainUpgradeButton;
    public Button resetButton;
    public Button backButton;

    private int startGoldUpgradeCost;
    private int maxHealthUpgradeCost;
    private int randomRelicUpgradeCost;
    private int powerUpgradeCost;
    private int diamondGainUpgradeCost;

    private const int initialUpgradeCost = 100;
    private const int maxLevel = 5;

    void Start()
    {
        LoadUpgradeCosts();
        UpdateUI();

        startGoldUpgradeButton.onClick.AddListener(() => UpgradeAbility("StartGold"));
        maxHealthUpgradeButton.onClick.AddListener(() => UpgradeAbility("MaxHealth"));
        randomRelicUpgradeButton.onClick.AddListener(() => UpgradeAbility("RandomRelic"));
        powerUpgradeButton.onClick.AddListener(() => UpgradeAbility("Power"));
        diamondGainUpgradeButton.onClick.AddListener(() => UpgradeAbility("DiamondGain"));
        resetButton.onClick.AddListener(ResetUpgrades);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void LoadUpgradeCosts()
    {
        startGoldUpgradeCost = PlayerPrefs.GetInt("StartGoldUpgradeCost", initialUpgradeCost * (GameData.Instance.startGoldLevel + 1));
        maxHealthUpgradeCost = PlayerPrefs.GetInt("MaxHealthUpgradeCost", initialUpgradeCost * (GameData.Instance.maxHealthLevel + 1));
        randomRelicUpgradeCost = PlayerPrefs.GetInt("RandomRelicUpgradeCost", initialUpgradeCost * (GameData.Instance.randomRelicLevel + 1));
        powerUpgradeCost = PlayerPrefs.GetInt("PowerUpgradeCost", initialUpgradeCost * (GameData.Instance.powerLevel + 1));
        diamondGainUpgradeCost = PlayerPrefs.GetInt("DiamondGainUpgradeCost", initialUpgradeCost * (GameData.Instance.diamondGainLevel + 1));
    }

    void SaveUpgradeCosts()
    {
        PlayerPrefs.SetInt("StartGoldUpgradeCost", startGoldUpgradeCost);
        PlayerPrefs.SetInt("MaxHealthUpgradeCost", maxHealthUpgradeCost);
        PlayerPrefs.SetInt("RandomRelicUpgradeCost", randomRelicUpgradeCost);
        PlayerPrefs.SetInt("PowerUpgradeCost", powerUpgradeCost);
        PlayerPrefs.SetInt("DiamondGainUpgradeCost", diamondGainUpgradeCost);
    }

    void UpdateUI()
    {
        startGoldLevelText.text = "시작 골드 레벨 " + GameData.Instance.startGoldLevel;
        maxHealthLevelText.text = "최대 체력 레벨 " + GameData.Instance.maxHealthLevel;
        randomRelicLevelText.text = "랜덤 유물 레벨 " + GameData.Instance.randomRelicLevel;
        powerLevelText.text = "힘 레벨 " + GameData.Instance.powerLevel;
        diamondGainLevelText.text = "다이아 획득량 레벨 " + GameData.Instance.diamondGainLevel;

        UpdateButtonText(startGoldUpgradeButton, GameData.Instance.startGoldLevel, startGoldUpgradeCost);
        UpdateButtonText(maxHealthUpgradeButton, GameData.Instance.maxHealthLevel, maxHealthUpgradeCost);
        UpdateButtonText(randomRelicUpgradeButton, GameData.Instance.randomRelicLevel, randomRelicUpgradeCost);
        UpdateButtonText(powerUpgradeButton, GameData.Instance.powerLevel, powerUpgradeCost);
        UpdateButtonText(diamondGainUpgradeButton, GameData.Instance.diamondGainLevel, diamondGainUpgradeCost);

         UpdateAbilityEffects();
    }
     void UpdateAbilityEffects()
    {
        startGoldEffectText.text = "게임 시작 시 골드를 " + (GameData.Instance.startGoldLevel * 50) + "만큼 추가로 가지고 시작합니다.";
        maxHealthEffectText.text = "최대 체력이 " + (GameData.Instance.maxHealthLevel * 3) + " 만큼 증가합니다.";
        randomRelicEffectText.text = "게임 시작 시 유물을 " + GameData.Instance.randomRelicLevel + "개 추가로 얻습니다.";
        powerEffectText.text = "전투 시작 시 20% 확률로 힘 +1 을" + (GameData.Instance.powerLevel) + "회 반복합니다.";
        diamondGainEffectText.text = "다이아몬드를" + (GameData.Instance.diamondGainLevel * 20) + "% 추가로 얻습니다.";
    }
    void UpdateButtonText(Button button, int currentLevel, int upgradeCost)
    {
        var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (currentLevel >= maxLevel)
        {
            buttonText.text = "MAX";
            button.interactable = false;
        }
        else
        {
            buttonText.text = "업그레이드 비용: " + upgradeCost + " 다이아몬드";
            button.interactable = true;
        }
    }

    void UpgradeAbility(string abilityType)
    {
        int currentLevel = 0;
        int upgradeCost = 0;

        switch (abilityType)
        {
            case "StartGold":
                currentLevel = GameData.Instance.startGoldLevel;
                upgradeCost = startGoldUpgradeCost;
                break;
            case "MaxHealth":
                currentLevel = GameData.Instance.maxHealthLevel;
                upgradeCost = maxHealthUpgradeCost;
                break;
            case "RandomRelic":
                currentLevel = GameData.Instance.randomRelicLevel;
                upgradeCost = randomRelicUpgradeCost;
                break;
            case "Power":
                currentLevel = GameData.Instance.powerLevel;
                upgradeCost = powerUpgradeCost;
                break;
            case "DiamondGain":
                currentLevel = GameData.Instance.diamondGainLevel;
                upgradeCost = diamondGainUpgradeCost;
                break;
        }

        if (currentLevel >= maxLevel)
        {
            Debug.Log(abilityType + " is already at max level.");
            return;
        }

        if (GameData.Instance.diamonds >= upgradeCost)
        {
            GameData.Instance.diamonds -= upgradeCost;

            switch (abilityType)
            {
                case "StartGold":
                    GameData.Instance.startGoldLevel++;
                    startGoldUpgradeCost += 100;
                    break;
                case "MaxHealth":
                    GameData.Instance.maxHealthLevel++;
                    maxHealthUpgradeCost += 100;
                    break;
                case "RandomRelic":
                    GameData.Instance.randomRelicLevel++;
                    randomRelicUpgradeCost += 100;
                    break;
                case "Power":
                    GameData.Instance.powerLevel++;
                    powerUpgradeCost += 100;
                    break;
                    //전투마다 0.2,0.4,0.6,0.8,1.0 확률로 힘+1
                case "DiamondGain":
                    GameData.Instance.diamondGainLevel++;
                    diamondGainUpgradeCost += 100;
                    break;
            }

            GameData.Instance.SaveData();
            SaveUpgradeCosts();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough diamonds to upgrade " + abilityType);
        }
    }

    void ResetUpgrades()
    {
        GameData.Instance.startGoldLevel = 0;
        GameData.Instance.maxHealthLevel = 0;
        GameData.Instance.randomRelicLevel = 0;
        GameData.Instance.powerLevel = 0;
        GameData.Instance.diamondGainLevel = 0;
        GameData.Instance.buyCard = 0;

        startGoldUpgradeCost = initialUpgradeCost;
        maxHealthUpgradeCost = initialUpgradeCost;
        randomRelicUpgradeCost = initialUpgradeCost;
        powerUpgradeCost = initialUpgradeCost;
        diamondGainUpgradeCost = initialUpgradeCost;

        GameData.Instance.SaveData();
        SaveUpgradeCosts();
        UpdateUI();
    }
    void OnBackButtonClicked()
    {
        SceneManager.LoadScene("GameStart");
    }
}
