using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    public GameObject PlayerDmgEffectPrefab;
    public int DamageToEnemy = 0;
    public void AddShieldToPlayer(int shield)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats.corrosion > 0)
            playerStats.shield += (int)(shield * 0.75f);
        else
            playerStats.shield += shield;
    }
    // 플레이어에게 효과를 적용하는 카드
    public void ApplyPlayerEffect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        RelicManager relicManager = FindObjectOfType<RelicManager>();
        CardManager cardManager = FindObjectOfType<CardManager>();
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        int cardnumber = cardManager.lastUsedCard.GetItem().number; //사용한 카드의 number
        // Card card = FindObjectOfType<Card>();
        // card.CardUseSoundController(cardnumber);
        switch (cardnumber)
        {
            case 3:
                AddShieldToPlayer(5);
                break;
            case 4:
                AddShieldToPlayer(12);
                break;
            case 8:
                playerStats.currentHealth += 5;
                if (playerStats.currentHealth > playerStats.maxHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                break;
            case 10:
                AddShieldToPlayer(6);
                playerStats.thorn += 3;
                break;
            case 14:
                AddShieldToPlayer(3);
                cardManager.AddCard(1);
                break;
            case 18:
                AddShieldToPlayer(8);
                break;
            case 19:
                playerStats.gold += 30;
                break;
            case 21:
                cardManager.AddCard(6);
                break;

            case 101:
                playerStats.shieldmanager = 2;
                break;
            case 104:
                AddShieldToPlayer(relicManager.playerRelics.Count); //보유 유물 수 *2만큼 실드 획득
                break;
            case 105:
                playerStats.shieldcounter += 1;  //shieldcounter 1이면 카드를 낼 때마다 실드+4
                break;
            case 106:
                playerStats.currentHealth -= 2;
                AddShieldToPlayer(15);
                break;
            case 107:
                playerStats.powercounter += 1;
                break;
            case 108:
                playerStats.currentHealth -= 1;
                playerStats.power += 3;
                break;
            case 203:
                AddShieldToPlayer(turnManager.NumberOfCard);
                break;
            case 204:
                cardManager.AddCard(2);
                break;
            case 205:
                cardManager.AddCard(5);
                cardManager.throwcard = 2;
                break;
            case 207:
                playerStats.currentMana += 2;
                break;
            case 208:
                cardManager.AddCard(1);
                break;
            case 209:
                playerStats.cardarmor++;
                break;
            case 210:
                playerStats.extramana += playerStats.currentMana;
                playerStats.currentMana = 0;
                break;
            case 212:
                int rangold = Random.Range(0, 2);
                if (rangold == 0)
                    playerStats.gold -= 50;
                else
                    playerStats.gold += 200;

                if (playerStats.gold < 0)
                    playerStats.gold = 0;

                break;
            case 213:
                if (playerStats.gold >= 100)
                {
                    playerStats.gold -= 100;
                    playerStats.maxHealth += 5;
                    playerStats.currentHealth += 5;
                }
                break;
            case 214:
                if (playerStats.gold >= 50)
                {
                    playerStats.gold -= 50;
                    playerStats.currentMana += 3;
                }
                break;
            case 215:
                AddShieldToPlayer(Random.Range(5, 11));
                break;
            case 217:
                int ranbuff = Random.Range(0, 4);
                switch (ranbuff)
                {
                    case 0:
                        AddShieldToPlayer(5);
                        break;
                    case 1:
                        playerStats.power += 2;
                        break;
                    case 2:
                        playerStats.currentMana += 2;
                        break;
                    case 3:
                        playerStats.maxMana += 1;
                        break;
                }
                break;
            case 218:
                playerStats.combattraining++;
                break;

            case 302:
                playerStats.maxMana++;
                break;
            case 303:
                playerStats.maxMana += 2;
                break;
            case 304:
                playerStats.currentMana += 2;
                break;
            case 306:
                AddShieldToPlayer(playerStats.maxMana * 4);
                break;
            case 307:
                cardManager.AddCard(3);
                break;
            case 311:
                playerStats.firesea++;
                break;
            case 312:
                cardManager.AddCard(1);
                AddShieldToPlayer(8);
                break;
            case 318:
                playerStats.Defensemagic++;
                break;
            case 319:
                AddShieldToPlayer(30);
                break;
            case 320:
                cardManager.AddCard(1);
                playerStats.currentMana += 2;
                if (playerStats.currentMana > playerStats.maxMana)
                    playerStats.currentMana = playerStats.maxMana;
                break;
            case 402:
                cardManager.AddCard(1);
                break;
            case 406:
                playerStats.shield += 1;
                break;
            default:
                return;
        }

    }

    //피격 모션
    private IEnumerator MoveEnemyToPosition(Transform enemyTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = enemyTransform.position;
        float elapsedTime = 0f;

        // Move to target position
        while (elapsedTime < duration)
        {
            enemyTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyTransform.position = targetPosition; // Ensure it ends exactly at the target position

        // Wait for a moment before returning to original position
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed

        // Move back to the original position
        elapsedTime = 0f;
        while (elapsedTime < duration) // Use the same duration for return movement
        {
            enemyTransform.position = Vector3.Lerp(targetPosition, startPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyTransform.position = startPosition; // Ensure it ends exactly at the start position
    }

    // 단일 적을 대상으로 하는 효과

    public void ApplySingleEnemyEffect(int enemynumber)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        EnemyStats enemy = enemyManager.enemies[enemynumber];
        DamageToEnemy = 0;

        CardManager cardManager = FindObjectOfType<CardManager>();
        int cardnumber = cardManager.lastUsedCard.GetItem().number;
        int Numberofattacks = 1; // 공격 횟수
        int Absorption = 0; // 피해만큼 체력 회복
        // Card card = FindObjectOfType<Card>();
        // card.CardUseSoundController(cardnumber);
         switch (cardnumber)
        {
            case 1:
                DamageToEnemy = 5;
                break;
            case 2:
                DamageToEnemy = 12;
                break;
            case 5:
                DamageToEnemy = 10;
                enemy.vulnerable += 2;
                break;
            case 6:
                DamageToEnemy = 22;
                break;
            case 11:
                DamageToEnemy = 6;
                playerStats.currentHealth += 2;
                if (playerStats.currentHealth > playerStats.maxHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                break;
            case 12:
                DamageToEnemy = 2;
                cardManager.AddCard(1);
                break;
            case 15:
                enemy.vulnerable += 3;
                break;
            case 16:
                enemy.weaken += 3;
                break;
            case 17:
                DamageToEnemy = 8;
                break;
            case 20:
                playerStats.power++;
                AddShieldToPlayer(1);
                DamageToEnemy = 1;
                cardManager.AddCard(1);
                break;


            case 102:
                DamageToEnemy = playerStats.shield;
                break;
            case 103:
                DamageToEnemy = 6;
                AddShieldToPlayer(6);
                break;
            case 109:
                DamageToEnemy = 2;
                Numberofattacks = 6;
                break;
            case 110:
                DamageToEnemy = 2;
                Numberofattacks = 2;
                Absorption = 1;
                break;
            case 201:
                DamageToEnemy = turnManager.NumberOfCard;
                break;
            case 202:
                playerStats.power += 1;
                DamageToEnemy = 3;
                break;
            case 206:
                Numberofattacks = 3;
                DamageToEnemy = 2;
                break;
            case 211:
                DamageToEnemy = Random.Range(5, 11);
                break;
            case 216:
                int randebuff = Random.Range(0, 4);
                switch (randebuff)
                {
                    case 0:
                        enemy.weaken += 2;
                        break;
                    case 1:
                        enemy.power--;
                        break;
                    case 2:
                        enemy.vulnerable += 2;
                        break;
                    case 3:
                        enemy.burn += 3;
                        break;
                }
                break;
            case 219:
                DamageToEnemy = 6;
                playerStats.gold += 20;
                break;
            case 220:
                int coinattack = playerStats.gold / 10;
                playerStats.gold -= coinattack;
                DamageToEnemy = coinattack;
                break;
            case 301:
                DamageToEnemy = 6;
                enemy.burn += 4;
                break;
            case 305:
                DamageToEnemy = 20;
                enemy.burn += 8;
                break;
            case 308:
                enemy.power -= 3;
                break;
            case 309:
                DamageToEnemy = 4;
                Numberofattacks = 2;
                break;
            case 313:
                enemy.burn *= 2;
                break;
            case 314:
                cardManager.AddCard(1);
                enemy.burn += 4;
                break;
            case 315:
                playerStats.gold += enemy.burn / 10;
                break;
            case 316:
                enemy.burn += playerStats.maxMana * 3;
                break;
            case 317:
                AddShieldToPlayer(enemy.burn);
                break;
            case 405:
                DamageToEnemy = 1;
                break;
            default:
                return;
        }
        if (DamageToEnemy != 0)
        {
            DamageToEnemy += playerStats.power;
            if (enemy.vulnerable > 0)
                DamageToEnemy = Mathf.RoundToInt(DamageToEnemy * 1.5f);

            for (int i = 0; i < Numberofattacks; i++)
            {
                enemy.shield -= DamageToEnemy;

                if (Absorption == 1)
                {
                    playerStats.currentHealth += DamageToEnemy;
                    if (playerStats.currentHealth > playerStats.maxHealth)
                        playerStats.currentHealth = playerStats.maxHealth;
                }
                if (enemy.shield <= 0)
                {
                    enemy.currentHealth += enemy.shield;
                    enemy.shield = 0;
                }
            }
            StartCoroutine(MoveEnemyToPosition(enemy.transform, new Vector3(enemy.transform.position.x + 200f, enemy.transform.position.y, enemy.transform.position.z), 0.25f));
            // PlayerDmgEffect를 생성
            GameObject PlayerDmgEffect;
            if (playerStats.enemynumber <= 1)
            {
                if (enemynumber == 0)
                {
                    PlayerDmgEffect = Instantiate(PlayerDmgEffectPrefab, new Vector3(1273, 826f, 0f), Quaternion.identity);

                }
                else
                {
                    PlayerDmgEffect = Instantiate(PlayerDmgEffectPrefab, new Vector3(1649, 826f, 0f), Quaternion.identity);

                }
            }
            else
            {
                PlayerDmgEffect = Instantiate(PlayerDmgEffectPrefab, new Vector3(1116, 826f, 0f), Quaternion.identity);

            }

            // 1초 뒤 제거
            if (PlayerDmgEffect != null)
            {
                Destroy(PlayerDmgEffect, 0.5f);
            }
        }

    }


    // 모든 적을 대상으로 하는 효과
    public void ApplyAllEnemiesEffect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            EnemyStats enemy = enemyManager.enemies[i];

            DamageToEnemy = 0;

            CardManager cardManager = FindObjectOfType<CardManager>();
            int cardnumber = cardManager.lastUsedCard.GetItem().number;
            int Numberofattacks = 1; //공격횟수
            int Absorption = 0; // 피해만큼 체력 회복
            // Card card = FindObjectOfType<Card>();

            // card.CardUseSoundController(cardnumber);

            switch (cardnumber)
            {
                case 7:
                    DamageToEnemy = 10;
                    break;

                case 9:
                    DamageToEnemy = 10;
                    break;

                case 13:
                    DamageToEnemy = 6;
                    break;
                case 310:
                    enemy.burn += 4;
                    break;
                case 403:
                    enemy.burn += 3;
                    playerStats.burn += 3;
                    break;
                default:
                    return;
            }
            if (DamageToEnemy != 0)
            {

                DamageToEnemy += playerStats.power;

                if (enemy.vulnerable > 0) //받는 피해 50% 증가
                    DamageToEnemy = Mathf.RoundToInt(DamageToEnemy * 1.5f);

                for (int j = 0; j < Numberofattacks; j++) // 공격 횟수만큼 반복
                {
                    enemy.shield -= DamageToEnemy;
                    if (Absorption == 1) // 피해만큼 체력 회복
                    {
                        playerStats.currentHealth += DamageToEnemy;
                        if (playerStats.currentHealth > playerStats.maxHealth)
                            playerStats.currentHealth = playerStats.maxHealth;
                    }
                    if (enemy.shield <= 0)
                    {
                        enemy.currentHealth += enemy.shield;
                        enemy.shield = 0;
                    }
                }
                StartCoroutine(MoveEnemyToPosition(enemy.transform, new Vector3(enemy.transform.position.x + 200f, enemy.transform.position.y, enemy.transform.position.z), 0.25f));
                GameObject PlayerDmgEffect;
                if (playerStats.enemynumber <= 1)
                {
                    PlayerDmgEffect = Instantiate(PlayerDmgEffectPrefab, new Vector3(1273, 826f, 0f), Quaternion.identity);
                    PlayerDmgEffect = Instantiate(PlayerDmgEffectPrefab, new Vector3(1649, 826f, 0f), Quaternion.identity);
                }
                else
                {
                    PlayerDmgEffect = Instantiate(PlayerDmgEffectPrefab, new Vector3(1116, 826f, 0f), Quaternion.identity);

                }
                // 1초 뒤 제거
                if (PlayerDmgEffect != null)
                {
                    Destroy(PlayerDmgEffect, 0.5f);
                }
            }
        }
    }
}
