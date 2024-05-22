using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Level;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<EnemyEntity> aliveEnemies;
    private int _currentEnemyIndex;
    private int _currentEnemyTakingDamageIndex;

    void Start()
    {
        aliveEnemies = GameObject
            .FindGameObjectsWithTag("Enemy")
            .Select(obj => obj.GetComponent<EnemyEntity>())
            .ToList();
    }

    void OnEnable()
    {
        EventManager.TurnChanged += OnTurnChanged;
        EventManager.NextEnemy += OnNextEnemy;
        EventManager.NextEnemyTakesDamage += OnNextEnemyTakesDamage;
        EventManager.EnemiesDamaged += HandleEnemiesDamaged;
        EventManager.EnemiesRegainMana += HandleEnemiesRegainMana;
    }

    private void OnNextEnemyTakesDamage() => _currentEnemyTakingDamageIndex++;

    void OnDisable()
    {
        EventManager.TurnChanged -= OnTurnChanged;
        EventManager.NextEnemy -= OnNextEnemy;
        EventManager.NextEnemyTakesDamage -= OnNextEnemyTakesDamage;
        EventManager.EnemiesDamaged -= HandleEnemiesDamaged;
        EventManager.EnemiesRegainMana -= HandleEnemiesRegainMana;
    }

    private void OnTurnChanged(Turn turn)
    {
        switch (turn)
        {
            case Turn.PlayersTurn:
                OnPlayerTurn();
                break;
            case Turn.EnemiesTurn:
                OnEnemyTurn();
                break;
        }
    }

    private void OnPlayerTurn()
    {
        _currentEnemyTakingDamageIndex = 0;
    }

    private void OnEnemyTurn()
    {
        _currentEnemyIndex = 0;
        EventManager.CantPause?.Invoke(false);
        StartCoroutine(WaitTillEachEnemyIsDone());
    }

    private void HandleEnemiesRegainMana()
    {
        for (int i = 0; i < aliveEnemies.Count; i++)
            aliveEnemies[i].RegainMana();
    }

    private void HandleEnemiesDamaged(float damage, float massiveDamage) =>
        StartCoroutine(WaitTillEachEnemyTakeDamage(damage, massiveDamage));

    private void OnNextEnemy() => _currentEnemyIndex++;

    void EndEnemiesTurn() => EventManager.TurnChanged?.Invoke(Turn.PlayersTurn);

    private IEnumerator WaitTillEachEnemyIsDone()
    {
        EventManager.CantPause?.Invoke(true);

        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            if (PlaythroughContainer.Instance.PlayerStats.CurrentMoral == 0)
                break;

            aliveEnemies[_currentEnemyIndex].RegainMana();
            aliveEnemies[_currentEnemyIndex].ExecuteTurn();
            while (i == _currentEnemyIndex)
                yield return new WaitForSeconds(1f);
        }

        aliveEnemies = aliveEnemies.Where(obj => !obj.IsDead).ToList();
        yield return new WaitForSeconds(1f);
        EndEnemiesTurn();
    }

    private IEnumerator WaitTillEachEnemyTakeDamage(float damage, float massiveDamage)
    {
        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            aliveEnemies[_currentEnemyTakingDamageIndex].ReduceMoral(damage, massiveDamage);
            while (i == _currentEnemyTakingDamageIndex)
                yield return new WaitForSeconds(1f);
        }

        aliveEnemies = aliveEnemies.Where(obj => !obj.IsDead).ToList();
        EventManager.EnemiesEndTakingDamage?.Invoke();
    }
}
