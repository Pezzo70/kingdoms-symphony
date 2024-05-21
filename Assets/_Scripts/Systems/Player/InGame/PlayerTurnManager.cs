using System;
using System.Collections;
using System.Linq;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Level;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerTurnManager : MonoBehaviour
{
    public GameObject playersOptions;
    public GameObject playerEffectIcon;

    void Start()
    {
        PlaythroughContainer.Instance.currentTurn.Item2 = 0;
        EventManager.TurnChanged(Turn.PlayersTurn);
    }

    void OnEnable()
    {
        EventManager.TurnChanged += OnTurnChanged;
        EventManager.EnemyAttackExecuted += HandleEnemyAttack;
    }

    void OnDisable()
    {
        EventManager.TurnChanged -= OnTurnChanged;
        EventManager.EnemyAttackExecuted -= HandleEnemyAttack;
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
        playersOptions.SetActive(true);
    }

    private void OnEnemyTurn()
    {
        playersOptions.SetActive(false);
    }

    public void EndPlayersTurn()
    {
        EventManager.TurnChanged?.Invoke(Turn.EnemiesTurn);
    }

    private void HandleEnemyAttack(EnemyID enemyID, EnemyAttackID enemyAttackID)
    {
        EnemyAttack enemyAttack = EnemiesContainer
            .Instance
            .enemies
            .First(enemy => enemy.enemyID == enemyID)
            .attacks
            .First(attack => attack.enemyAttackID == enemyAttackID);

        StartCoroutine(AttackEffectCoroutine(enemyAttack));
    }

    private IEnumerator AttackEffectCoroutine(EnemyAttack enemyAttack)
    {
        SpriteRenderer spr = playerEffectIcon.GetComponent<SpriteRenderer>();
        spr.sprite = enemyAttack.attackSymbol;
        Animator playerEffectIconAnimator = playerEffectIcon.GetComponent<Animator>();
        playerEffectIconAnimator.SetBool("EffectIconShow", true);
        yield return new WaitForSeconds(1f);
        playerEffectIconAnimator.SetBool("EffectIconShow", false);
        spr.color = new Color(255, 255, 255, 0f);
    }
}
