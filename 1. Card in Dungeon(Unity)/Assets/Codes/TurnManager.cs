using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }
    public int randombehavior;
    public TurnState currentTurnState;
    public Button endTurnButton; // 턴 종료 버튼
    public int turncounter;
    public int NumberOfCard = 0; //이번 턴에 플레이어가 낸 카드의 수
    bool reliceffect = true; //유물 효과 발동 관리
    bool firstturnrelic = true;
    public int receiveddamage = 0; //enemy 12
    public int currentturnHealth = 0; //enemy 12
    private int currentStarLevel; //현재 별 레벨
    public GameObject dmgEffectPrefab;
    public int calculatedDamage = 0;
    public int dmgcounter = 0;
    public GameObject object1; // 첫 번째 게임 오브젝트
    public GameObject object2; // 두 번째 게임 오브젝트

    public void ToggleObject(GameObject targetObject)
    {
        StartCoroutine(ToggleCoroutine(targetObject));
    }

    private IEnumerator ToggleCoroutine(GameObject targetObject)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            targetObject.SetActive(false);
        }
    }
    void Start()
    {
        randombehavior = Random.Range(0, 2);
        currentTurnState = TurnState.PlayerTurn; // 게임 시작 시 플레이어의 턴으로 시작
        endTurnButton.onClick.AddListener(EndTurn);
        turncounter = 1;
        currentStarLevel = PlayerPrefs.GetInt("CurrentStarLevel", 0);
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (currentStarLevel >= 20)
        {
            int randebuff = Random.Range(0, 4);
            if (randebuff == 0)
            {
                playerStats.weaken += 2;
            }
            else if (randebuff == 1)
            {
                playerStats.corrosion += 2;
            }
            else if (randebuff == 2)
            {
                playerStats.vulnerable += 2;
            }
            else
            {
                playerStats.burn += 3;
            }
        }

        int powerLevel = PlayerPrefs.GetInt("powerLevel", 0);
        float[] powerIncreaseChances = { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < powerIncreaseChances[powerLevel])
        {
            playerStats.power++;
        }

    }

    void Update()
    {
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        CardManager cardManager = FindObjectOfType<CardManager>();
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        Card card = FindObjectOfType<Card>();
        CardList cardList = FindObjectOfType<CardList>();
        StarLevel starlevel = FindObjectOfType<StarLevel>();

        if (enemyManager.enemies.Count == 1)
        {
            EnemyStats enemy = enemyManager.enemies[0];
            if (enemy.IsAlive == 0)
            {

                if (playerStats.enemytype == 0)
                {
                    GameData.Instance.AddDiamonds(20);
                }
                else if (playerStats.enemytype == 1)
                {
                    GameData.Instance.AddDiamonds(40);
                }
                else if (playerStats.enemytype == 2)
                {
                    GameData.Instance.AddDiamonds(100);
                }
                ClearPlayerStats();
                if (relicManager.playerRelics.Exists(relic => relic.number == 34))
                {
                    relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
                }
                if (relicManager.playerRelics.Exists(relic => relic.number == 39))
                {
                    playerStats.gold += playerStats.bankgold;
                }
                if (playerStats.enemytype == 0)
                    playerStats.gold += Random.Range(50, 100);
                else if (playerStats.enemytype == 1)
                    playerStats.gold += Random.Range(100, 150);
                else if (playerStats.enemytype == 2)
                    playerStats.gold += Random.Range(150, 200);

                if (currentStarLevel >= 8)
                    playerStats.gold -= 15;

                if (playerStats.enemytype == 2)
                {
                    starlevel.OnFinalBossDefeated();
                    SceneManager.LoadScene("Win");
                    
                }
                else
                    SceneManager.LoadScene("BattleReward");

            }
        }
        else
        {
            EnemyStats enemy = enemyManager.enemies[0];
            EnemyStats enemy1 = enemyManager.enemies[1];
            if (enemy.IsAlive == 0 && enemy1.IsAlive == 0)
            {
                ClearPlayerStats();
                if (relicManager.playerRelics.Exists(relic => relic.number == 34))
                {
                    relicManager.AddRelicToPlayer(relicManager.ChoiceRanRelic());
                }
                if (relicManager.playerRelics.Exists(relic => relic.number == 39))
                {
                    playerStats.gold += playerStats.bankgold;
                }
                if (playerStats.enemytype == 0)
                    playerStats.gold += Random.Range(50, 100);
                else if (playerStats.enemytype == 1)
                    playerStats.gold += Random.Range(100, 150);
                else if (playerStats.enemytype == 2)
                    playerStats.gold += Random.Range(150, 200);

                if (currentStarLevel >= 8)
                    playerStats.gold -= 15;
                if (playerStats.enemytype == 2)
                {
                    starlevel.OnFinalBossDefeated();
                    SceneManager.LoadScene("Win");
                    Debug.Log("Win");
                    
                }
                else
                {
                    SceneManager.LoadScene("BattleReward");
                }

            }

        }
        // 턴에 따른 동작 수행
        if (currentTurnState == TurnState.PlayerTurn)
        {
            // 플레이어의 턴 동작 처리
            endTurnButton.interactable = true;
            // 유물 발동 처리
            if (reliceffect == true)
            {

                relicManager.ActivatePlayerRelicEffects(1);

                reliceffect = false;
            }
            if (relicManager.playerRelics.Exists(relic => relic.number == 26) && firstturnrelic == true)
            {
                for (int i = 0; i < enemyManager.enemies.Count; i++)
                {
                    EnemyStats enemy = enemyManager.enemies[i];
                    enemy.currentHealth -= 5;
                }
                firstturnrelic = false;
            }

        }
        else if (currentTurnState == TurnState.EnemyTurn)
        {
            ToggleObject(object2);
            relicManager.ActivatePlayerRelicEffects(2);
            // 적의 턴 동작 처리
            if (enemyManager.enemies.Count == 0)
            {
                Debug.LogWarning("No enemies in the list.");
            }

            playerStats.shield += playerStats.Defensemagic * 3;

            for (int i = 0; i < enemyManager.enemies.Count; i++)
            {
                EnemyStats enemy = enemyManager.enemies[i];
                enemy.burn += playerStats.firesea * 3; //시전한 불의바다의 수*3 만큼 화상 부여
                enemy.currentHealth -= enemy.burn;
                Debug.Log("Enemy's turn");
                // 각 적에 대한 동작 수행
                if (enemy.IsAlive == 1)
                {
                    switch (playerStats.enemynumber)
                    {
                        case 0:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;

                            }
                            else
                            {
                                if (i == 0)
                                    playerStats.weaken += 2;
                                else
                                    playerStats.vulnerable += 2;
                                enemy.behavior = 0;
                            }
                            break;
                        case 1:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            else
                            {
                                playerStats.burn += 3;
                                enemy.behavior = 0;
                            }
                            break;
                        case 2:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            else
                            {
                                enemy.shield += 10;
                                enemy.behavior = 0;
                            }
                            break;
                        case 3:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            else
                            {
                                enemy.power += 2;
                                enemy.behavior = 0;
                            }
                            break;
                        case 4:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            else
                            {
                                int d = Random.Range(0, 4);
                                enemy.behavior = 0;
                                switch (d)
                                {
                                    case 0:
                                        playerStats.weaken += 3;
                                        break;
                                    case 1:
                                        playerStats.vulnerable += 3;
                                        break;
                                    case 2:
                                        playerStats.corrosion += 3;
                                        break;
                                    case 3:
                                        playerStats.burn += 3;
                                        break;
                                    case 4:
                                        enemy.power += 5;
                                        break;
                                }
                            }
                            break;
                        case 5:
                            if (turncounter % 3 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            else if (turncounter % 3 == 1)
                            {
                                //플레이어 덱에 방해 카드 추가
                                //종류별로 하나씩 401~406
                                cardManager.AddDisturbCard(401);
                                cardManager.AddDisturbCard(402);
                                cardManager.AddDisturbCard(403);
                                cardManager.AddDisturbCard(404);
                                cardManager.AddDisturbCard(405);
                                cardManager.AddDisturbCard(406);

                                enemy.behavior = 1;
                            }
                            else
                            {
                                playerStats.vulnerable += 2;
                                playerStats.corrosion += 2;
                                playerStats.weaken += 2;
                                enemy.power += 3;
                                enemy.behavior = 0;
                            }
                            break;
                        case 6:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            if (turncounter % 2 == 1)
                            {
                                playerStats.weaken += Random.Range(0, 3);
                                playerStats.vulnerable += Random.Range(0, 3);
                                playerStats.corrosion += Random.Range(0, 3);
                            }
                            break;
                        case 7:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            if (turncounter % 2 == 1)
                            {
                                //방해카드 404추가
                                cardManager.AddDisturbCard(404);
                            }
                            break;
                        case 8:
                            if (turncounter % 2 == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.behavior = 1;
                            }
                            if (turncounter % 2 == 1)
                            {
                                enemy.power += 5;
                                enemy.behavior = 0;
                            }
                            break;
                        case 9:
                            EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                            enemy.shield -= playerStats.thorn;
                            if (enemy.shield <= 0)
                            {
                                enemy.currentHealth += enemy.shield;
                                enemy.shield = 0;
                            }
                            //방해카드 405추가
                            cardManager.AddDisturbCard(405);
                            break;
                        case 10:
                            EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                            enemy.shield -= playerStats.thorn;
                            if (enemy.shield <= 0)
                            {
                                enemy.currentHealth += enemy.shield;
                                enemy.shield = 0;
                            }
                            if (turncounter % 3 == 1)
                                playerStats.vulnerable += 2;
                            else if (turncounter % 3 == 2)
                                playerStats.corrosion += 2;
                            else
                                playerStats.weaken += 2;
                            break;
                        case 11:
                            // 체력 100
                            // 1턴 방해카드 5장 덱에 넣음
                            // 2턴 상태이상 부여 3턴지속 실드+30(플레이어 기준 3턴에 적 실드 유지)
                            // 3턴 3 * 5딜
                            // 4턴 힘+3, 체력 20회복
                            // 5턴부터 2~4턴 행동 반복
                            if (turncounter == 1)
                            {//방해카드 5장
                             //402~406
                                cardManager.AddDisturbCard(402);
                                cardManager.AddDisturbCard(403);
                                cardManager.AddDisturbCard(404);
                                cardManager.AddDisturbCard(405);
                                cardManager.AddDisturbCard(406);

                                enemy.behavior = 1;
                            }
                            else if (turncounter % 3 == 2)
                            {
                                playerStats.weaken += 3;
                                playerStats.corrosion += 3;
                                playerStats.vulnerable += 3;
                                enemy.shield += 30;
                                enemy.behavior = 0;

                            }
                            else if (turncounter % 3 == 0)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }
                                    enemy.behavior = 1;
                                }


                            }
                            else if (turncounter % 3 == 1)
                            {
                                enemy.power += 3;
                                enemy.currentHealth += 20;
                                if (enemy.currentHealth >= enemy.maxHealth)
                                    enemy.currentHealth = enemy.maxHealth;
                                enemy.behavior = 1;

                            }
                            break;

                        case 12:
                            if (turncounter == 1)
                            {
                                receiveddamage = enemy.maxHealth - enemy.currentHealth;
                            }
                            else
                            {
                                receiveddamage = currentturnHealth - enemy.currentHealth;
                            }
                            EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                            enemy.shield -= playerStats.thorn;
                            if (enemy.shield <= 0)
                            {
                                enemy.currentHealth += enemy.shield;
                                enemy.shield = 0;
                            }
                            enemy.power += 2;

                            if (receiveddamage % 2 == 0)
                            {
                                if (receiveddamage >= 0)
                                    enemy.shield += receiveddamage;
                            }
                            else
                            {
                                if (receiveddamage >= 0)
                                    enemy.currentHealth += receiveddamage / 2;
                            }
                            receiveddamage = 0;
                            currentturnHealth = enemy.currentHealth;
                            break;
                        case 13:
                            playerStats.burn += turncounter;
                            break;
                        case 14:
                            if (turncounter == 1)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.shield += 15;
                                enemy.numberofhits = 0;
                            }
                            else if (turncounter % 3 == 2)
                            {
                                randombehavior = Random.Range(0, 2);
                                if (randombehavior == 1)
                                {
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }
                                }
                                else
                                {
                                    cardManager.throwcard += 2;
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }

                                }


                            }
                            else if (turncounter % 3 == 0)
                            {
                                randombehavior = Random.Range(0, 2);
                                if (randombehavior == 1)
                                {
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }
                                    enemy.shield += 10;
                                    enemy.behavior = 1;
                                }
                                else
                                {
                                    EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                    enemy.shield -= playerStats.thorn;
                                    if (enemy.shield <= 0)
                                    {
                                        enemy.currentHealth += enemy.shield;
                                        enemy.shield = 0;
                                    }
                                    enemy.shield += 20;
                                    enemy.behavior = 1;

                                }
                            }
                            else if (turncounter % 3 == 1)
                            {
                                randombehavior = Random.Range(0, 2);
                                if (randombehavior == 1)
                                {
                                    //방해카드401추가
                                    cardManager.AddDisturbCard(401);
                                }
                                else
                                {
                                    playerStats.weaken += 3;
                                    playerStats.corrosion += 3;
                                    playerStats.vulnerable += 3;
                                }
                            }

                            break;
                        case 15:
                            randombehavior = Random.Range(0, 4);
                            if (randombehavior == 0)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                            }
                            else if (randombehavior == 1)
                            {
                                EnemyAttack(enemy.EnemyDamage, enemy.power, i);
                                enemy.shield -= playerStats.thorn;
                                if (enemy.shield <= 0)
                                {
                                    enemy.currentHealth += enemy.shield;
                                    enemy.shield = 0;
                                }
                                enemy.shield += 10;
                            }
                            else if (randombehavior == 2)
                            {
                                enemy.shield += 10;
                                //방해카드401
                                cardManager.AddDisturbCard(401);
                            }
                            else if (randombehavior == 3)
                            {
                                enemy.currentHealth += 10;
                                if (enemy.currentHealth > enemy.maxHealth)
                                    enemy.currentHealth = enemy.maxHealth;
                                playerStats.weaken += 3;
                                playerStats.corrosion += 3;
                                playerStats.vulnerable += 3;
                            }
                            break;
                    }

                }
            }

            // 예: 턴 종료 버튼 비활성화
            //endTurnButton.interactable = false;

            if (playerStats.shieldmanager == 1)
            {

            }
            else if (playerStats.barrier == 1)
            {
                playerStats.shield -= 20;
                if (playerStats.shield < 0)
                    playerStats.shield = 0;
            }
            else if (playerStats.shieldmanager == 0)
            {
                playerStats.shield = 0;
            }
            NumberOfCard = 0; //턴이 끝날 때 초기화

            // 적의 턴이 끝나면 다시 플레이어의 턴으로 전환
            if (playerStats.burn > 0)
            {
                playerStats.currentHealth -= playerStats.burn;
                playerStats.burn--;
            }
            if (playerStats.weaken > 0)
                playerStats.weaken--;
            if (playerStats.corrosion > 0)
                playerStats.corrosion--;
            if (playerStats.vulnerable > 0)
                playerStats.vulnerable--;
            for (int i = 0; i < enemyManager.enemies.Count; i++)
            {
                EnemyStats enemy = enemyManager.enemies[i];
                if (enemy.burn > 0)
                {
                    enemy.burn--;
                }
                if (enemy.weaken > 0)
                    enemy.weaken--;
                if (enemy.corrosion > 0)
                    enemy.corrosion--;
                if (enemy.vulnerable > 0)
                    enemy.vulnerable--;
                enemy.shield = 0;
            }
            playerStats.shieldcounter = 0; //shieldcounter 1당 카드를 사용할 때마다 실드 +4
            playerStats.power += playerStats.powercounter * 3; //턴이 끝날 때마다 powercounter*3만큼 힘 증가

            playerStats.power += playerStats.combattraining * 4; //사용한 전투 훈련 1장당 힘+4
            playerStats.combattraining = 0;
            SwitchTurn(TurnState.PlayerTurn);

            DelayManager delayManager = FindObjectOfType<DelayManager>();
            delayManager.DelayAction(() =>
            {
                Debug.Log("0.5초 후에 실행되었습니다.");
                ToggleObject(object1);
            });
            cardManager.usecardcounter = 0;
            if (cardManager.PlayerCards != null)
            {
                for (int i = 0; i < 10; i++) //최대 핸드 수 10장
                {
                    Item usedItem = cardManager.UseItem();
                    if (usedItem != null)
                    {
                        print("Used item: " + usedItem.name);
                        cardManager.UseCard(usedItem);
                    }
                }
            }
            playerStats.currentMana = playerStats.maxMana;
            cardManager.usecardcounter = 1;
            playerStats.currentMana += playerStats.extramana;
            playerStats.extramana = 0;
            playerStats.shield += playerStats.extrashield;
            playerStats.extrashield = 0;

            if (relicManager.playerRelics.Exists(relic => relic.number == 32))
            {
                cardManager.AddCard(6);
            }
            else
                cardManager.AddCard(5);
            reliceffect = true;
            turncounter++;
        }
    }


    void EndTurn()
    {
        // 중복 클릭 방지를 위해 버튼이 활성화된 상태에서만 실행
        if (!endTurnButton.interactable)
            return;

        // 턴 종료 버튼을 즉시 비활성화하여 중복 클릭 방지
        endTurnButton.interactable = false;

        SoundManager soundManager = FindObjectOfType<SoundManager>();

        // 턴 종료 버튼을 누르면 턴을 전환
        if (currentTurnState == TurnState.PlayerTurn)
        {
            // 플레이어의 턴이 끝나면 적의 턴으로 전환
            SwitchTurn(TurnState.EnemyTurn);
            Debug.Log("End turn");

            if (soundManager != null)
            {
                soundManager.PlaySound(12);
            }
        }
    }

    void SwitchTurn(TurnState nextTurn)
    {
        currentTurnState = nextTurn;

        // 플레이어 턴으로 전환되면 버튼을 활성화
        if (currentTurnState == TurnState.PlayerTurn)
        {
            endTurnButton.interactable = true;
        }

        // 턴 전환 시 필요한 초기화 및 동작
    }

    void ClearPlayerStats() //전투가 끝날 때 버프,디버프 초기화
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.maxMana = 3;
        playerStats.currentMana = 3;
        playerStats.shield = 0;
        playerStats.shieldcounter = 0;
        playerStats.cardarmor = playerStats.basiccardarmor;
        playerStats.power = 0;
        playerStats.powercounter = 0;
        playerStats.thorn = 0;
        playerStats.shieldmanager = 0;
        playerStats.extramana = 0;
        playerStats.extrashield = 0;
        playerStats.firesea = 0;
        playerStats.weaken = 0;
        playerStats.corrosion = 0;
        playerStats.vulnerable = 0;
        playerStats.nodraw = 0;
        playerStats.burn = 0;
        playerStats.mdraw = 0;
        playerStats.attackcount = 0;
        reliceffect = true;
        playerStats.combattraining = 0;
        playerStats.Defensemagic = 0;
    }

    void EnemyAttack(int damage, int enemyPower, int enemynum)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        EnemyStats enemy = enemyManager.enemies[enemynum];

        // 적 오브젝트의 RectTransform을 가져옴
        RectTransform enemyTransform = enemy.GetComponent<RectTransform>();

        // 적의 원래 위치를 저장
        Vector3 originalPosition = enemyTransform.anchoredPosition;

        // 적의 X좌표를 -200만큼 서서히 이동시킴
        StartCoroutine(MoveEnemyToPosition(enemyTransform, new Vector3(originalPosition.x - 200f, originalPosition.y, originalPosition.z), 0.5f));

        if (enemy.weaken > 0) // 적에게 약화가 있을 경우 50% 데미지
        {
            if (playerStats.vulnerable > 0)
                calculatedDamage = (int)((damage + enemyPower) * 1.5f / 2);
            else
                calculatedDamage = (damage + enemyPower) / 2;
        }
        else
        {
            if (playerStats.vulnerable > 0)
                calculatedDamage = (int)((damage + enemyPower) * 1.5f);
            else
                calculatedDamage = damage + enemyPower;
        }
        playerStats.shield -= calculatedDamage;
        if (playerStats.shield < 0)
        {
            playerStats.currentHealth += playerStats.shield;
            playerStats.shield = 0;
        }
        // DmgEffect를 생성
        GameObject dmgEffect;
        if (dmgcounter == 0)
        {
            dmgEffect = Instantiate(dmgEffectPrefab, new Vector3(629f, 826f, 0f), Quaternion.identity);
            dmgcounter = 1;
        }
        else
        {
            dmgEffect = Instantiate(dmgEffectPrefab, new Vector3(800, 826f, 0f), Quaternion.identity);
            dmgcounter = 0;
        }

        // 1초 뒤 제거
        if (dmgEffect != null)
        {
            Destroy(dmgEffect, 1f);
        }
    }

    // 적의 위치를 서서히 이동시키는 코루틴 함수
    IEnumerator MoveEnemyToPosition(RectTransform enemyTransform, Vector3 targetPosition, float duration)
    {
        Vector3 originalPosition = enemyTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 현재 위치를 보간하여 업데이트
            enemyTransform.anchoredPosition = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime; // 시간 증가
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치를 정확히 설정
        enemyTransform.anchoredPosition = targetPosition;

        // 다시 원래 자리로 돌아가는 시간 설정
        yield return new WaitForSeconds(0.5f); // 원래 자리로 돌아가기 전에 대기

        // 원래 위치로 돌아오는 과정
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            enemyTransform.anchoredPosition = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime; // 시간 증가
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치를 정확히 설정
        enemyTransform.anchoredPosition = originalPosition;
    }


}

