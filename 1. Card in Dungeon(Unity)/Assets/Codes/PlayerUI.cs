using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;

    public GameObject shieldIcon;
    public Slider HpBarSlider;

    public GameObject[] tooltips; // 툴팁 패널 배열
    public TextMeshProUGUI[] tooltipTexts; // 툴팁 텍스트 배열

    // 버프 아이콘 및 텍스트
    public GameObject weakenIcon;
    public TextMeshProUGUI weakenText;
    public GameObject corrosionIcon;
    public TextMeshProUGUI corrosionText;
    public GameObject vulnerableIcon;
    public TextMeshProUGUI vulnerableText;
    public GameObject powerIcon;
    public TextMeshProUGUI powerText;
    public GameObject burnIcon;
    public TextMeshProUGUI burnText;

    // 새로운 버프 아이콘 및 텍스트
    public GameObject shieldCounterIcon;
    public TextMeshProUGUI shieldCounterText;
    public GameObject cardArmorIcon;
    public TextMeshProUGUI cardArmorText;
    public GameObject combatTrainingIcon;
    public TextMeshProUGUI combatTrainingText;
    public GameObject powerCounterIcon;
    public TextMeshProUGUI powerCounterText;
    public GameObject thornIcon;
    public TextMeshProUGUI thornText;
    public GameObject shieldManagerIcon;
    public TextMeshProUGUI shieldManagerText;
    public GameObject extraManaIcon;
    public TextMeshProUGUI extraManaText;
    public GameObject extraShieldIcon;
    public TextMeshProUGUI extraShieldText;
    public GameObject attackCountIcon;
    public TextMeshProUGUI attackCountText;
    public GameObject mDrawIcon;
    public TextMeshProUGUI mDrawText;

    void Start()
    {
        manaText = transform.Find("ManaText").GetComponent<TextMeshProUGUI>();
        healthText = transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        shieldText = transform.Find("ShieldText").GetComponent<TextMeshProUGUI>();
        
        foreach (GameObject tooltip in tooltips)
        {
            tooltip.SetActive(false);
        }

        // 기존 버프 및 디버프 툴팁 설정
        SetUpTooltip(weakenIcon, 0, "약화 : 공격으로 입히는 피해가 25%감소합니다");
        SetUpTooltip(corrosionIcon, 1, "부식 : 얻는 실드가 25% 감소합니다");
        SetUpTooltip(vulnerableIcon, 2, "취약 : 받는 피해가 50% 증가합니다");
        SetUpTooltip(powerIcon, 3, "힘 : 공격할 때 힘 수치만큼 피해를 더 입힙니다");
        SetUpTooltip(burnIcon, 4, "화상 : 턴이 끝날 때 화상 수치만큼 데미지를 입습니다");

        // 새로운 버프 및 디버프 툴팁 설정
        SetUpTooltip(shieldCounterIcon, 5, "방어 태세 : 카드를 사용할 때마다 실드를 4 얻습니다");
        SetUpTooltip(cardArmorIcon, 6, "천 덧대기 : 카드를 사용할 때마다 실드를 얻습니다");
        SetUpTooltip(combatTrainingIcon, 7, "전투 훈련 : 턴이 끝날 때 힘을 4 얻습니다");
        SetUpTooltip(powerCounterIcon, 8, "어둠의 거래  : 턴이 끝날 때마다 힘을 3 얻습니다");
        SetUpTooltip(thornIcon, 9, "가시 : 공격받았을 때 공격한 적에게 피해를 1 줍니다");
        SetUpTooltip(shieldManagerIcon, 10, "방패 방어술 : 턴이 끝날 때 실드를 잃지 않습니다");
        SetUpTooltip(extraManaIcon, 11, "추가 마나 : 다음 턴에 마나를 추가로 획득합니다");
        SetUpTooltip(extraShieldIcon, 12, "추가 실드 : 다음 턴에 실드를 추가로 획득합니다");
        SetUpTooltip(attackCountIcon, 13, "전사의 부적 : 공격 카드를 5번 사용하면 힘을 1 얻습니다");
        SetUpTooltip(mDrawIcon, 14, "드로우 제한 : 다음 턴에 뽑는 카드 수가 감소합니다");

        InvokeRepeating("UpdateUI", 0, 0.2f);
    }

    private void SetUpTooltip(GameObject icon, int index, string message)
    {
        EventTrigger iconTrigger = icon.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { ShowTooltip(index, message); });
        iconTrigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { HideTooltip(index); });
        iconTrigger.triggers.Add(entryExit);
    }

    public void ShowTooltip(int index, string message)
    {
        tooltipTexts[index].text = message;
        tooltips[index].SetActive(true);
    }

    public void HideTooltip(int index)
    {
        tooltips[index].SetActive(false);
    }

    void UpdateUI()
    {
        // 텍스트 업데이트
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        manaText.text = $"{playerStats.currentMana}/{playerStats.maxMana}";
        healthText.text = $"{playerStats.currentHealth}/{playerStats.maxHealth}";
        HpBarSlider.value = (float)playerStats.currentHealth / playerStats.maxHealth;

        if (playerStats.shield > 0)
        {
            shieldText.text = $"{playerStats.shield}";
            shieldText.gameObject.SetActive(true);
            shieldIcon.gameObject.SetActive(true);
        }
        else
        {
            shieldText.gameObject.SetActive(false);
            shieldIcon.gameObject.SetActive(false);
        }

        UpdateBuffIcons(playerStats);
    }

    void UpdateBuffIcons(PlayerStats playerStats)
    {
        // 업데이트할 각 버프의 상태를 playerStats에 맞춰 업데이트
        UpdateBuffIcon(playerStats.weaken, weakenIcon, weakenText);
        UpdateBuffIcon(playerStats.corrosion, corrosionIcon, corrosionText);
        UpdateBuffIcon(playerStats.vulnerable, vulnerableIcon, vulnerableText);
        UpdateBuffIcon(playerStats.power, powerIcon, powerText);
        UpdateBuffIcon(playerStats.burn, burnIcon, burnText);
        UpdateBuffIcon(playerStats.shieldcounter, shieldCounterIcon, shieldCounterText);
        UpdateBuffIcon(playerStats.cardarmor, cardArmorIcon, cardArmorText);
        UpdateBuffIcon(playerStats.combattraining, combatTrainingIcon, combatTrainingText);
        UpdateBuffIcon(playerStats.powercounter, powerCounterIcon, powerCounterText);
        UpdateBuffIcon(playerStats.thorn, thornIcon, thornText);
        UpdateBuffIcon(playerStats.shieldmanager, shieldManagerIcon, shieldManagerText);
        UpdateBuffIcon(playerStats.extramana, extraManaIcon, extraManaText);
        UpdateBuffIcon(playerStats.extrashield, extraShieldIcon, extraShieldText);
        UpdateBuffIcon(playerStats.attackcount, attackCountIcon, attackCountText);
        UpdateBuffIcon(playerStats.mdraw, mDrawIcon, mDrawText);
    }

    void UpdateBuffIcon(int stat, GameObject icon, TextMeshProUGUI text)
    {
        if (stat > 0)
        {
            icon.SetActive(true);
            text.text = stat.ToString();
        }
        else
        {
            icon.SetActive(false);
        }
    }
}
