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
        EventManager.EnemyBanished += OnEnemyBanished;
        EventManager.NextEnemy += OnNextEnemy;
    }

    void OnDisable()
    {
        EventManager.TurnChanged -= OnTurnChanged;
        EventManager.EnemyBanished -= OnEnemyBanished;
        EventManager.NextEnemy -= OnNextEnemy;
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

    private void OnPlayerTurn() { }

    private void OnEnemyTurn()
    {
        _currentEnemyIndex = 0;
        StartCoroutine(WaitTillEachEnemyIsDone());
    }

    private void OnNextEnemy() => _currentEnemyIndex++;

    void OnEnemyBanished(EnemyID _)
    {
        if (PlaythroughContainer.Instance.currentTurn.Item1 == Turn.PlayersTurn)
            aliveEnemies = aliveEnemies.Where(obj => !obj.IsDead).ToList();
    }

    void EndEnemiesTurn() => EventManager.TurnChanged?.Invoke(Turn.PlayersTurn);

    private IEnumerator WaitTillEachEnemyIsDone()
    {
        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            aliveEnemies[_currentEnemyIndex].ExecuteTurn();
            while (i == _currentEnemyIndex)
                yield return new WaitForSeconds(1f);
        }

        aliveEnemies = aliveEnemies.Where(obj => !obj.IsDead).ToList();
        yield return new WaitForSeconds(1f);
        EndEnemiesTurn();
    }
}
