using System.Collections.Generic;
using System.Linq;
using Kingdom.Enemies;
using UnityEngine;

namespace Kingdom.Level
{
    public class LevelSetupManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject[] enemiesPrefab;

        public Transform[] playerSpawnPoints;
        public Transform[] enemySpawnPoints;

        public GameObject entitiesParent;

        private List<Transform> _enemiesSpawnPointsToUse;

        void Awake()
        {
            _enemiesSpawnPointsToUse = new List<Transform>();
            _enemiesSpawnPointsToUse.AddRange(enemySpawnPoints);
            SpawnEntities();
        }

        private void SpawnEntities()
        {
            Transform playerSpawnPoint = playerSpawnPoints[
                Random.Range(0, playerSpawnPoints.Length)
            ];

            GameObject playerInstance = Instantiate(playerPrefab, entitiesParent.transform);
            playerInstance.transform.position = playerSpawnPoint.position;

            EnemyPerLevelPhase phaseInfo = PlaythroughContainer.Instance.GetCurrentLevelEnemies();

            for (int i = 0; i < phaseInfo.maxNumberOfEnemies; i++)
            {
                Debug.Log(phaseInfo.enemies.Length);
                Enemy enemy = phaseInfo.enemies[Random.Range(0, phaseInfo.enemies.Length)];
                Debug.Log(enemy.enemyID);
                GameObject enemyGameObject = enemiesPrefab.First(
                    obj => obj.GetComponent<EnemyEntity>().enemyData.enemyID == enemy.enemyID
                );

                enemyGameObject.name += i.ToString();

                Transform enemySpawnPoint = _enemiesSpawnPointsToUse[
                    Random.Range(0, _enemiesSpawnPointsToUse.Count)
                ];

                GameObject enemyInstance = Instantiate(enemyGameObject, entitiesParent.transform);
                enemyInstance.transform.position = enemySpawnPoint.position;

                _enemiesSpawnPointsToUse.Remove(enemySpawnPoint);
            }

            List<Transform> sortedChild = new List<Transform>();

            for (int i = 1; i < entitiesParent.transform.childCount; i++)
                sortedChild.Add(entitiesParent.transform.GetChild(i));

            sortedChild = sortedChild.OrderByDescending(obj => obj.position.y).ToList();

            for (int i = 0; i < sortedChild.Count; i++)
            {
                GameObject spriteContainer = sortedChild[i].GetChild(0).gameObject;
                SpriteRenderer spr = spriteContainer.GetComponent<SpriteRenderer>();
                spr.sortingOrder = i;
            }
        }
    }
}
