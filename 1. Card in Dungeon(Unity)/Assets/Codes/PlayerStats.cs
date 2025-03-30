using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 70;
    public int currentHealth = 70;
    public int gold = 100;
    public int bankgold=0;
    public int Rand_Map_Gene;
    public int revive=0; // 부활 유물 획득 시 1

    public int character = 0;// 0전사, 1도적 , 2마법사
    public int maxMana = 3;
    public int currentMana = 3;
    public int shield = 0;
    public int shieldcounter = 0;  //shieldcounter 1당 카드를 사용할 때마다 실드 +4
    public int basiccardarmor=0;
    public int cardarmor=0; // 카드를 사용할 때마다 이 수치만큼 실드를 얻음
    public int enemynumber = 0; // 0,1,2: Normal, 3,4: Elite, 5: Boss
    public int enemytype=0;// 0: 노말, 1: 엘리트, 2: 보스
    public int restrelic=0; //휴식 유물이 있다면 휴식 시 체력 10 추가 회복
    public int shoprelic=0; //상점 유물이 있다면 상점 도착 시 체력 10 회복    
    public int combattraining=0; //사용한 전투 훈련 카드 1장 당 턴이 끝날 때 힘+4
    public int Defensemagic=0;
    public int firststrike =0; //선제공격 유물이 있다면 턴 시작시 모든 적에게 피해 5

    private int currentStarLevel; //현재 별 레벨

    //상점 코드 작성 시 회복 코드 추가 필요

    //버프 
    public int power = 0;// 이 수치만큼 공격할 때 데미지 추가
    public int powercounter =0; // 턴이 끝날 때마다 powercounter*3 만큼 힘 증가
    public int thorn = 0;//공격받았을 때 이 수치만큼 공격한 적에게 데미지

    public int barrier = 0; //1 이면 한 턴에 실드를 20까지만 잃음
    public int shieldmanager=0; //1 이면 턴 종료 시 실드를 잃지 않음
    public int extramana = 0; //다음 턴에 추가 마나 획득
    public int extrashield = 0; //다음 턴에 추가 실드 획득
    public int firesea=0; //내 턴이 끝날 때 모든 적에게 화상 +3
    public int attackcount = 0; // 공격 카드를 5번 사용하면 힘+1
    public int startGoldLevel = 0;
    public int maxHealthLevel=0;

    //디버프
    public int weaken = 0; //가하는 피해 25% 감소
    public int corrosion = 0; //얻는 실드 25% 감소
    public int vulnerable = 0;//받는 피해 50% 증가
    public int nodraw = 0; //이번 턴에 더이상 드로우 불가
    public int burn = 0; //화상 수치만큼 데미지, 턴 시작시 1 감소
    public int mdraw = 0; //수치만큼 다음턴에 뽑는 카드 수 감소 
    public int attackcountrelic=0; //유물을 얻으면 1


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Rand_Map_Gene = Random.Range(1, 6);

    }
    void Start()
    {
        InvokeRepeating("MyFunction", 0, 0.2f);
        currentStarLevel = PlayerPrefs.GetInt("CurrentStarLevel", 0);
        Debug.Log("Current Star Level: " + currentStarLevel);

        
        if(currentStarLevel>=9)
        {
            gold-=50;
        }
        if(currentStarLevel>=10)
        {
            maxHealth-=3;
        }
        if(currentStarLevel>=11)
        {
            maxHealth-=3;
        }
        startGoldLevel = PlayerPrefs.GetInt("StartGoldLevel", 0);
        gold+=startGoldLevel*50;
        maxHealthLevel = PlayerPrefs.GetInt("MaxHealthLevel", 0);
        maxHealth+=maxHealthLevel*3;
        currentHealth=maxHealth;

        if(currentStarLevel>=7)
        {
            currentHealth-=5;
        }
    }
    void Update()
    {
        
    }
    void MyFunction()
    {
        // 0.2초에 1번만 호출
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Die();
    }
    public void Die(){
        if(revive==0)
            {
                if(currentHealth<=0)
                {
                    //사망
                    SceneManager.LoadScene("Dead");

                }
            }
            else{ //부활 유물이 있으면 1회 부활 후 50%회복
                currentHealth=maxHealth/2;
                revive=0;
            }
    }
}
