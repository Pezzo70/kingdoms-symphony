using System.Linq;
using Kingdom.Enemies;
using Kingdom.Enums.Enemies;
using Kingdom.Level;
using UnityEngine;

public class VictoryEmitter : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.EnemyBanished += OnEnemyBanishedHandler;
    }

    void OnDisable()
    {
        EventManager.EnemyBanished -= OnEnemyBanishedHandler;
    }

    private void OnEnemyBanishedHandler(EnemyID _)
    {
        GameObject[] enemiesObjs = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemiesObjs.Length > 0)
        {
            EnemyEntity[] enemyEntities = enemiesObjs
                .Select(obj => obj.GetComponent<EnemyEntity>())
                .ToArray();

            if (enemyEntities.All(obj => obj.IsDead))
            {
                if (PlaythroughContainer.Instance.GetCurrentLevel.endGame)
                    EventManager.OnVictory?.Invoke(true);
                else
                    EventManager.OnVictory?.Invoke(false);
            }
        }
    }
}
