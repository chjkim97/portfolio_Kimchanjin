using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<EnemyStats> enemies = new List<EnemyStats>();
    public Canvas canvas;
    private int currentStarLevel; //현재 별 레벨

    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        int randompower = 1;
        //일반몹 이면 enemytype=0, 엘리트 1, 보스 2
        //지도에서 선택한 아이콘에 따라 그 enemytype에서 적 랜덤 선택
        //패키징 받은 후 변경 예정
        currentStarLevel = PlayerPrefs.GetInt("CurrentStarLevel", 0); //현재 별 레벨 가져오기

        Dictionary<int, int> enemyTypeMapping = new Dictionary<int, int>
        {
            { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 1 }, { 4, 1 },
            { 5, 2 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 0 },
            { 10, 0 },{ 11, 2 },{ 12, 2 },{ 13, 2 },{ 14, 2 },{ 15, 2 }
        };//enemynumber와 enemytype을 미리 설정
        //적 추가시 {enemynumber,enemytype}추가

        List<int> possibleEnemies = enemyTypeMapping
            .Where(pair => pair.Value == playerStats.enemytype)
            .Select(pair => pair.Key)
            .ToList();
        playerStats.enemynumber = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
        Debug.Log("Possible Enemies: " + string.Join(", ", possibleEnemies));
        //playerStats.enemynumber=3; // 임의의 적 코드 확인용
        playerStats.enemytype = enemyTypeMapping[playerStats.enemynumber];
        Vector3[] spawnPositions = null;

        if (playerStats.enemynumber <= 1)
        {
            spawnPositions = new Vector3[] {
                new Vector3(250f, 103f, 0f),
                new Vector3(650f, 103f, 0f)
            };
        }
        else if (playerStats.enemynumber >= 2)
        {
            spawnPositions = new Vector3[] {
                new Vector3(175f, 103f, 0f)
            };
        }

        if (relicManager.playerRelics.Exists(relic => relic.number == 30))
        {
            randompower = Random.Range(0, 4);
            if (randompower != 0)
            {
                playerStats.power += 2;
            }
        }

        if (spawnPositions != null)
        {
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                // enemyPrefab을 이용하여 프리팹을 인스턴스화
                GameObject enemyObject = Instantiate(enemyPrefab, canvas.transform);

                // 지정된 위치로 적을 배치
                RectTransform enemyTransform = enemyObject.GetComponent<RectTransform>();
                enemyTransform.anchoredPosition = spawnPositions[i];

                // if (playerStats.enemynumber >= 2)
                // enemyTransform.localScale = new Vector3(4f, 4f, 4f);

                // 자식 오브젝트인 EnemyArea 찾기
                Transform enemyArea = enemyTransform.Find("EnemyArea");
                if (enemyArea != null)
                {
                    // 두 번째 적인 경우, 자식 오브젝트의 레이어를 EnemyArea2로 설정
                    if (i == 1 && spawnPositions.Length > 1)
                    {
                        enemyArea.gameObject.layer = LayerMask.NameToLayer("EnemyArea2");
                    }
                    else
                    {
                        enemyArea.gameObject.layer = LayerMask.NameToLayer("EnemyArea");
                    }
                }

                EnemyStats enemyStats = enemyObject.GetComponent<EnemyStats>();

                // 임시 이름 설정
                enemyStats.enemyName = "Enemy" + (playerStats.enemynumber * 2 + i + 1);

                // 적의 스탯 설정
                switch (playerStats.enemynumber)
                {
                    case 0:
                        enemyStats.maxHealth = Random.Range(10, 31);
                        enemyStats.EnemyDamage = Random.Range(4, 11);
                        enemyStats.enemyName ="슬라임";
                        break;
                    case 1:
                        enemyStats.maxHealth = 20;
                        enemyStats.EnemyDamage = 7;
                        enemyStats.enemyName ="고블린";
                        break;
                    case 2:
                        enemyStats.maxHealth = Random.Range(30, 35);
                        enemyStats.EnemyDamage = Random.Range(8, 15);
                        enemyStats.enemyName ="트롤";
                        break;
                    case 3:
                        enemyStats.maxHealth = 80;
                        enemyStats.EnemyDamage = Random.Range(1, 3);
                        enemyStats.enemyName ="호박기사";
                        break;
                    case 4:
                        enemyStats.maxHealth = Random.Range(50, 71);
                        enemyStats.EnemyDamage = Random.Range(6, 8);
                        enemyStats.enemyName ="철갑기사";
                        break;
                    case 5:
                        enemyStats.maxHealth = 200;
                        enemyStats.EnemyDamage = 10;
                        enemyStats.enemyName ="융합체X-12";
                        break;
                    case 6:
                        enemyStats.maxHealth = Random.Range(40, 51);
                        enemyStats.EnemyDamage = Random.Range(8, 11);
                        enemyStats.enemyName ="자동파괴로봇";
                        break;
                    case 7:
                        enemyStats.maxHealth = Random.Range(50, 71);
                        enemyStats.EnemyDamage = 10;
                        enemyStats.enemyName ="드래곤";
                        break;
                    case 8:
                        enemyStats.maxHealth = Random.Range(50, 71);
                        enemyStats.EnemyDamage = 5;
                        enemyStats.enemyName ="삐에로킹";
                        //공격, 힘+5 반복
                        break;
                    case 9:
                        enemyStats.maxHealth = Random.Range(30, 51);
                        enemyStats.EnemyDamage = Random.Range(4, 11);
                        enemyStats.behavior=3;
                        enemyStats.enemyName ="성난황소";
                        //공격+방해카드1장추가
                        break;
                    case 10:
                        enemyStats.maxHealth = 25;
                        enemyStats.EnemyDamage = 10;
                        //상태이상 + 공격 패턴
                        enemyStats.behavior=3;
                        enemyStats.enemyName ="구울";
                        break;
                    case 11:
                        enemyStats.maxHealth = 100;
                        enemyStats.EnemyDamage = 3;
                        enemyStats.numberofhits=5;
                        enemyStats.behavior=1;

                        enemyStats.enemyName ="융합체T-17";
                        break;
                    case 12:
                        enemyStats.maxHealth = 100;
                        enemyStats.EnemyDamage = 2;
                        enemyStats.enemyName ="융합체M-25";
                        break;
                    case 13:
                        enemyStats.maxHealth = 100;
                        enemyStats.EnemyDamage = 0;
                        enemyStats.behavior=1;
                        enemyStats.enemyName ="다크위치";
                    break;
                    case 14:
                        enemyStats.maxHealth = Random.Range(100, 121);
                        enemyStats.enemyName ="대마법사";
                    break;
                    case 15:
                        enemyStats.maxHealth = 100;
                        enemyStats.EnemyDamage = 10;
                        enemyStats.behavior=2;
                        enemyStats.enemyName ="드워프기사";

                    break;

                }
                if (playerStats.enemytype == 3 && relicManager.playerRelics.Exists(relic => relic.number == 27))
                    playerStats.power += 3;

                if (playerStats.enemytype == 2 && relicManager.playerRelics.Exists(relic => relic.number == 28))
                    playerStats.power += 2;

                //체력 -5 유물
                if (playerStats.firststrike == 1)
                    enemyStats.currentHealth -= 5;

                enemyStats.currentHealth = enemyStats.maxHealth;
                enemyStats.power = 0;
                if (randompower == 0)
                    enemyStats.power += 1;
                //모든 캐릭터 힘+3 유물
                if (relicManager.playerRelics.Exists(relic => relic.number == 31))
                {
                    enemyStats.power += 3;
                    playerStats.power += 3;
                }

                //별 레벨에 따라 적 스텟 조정
                if(currentStarLevel>=1&&playerStats.enemytype==0)
                {
                    enemyStats.maxHealth+=5;
                    enemyStats.currentHealth+=5;
                }
                if(currentStarLevel>=2&&playerStats.enemytype==1)
                {
                    enemyStats.maxHealth+=10;
                    enemyStats.currentHealth+=10;
                }
                if(currentStarLevel>=3&&playerStats.enemytype==2)
                {
                    enemyStats.maxHealth+=15;
                    enemyStats.currentHealth+=15;
                }
                if(currentStarLevel>=4&&playerStats.enemytype==0)
                {
                    enemyStats.power+=1;
                }
                if(currentStarLevel>=5&&playerStats.enemytype==1)
                {
                    enemyStats.power+=1;
                }
                if(currentStarLevel>=6&&playerStats.enemytype==2)
                {
                    enemyStats.power+=1;
                }
                if(currentStarLevel>=13&&playerStats.enemytype==0)
                {
                    enemyStats.maxHealth+=5;
                    enemyStats.currentHealth+=5;
                }
                if(currentStarLevel>=14&&playerStats.enemytype==1)
                {
                    enemyStats.maxHealth+=10;
                    enemyStats.currentHealth+=10;
                }
                if(currentStarLevel>=15&&playerStats.enemytype==2)
                {
                    enemyStats.maxHealth+=15;
                    enemyStats.currentHealth+=15;
                }
                if(currentStarLevel>=16&&playerStats.enemytype==0)
                {
                    enemyStats.power+=1;
                }
                if(currentStarLevel>=17&&playerStats.enemytype==1)
                {
                    enemyStats.power+=1;
                }
                if(currentStarLevel>=18&&playerStats.enemytype==2)
                {
                    enemyStats.power+=1;
                }
                // 적 추가
                enemies.Add(enemyStats);
            }
        }
    }



}
