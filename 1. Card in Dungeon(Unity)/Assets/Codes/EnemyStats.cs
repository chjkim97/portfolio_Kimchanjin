using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth = 50;
    public int shield = 0;
    public int EnemyDamage = 2;
    public int IsAlive = 1;
    public string enemyName;
    public int numberofhits = 1;
    public int behavior = 1; // 0:공격, 1:공격 이외 행동, 2:랜덤패턴, 3:복합
    public int weaken = 0;
    public int corrosion = 0;
    public int vulnerable = 0;
    public int power = 0;
    public int burn = 0;
    public int enemytype = 0;
    public Slider HpBarSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI behaviorText;
    public TextMeshProUGUI nameText;
    public GameObject[] tooltips; // 툴팁 패널 배열
    public TextMeshProUGUI[] tooltipTexts; // 툴팁 텍스트 배열
    
    // 버프 아이콘 및 텍스트
    public GameObject weakenIcon;
    public TextMeshProUGUI weakenText;
    public GameObject corrosionIcon;
    public TextMeshProUGUI corrosionText;
    public GameObject vulnerableIcon;
    public GameObject shieldIcon;
    public TextMeshProUGUI vulnerableText;
    public GameObject powerIcon;
    public TextMeshProUGUI powerText;
    public GameObject burnIcon;
    public TextMeshProUGUI burnText;

    void Start()
    {
        currentHealth = maxHealth;
        healthText = transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        shieldText = transform.Find("ShieldText").GetComponent<TextMeshProUGUI>();
        behaviorText = transform.Find("BehaviorText").GetComponent<TextMeshProUGUI>();
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        foreach (GameObject tooltip in tooltips)
        {
            tooltip.SetActive(false);
        }
        SetUpTooltip(weakenIcon, 0, "약화 : 공격으로 입히는 피해가 25%감소합니다");
        SetUpTooltip(corrosionIcon, 1, "부식 : 얻는 실드가 25% 감소합니다");
        SetUpTooltip(vulnerableIcon, 2, "취약 : 받는 피해가 50% 증가합니다");
        SetUpTooltip(powerIcon, 3, "공격력 : 공격할 때 공격력만큼 피해를 더 입힙니다");
        SetUpTooltip(burnIcon, 4, "화상 : 턴이 끝날 때 화상 수치만큼 데미지를 입습니다");
        InvokeRepeating("MyFunction", 0, 0.2f);
        
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
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        healthText.text = $"{currentHealth}/{maxHealth}";
        HpBarSlider.value = (float)currentHealth/maxHealth;

        if (shield > 0)
        {
            shieldText.text = $"{shield}";
            shieldText.gameObject.SetActive(true);
            shieldIcon.gameObject.SetActive(true);
        }
        else
        {
            shieldText.gameObject.SetActive(false);
            shieldIcon.gameObject.SetActive(false);

        }

        if (behavior == 0 || behavior == 3)
        {
            string behaviorString = "";
            if (playerStats.vulnerable > 0)
            {
                if (playerStats.enemynumber == 3)
                {
                    behaviorString = $"{(int)((EnemyDamage + power) * 1.5)} * 2";
                }
                else
                {
                    behaviorString = $"{(int)((EnemyDamage + power) * 1.5)}";
                }
            }
            else
            {
                if (playerStats.enemynumber == 3)
                {
                    behaviorString = $"{EnemyDamage + power} * 2";
                }
                else
                {
                    behaviorString = $"{EnemyDamage + power}";
                }
            }

            if (numberofhits >= 2)
            {
                behaviorString += $" * {numberofhits}";
            }
            else if (numberofhits == 0)
            {
                behaviorString += $" * ?";
            }
            if (behavior == 3)
            {
                behaviorString += " 특수행동";
            }
            behaviorText.text = behaviorString;
        }
        else if (behavior == 1)
        {
            behaviorText.text = "특수행동";
        }
        else if (behavior == 2)
        {
            behaviorText.text = "???";
        }

        nameText.text = enemyName;
        UpdateBuffIcons();
    }

    void UpdateBuffIcons()
    {
        // 약화
        if (weaken > 0)
        {
            weakenIcon.SetActive(true);
            weakenText.text = weaken.ToString();
        }
        else
        {
            weakenIcon.SetActive(false);
        }

        // 부식
        if (corrosion > 0)
        {
            corrosionIcon.SetActive(true);
            corrosionText.text = corrosion.ToString();
        }
        else
        {
            corrosionIcon.SetActive(false);
        }

        // 취약
        if (vulnerable > 0)
        {
            vulnerableIcon.SetActive(true);
            vulnerableText.text = vulnerable.ToString();
        }
        else
        {
            vulnerableIcon.SetActive(false);
        }

        // 힘
        if (power != 0)
        {
            powerIcon.SetActive(true);
            powerText.text = power.ToString();
        }
        else
        {
            powerIcon.SetActive(false);
        }

        // 화상
        if (burn > 0)
        {
            burnIcon.SetActive(true);
            burnText.text = burn.ToString();
        }
        else
        {
            burnIcon.SetActive(false);
        }
    }

    void MyFunction()
    {
        UpdateUI();
        Die();
    }

    public string GetEnemyName()
    {
        return enemyName;
    }

    public void TakeDamage(int damage)
    {
        if (shield > 0)
        {
            int remainingShield = Mathf.Max(0, shield - damage);
            damage -= shield - remainingShield;
            shield = remainingShield;
        }
        currentHealth -= damage;
        UpdateUI();
    }

    void Die()
    {
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            IsAlive = 0;
        }
    }
}
