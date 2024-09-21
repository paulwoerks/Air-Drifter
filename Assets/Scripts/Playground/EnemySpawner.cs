using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] int enemyBudget = 10;
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] TransformAnchor playerAnchor;
        List<GameObject> enemies = new();

        void Update()
        {
            if (enemies.Count < enemyBudget)
            {
                SpawnEnemy();
            }
        }

        void SpawnEnemy()
        {
            Quaternion rotation = Quaternion.identity;

            GameObject enemy = Pooler.Spawn(enemyPrefab, GetSpawnPosition(), rotation);
            enemies.Add(enemy);
;        }

        Vector2 GetSpawnPosition()
        {
            Vector2 distance = new Vector2(5, 20);
            Vector2 spawnPosition = new Vector2(Random.Range(distance.x, distance.y), Random.Range(distance.x, distance.y));

            if (Random.Range(0,1) < 0.5f)
                spawnPosition.x *= -1;
            if (Random.Range(0, 1) < 0.5f)
                spawnPosition.y *= -1;

            return (Vector2)playerAnchor.Value.position + spawnPosition;
        }
    }
}
