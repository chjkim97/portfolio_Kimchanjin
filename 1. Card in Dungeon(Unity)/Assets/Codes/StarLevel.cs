using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StarLevel : MonoBehaviour
{
    // 현재 별 레벨
    public int currentStarLevel;
    // 해금된 최대 별 레벨
    public int possibleStarLevel;

    // 최대 별 레벨 20
    private const int maxStarLevel = 20;

    public Button starUpButton;
    public Button starDownButton;
    public TextMeshProUGUI starLevelText;
    void Awake()
{
    DontDestroyOnLoad(gameObject);
}
    void Start()
    {
        // 현재 별 레벨과 해금된 별 레벨 로드
        LoadStarLevels();

        starUpButton.onClick.AddListener(OnStarUpButtonClicked);
        starDownButton.onClick.AddListener(OnStarDownButtonClicked);

        UpdateUI();
    }
     // 별 레벨을 로드하는 메서드
    private void LoadStarLevels()
    {
        currentStarLevel = PlayerPrefs.GetInt("CurrentStarLevel", 0);
        possibleStarLevel = PlayerPrefs.GetInt("PossibleStarLevel", 0);
    }

    // 별 레벨을 저장하는 메서드
    private void SaveStarLevels()
    {
        PlayerPrefs.SetInt("CurrentStarLevel", currentStarLevel);
        PlayerPrefs.SetInt("PossibleStarLevel", possibleStarLevel);
        PlayerPrefs.Save();
    }
    // 최종 보스를 처치했을 때 호출
    public void OnFinalBossDefeated()
    {
        if (possibleStarLevel < maxStarLevel)
        {
            possibleStarLevel++;
            // 업데이트된 별 레벨을 PlayerPrefs에 저장
            PlayerPrefs.SetInt("PossibleStarLevel", possibleStarLevel);
            PlayerPrefs.Save();
        }
    }

        // currentStarLevel에 따라 난이도를 조정
        // 1 일반 적 최대 체력 +5
        // 2 엘리트 최대 체력 +10
        // 3 보스 최대 체력 +15
        // 4 일반 적 힘 +1
        // 5 엘리트 힘 +1
        // 6 보스 힘 +1
        // 7 현재 체력 5 감소한 상태로 시작
        // 8 전투 골드 보상 15 감소
        // 9 시작 골드 50 감소
        // 10 최대 체력 3 감소
        // 11 최대 체력 3 감소
        // 12 휴식 시 회복량 10 감소
        // 13 일반 적 최대 체력 +5
        // 14 엘리트 최대 체력 +10
        // 15 보스 최대 체력 + 15
        // 16 일반 적 힘 +1
        // 17 엘리트 힘 +1
        // 18 보스 힘 +1
        // 19 보스 힘 +1
        // 20 전투 시작 시 무작위 디버프

        

    // StarUp 버튼 클릭시 호출
    void OnStarUpButtonClicked()
    {
        if (currentStarLevel < possibleStarLevel)
        {
            currentStarLevel++;
            SaveStarLevels();
            UpdateUI();
        }
    }

    // StarDown 버튼 클릭시 호출
    void OnStarDownButtonClicked()
    {
        if (currentStarLevel > 0)
        {
            currentStarLevel--;
            SaveStarLevels();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        // 현재 별 레벨 UI 업데이트
        if (possibleStarLevel == 0)
        {
            starLevelText.text = "보스 처치시 해금";
        }
        else
        {
            starLevelText.text = new string('★', currentStarLevel);
        }
    }
}