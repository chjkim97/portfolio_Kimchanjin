using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RelicManager : MonoBehaviour
{
    public List<Relic> allRelics; // 전체 유물 리스트
    public List<Relic> playerRelics; // 플레이어가 보유한 유물 리스트
    public event System.Action OnRelicAdded;
    private static RelicManager _instance;
    int Tpower = 0;
    public static RelicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RelicManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("RelicManager");
                    _instance = go.AddComponent<RelicManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        allRelics = new List<Relic>();

        allRelics.Add(new Relic("치유의 부적", "전투 시작 시 체력을 5 회복합니다.", 1));
        allRelics.Add(new Relic("행운의 목걸이", "첫 턴에만 카드를 2장 더 뽑습니다.", 2));
        allRelics.Add(new Relic("마나 증폭기", "첫 턴에만 마나를 1 추가로 얻습니다.", 3));
        allRelics.Add(new Relic("활력의 부적", "최대 체력을 +8 얻습니다.", 4));
        allRelics.Add(new Relic("황금 동전", "획득 시 골드를 150 얻습니다.", 5));
        allRelics.Add(new Relic("약화의 토템", "전투 시작 시 적 전체에게 약화를 1 부여합니다.", 6));
        allRelics.Add(new Relic("부식의 토템", "전투 시작 시 적 전체에게 부식을 1 부여합니다.", 7));
        allRelics.Add(new Relic("취약의 토템", "전투 시작 시 적 전체에게 취약을 1 부여합니다.", 8));
        allRelics.Add(new Relic("황금 부적", "전투 시작 시 골드를 10 얻습니다.", 9));
        allRelics.Add(new Relic("방패 마법진", "전투 시작 시 실드를 8 얻습니다.", 10));
        allRelics.Add(new Relic("힘의 돌", "전투 시작 시 힘을 1 얻습니다.", 11));
        allRelics.Add(new Relic("분노의 수정", "잃은 체력 15 당 힘을 1 얻습니다.", 12));
        allRelics.Add(new Relic("가시 팔찌", "전투 시작 시 가시를 3 얻습니다.", 13));
        allRelics.Add(new Relic("활력의 펜던트", "휴식 시 체력을 10 추가로 회복합니다.", 14));
        allRelics.Add(new Relic("상인의 부적", "상점 칸에 도착시, 체력을 10 회복합니다.", 15));
        allRelics.Add(new Relic("수호자의 방패", "턴 종료시 실드가 없으면 실드를 8 얻습니다.", 16));
        allRelics.Add(new Relic("전사의 부적", "공격 카드를 5번 사용할 때마다 힘을 1 얻습니다.", 17));
        allRelics.Add(new Relic("방패병의 갑옷", "2번째 턴 시작시, 실드를 15 얻습니다.", 18));
        allRelics.Add(new Relic("거대한 방패", "3번째 턴 시작시, 실드를 25 얻습니다.", 19));

        allRelics.Add(new Relic("루비 수정", "카드를 사용할 때마다 실드를 1 얻습니다.", 20));
        allRelics.Add(new Relic("방벽", "턴 시작 시 실드가 전부 사라지지 않고 20만큼만 잃습니다.", 21));
        allRelics.Add(new Relic("불의 구슬", "내 턴이 끝날 때, 모든 적에게 피해를 4 줍니다.", 22));
        allRelics.Add(new Relic("근력의 주사위", "전투 시작 시 힘을 0~2 얻습니다.", 23));
        allRelics.Add(new Relic("불사조의 깃털", "사망 시 50%의 체력으로 1회 부활합니다.", 24));
        allRelics.Add(new Relic("불타는 정수", "내 턴이 끝날 때 모든 적에게 화상을 3 부여합니다.", 25));
        allRelics.Add(new Relic("정찰병의 화살", "전투 시작 시 모든 적에게 피해를 5 줍니다.", 26));
        allRelics.Add(new Relic("마우스피스", "보스 전투 시, 힘을 3 얻고 시작합니다.", 27));
        allRelics.Add(new Relic("전투글러브", "엘리트 전투 시, 힘을 2 얻고 시작합니다.", 28));
        allRelics.Add(new Relic("구르는 돌", "일회용, 지속 카드 사용 시 카드를 한 장 뽑습니다.", 29));
        allRelics.Add(new Relic("악마의 주사위", "전투 시작 시, 75%확률로 힘을 2 얻습니다. 25%확률로 적이 힘을 1 얻습니다.", 30));
        allRelics.Add(new Relic("악마의 돌", "전투 시작 시, 모든 캐릭터가 힘을 3 얻습니다.", 31));
        allRelics.Add(new Relic("악마의 계약", "최대 체력이 20 감소합니다. 매 턴 카드를 1장 더 뽑습니다.", 32));
        allRelics.Add(new Relic("무한한 팽이", "손에 카드가 없다면, 카드를 1장 뽑습니다.", 33));
        allRelics.Add(new Relic("끝없는 욕망", "전투가 끝날 때마다 무작위 카드와 유물을 얻습니다.", 34));
        allRelics.Add(new Relic("딜러의 목걸이", "카드 보상 새로고침 비용이 50골드로 고정됩니다.", 35)); //50골드 시작, 50골드씩 비싸짐
        allRelics.Add(new Relic("탐험가의 모자", "유물 보상 새로고침 비용이 100골드로 고정됩니다.", 36)); //100골드 시작, 100골드씩 비싸짐
        allRelics.Add(new Relic("네잎클로버", "첫 카드 보상 새로고침 비용이 0골드가 됩니다.", 37));
        allRelics.Add(new Relic("사파이어 수정", "골드를 50 얻습니다. 최대 체력이 5 증가합니다.", 38));
        allRelics.Add(new Relic("멤버십 카드", $"골드를 모두 은행에 맡깁니다. 전투 1번당 20%의 이자를 받습니다. ({playerStats.bankgold})", 39));


    }


    // 지정 유물 획득
    public void AddRelicToPlayer(int relicNumber)
    {
        
        Relic foundRelic = allRelics.Find(relic => relic.number == relicNumber);
        if (foundRelic != null)
        {
            playerRelics.Add(foundRelic);
            OnRelicAdded?.Invoke();
        }
        Debug.Log("relicNumber: " + relicNumber);
        AcquiredRelicEffect(relicNumber);

        
    }
    // 랜덤 유물 번호 출력
    public int ChoiceRanRelic()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        // 플레이어가 이미 보유한 유물 번호 리스트 생성
        List<int> playerRelicNumbers = new List<int>();
        foreach (Relic relic in playerRelics)
        {
            playerRelicNumbers.Add(relic.number);
        }

        // 추가 가능한 유물 번호 리스트 생성
        List<int> availableRelicNumbers = new List<int>();
        for (int i = 4; i <= allRelics.Count; i++)
        {
            if (!playerRelicNumbers.Contains(i))
            {
                availableRelicNumbers.Add(i);
            }
        }

        // 추가 가능한 유물이 없으면 함수 종료
        if (availableRelicNumbers.Count == 0)
        {
            return 0;
        }

        // 랜덤으로 유물 번호 선택
        int randomIndex = Random.Range(0, availableRelicNumbers.Count);
        int randomRelicNumber = availableRelicNumbers[randomIndex];

        return randomRelicNumber;
    }
    public List<int> GetRandomRelicNumbers(int count)
    {
        List<int> result = new List<int>();
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        // 플레이어가 이미 보유한 유물 번호 리스트 생성
        List<int> playerRelicNumbers = new List<int>();
        foreach (Relic relic in playerRelics)
        {
            playerRelicNumbers.Add(relic.number);
        }

        // 추가 가능한 유물 번호 리스트 생성
        List<int> availableRelicNumbers = new List<int>();
        for (int i = 1; i <= allRelics.Count; i++)
        {
            if (!playerRelicNumbers.Contains(i))
            {
                availableRelicNumbers.Add(i);
            }
        }

        // 추가 가능한 유물이 없으면 빈 리스트 반환
        if (availableRelicNumbers.Count == 0)
        {
            return result;
        }

        // 필요한 개수만큼 유물 번호 선택
        for (int i = 0; i < count && availableRelicNumbers.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableRelicNumbers.Count);
            result.Add(availableRelicNumbers[randomIndex]);
            availableRelicNumbers.RemoveAt(randomIndex);
        }

        return result;
    }
    // 플레이어의 유물 리스트를 PlayerPrefs에 저장
    public void SavePlayerRelics()
    {
        string json = JsonUtility.ToJson(playerRelics);
        PlayerPrefs.SetString("PlayerRelics", json);
    }

    // PlayerPrefs에서 플레이어의 유물 리스트를 불러옴
    public void LoadPlayerRelics()
    {
        string json = PlayerPrefs.GetString("PlayerRelics");
        if (!string.IsNullOrEmpty(json))
        {
            playerRelics = JsonUtility.FromJson<List<Relic>>(json);
        }
    }

    //획득 시 발동
    public void AcquiredRelicEffect(int relicNumber)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        switch (relicNumber)
        {
            case 4:
                playerStats.maxHealth += 7;
                break;
            case 5:
                playerStats.gold += 150;
                break;
            case 14:
                playerStats.restrelic = 1;
                break;
            case 15:
                playerStats.shoprelic = 1;
                break;
            case 17:
                playerStats.attackcountrelic = 1;
                break;
            case 20:
                playerStats.cardarmor++;
                playerStats.basiccardarmor++;
                break;
            case 21:
                playerStats.barrier = 1;
                break;
            case 24:
                playerStats.revive = 1;
                break;
            case 26:
                playerStats.firststrike = 1;
                break;
            case 32:
                playerStats.maxHealth -= 20;
                if (playerStats.maxHealth < playerStats.currentHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                break;
            case 38:
                playerStats.gold += 50;
                playerStats.maxHealth += 5;
                playerStats.currentHealth += 5;
                break;
            case 39:
                playerStats.bankgold += playerStats.gold/5;
                playerStats.gold = 0;
                 Relic membershipCard = allRelics.FirstOrDefault(r => r.number == 39);
            if (membershipCard != null)
            {
                membershipCard.description = $"골드를 모두 은행에 맡깁니다. 전투 1번당 20%의 이자를 받습니다. ({playerStats.bankgold})";
            }
                break;
            default:
                return;
        }
    }

    //조건부 발동
    public void ConditionalRelicEffect(int relicNumber)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        EnemyStats enemyStats = FindObjectOfType<EnemyStats>();
        CardManager cardManager = FindObjectOfType<CardManager>();
        TurnManager turnManager = FindObjectOfType<TurnManager>();

        switch (relicNumber)
        {
            case 1:
                if (turnManager.turncounter == 1)
                {
                    playerStats.currentHealth += 5;
                    if (playerStats.currentHealth > playerStats.maxHealth)
                        playerStats.currentHealth = playerStats.maxHealth;
                }
                break;
            case 2:
                if (turnManager.turncounter == 1)
                {
                    cardManager.AddCard(2);
                }
                break;
            case 3:
                if (turnManager.turncounter == 1)
                {
                    playerStats.currentMana += 1;
                }
                break;
            case 6:
                if (turnManager.turncounter == 1)
                {
                    EnemyStats[] allEnemies = FindObjectsOfType<EnemyStats>();
                    foreach (EnemyStats enemy in allEnemies)
                    {
                          enemyStats.weaken += 1;
                    }
                }
                  
                break;
            case 7:
                if (turnManager.turncounter == 1)
                {
                    EnemyStats[] allEnemies = FindObjectsOfType<EnemyStats>();
                    foreach (EnemyStats enemy in allEnemies)
                    {
                        enemy.corrosion += 1;
                    }
                }
                break;
            case 8:
                if (turnManager.turncounter == 1)
                {
                    EnemyStats[] allEnemies = FindObjectsOfType<EnemyStats>();
                    foreach (EnemyStats enemy in allEnemies)
                    {
                        enemyStats.vulnerable += 1;
                    }
                }
                break;
            case 9:
                if (turnManager.turncounter == 1)
                    playerStats.gold += 10;
                break;
            case 10:
                if (turnManager.turncounter == 1)
                    playerStats.shield += 8;
                break;
            case 11:
                if (turnManager.turncounter == 1)
                    playerStats.power += 1;
                break;
            case 12:
                playerStats.power -= Tpower;
                Tpower = (playerStats.maxHealth - playerStats.currentHealth) / 15;
                playerStats.power += Tpower;
                break;
            case 13:
                if (turnManager.turncounter == 1)
                    playerStats.thorn += 3;
                break;

            case 18:
                if (turnManager.turncounter == 2)
                    playerStats.shield += 15;
                break;
            case 19:
                if (turnManager.turncounter == 3)
                    playerStats.shield += 25;
                break;
            case 23:
                if (turnManager.turncounter == 1)
                    playerStats.power += Random.Range(0, 3);
                break;
            default:
                return;
        }
    }

    //턴 끝날 때 발동
    public void EndTurnRelicEffect(int relicNumber)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();

        switch (relicNumber)
        {
            case 16:
                if (playerStats.shield == 0)
                    playerStats.shield += 8;
                break;
            case 22:
                for (int i = 0; i < enemyManager.enemies.Count; i++)
                {
                    EnemyStats enemy = enemyManager.enemies[i];
                    enemy.shield -= 4;
                    if (enemy.shield <= 0)
                    {
                        enemy.currentHealth += enemy.shield;
                        enemy.shield = 0;
                    }
                }
                break;
            case 25:
                for (int i = 0; i < enemyManager.enemies.Count; i++)
                {
                    EnemyStats enemy = enemyManager.enemies[i];
                    enemy.burn += 3;
                }
                break;
                
            default:
                return;
        }
    }
    public void ActivatePlayerRelicEffects(int num)
    {
        foreach (Relic relic in playerRelics)
        {
            if (num == 1)
                ConditionalRelicEffect(relic.number);
            else if (num == 2)
                EndTurnRelicEffect(relic.number);
            else
                return;
        }
    }
    //유물 초기화 함수
    public void RemoveAllPlayerRelics()
    {
        playerRelics.Clear();
    }


}

[System.Serializable]
public class Relic
{
    public string name;
    public string description;
    public int number;

    public Relic(string name, string description, int number)
    {
        this.name = name;
        this.description = description;
        this.number = number;
    }

}



