using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] int maxEnemies = 5;
        [SerializeField] bool debug;
        [SerializeField] LayerMask spawnableLayers;
        [SerializeField] Transform parent;
        [SerializeField] TransformAnchor playerAnchor;
        [SerializeField] GroupAnchors activeEnemies;

        [SerializeField] GameObject prefab;

        private void Start()
        {
            Spawn(prefab);
        }

        private void Update()
        {
            int desiredEnemies = maxEnemies - activeEnemies.Group.Count;
            if (desiredEnemies > 0)
            {
                Spawn(prefab);
            }
        }


        public void Spawn(GameObject prefab)
        {
            StartCoroutine(SpawnGroup(prefab, 2, 6));
        }

        IEnumerator SpawnGroup(GameObject prefab, int groupSize = 1, int groupSizeMax = 1)
        {
            if (playerAnchor.IsSet)
            {
                Vector3 spawnArea = GetSpawnPoint(playerAnchor.Value.position, 15);

                int units = Random.Range(groupSize, groupSizeMax + 1);
                float unitDistance = 3f;

                List<Vector3> spawnPositions = new();
                for (int i = 0; i < units; i++)
                    spawnPositions.Add(GetSpawnPoint(spawnArea, unitDistance));

                yield return new WaitForSeconds(.5f);

                foreach (Vector3 position in spawnPositions)
                {
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    GameObject enemy = Pooler.Spawn(prefab, position, rotation, parent);
                }
            }
        }

        Vector3 GetSpawnPoint(Vector3 origin, float radius)
        {

            bool isWalkable = false;
            Vector3 position = default;

            int i = 0;

            while (!isWalkable && i < 20)
            {
                Vector3 randomPosition = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
                position = origin + randomPosition;

                isWalkable = IsWalkable(position);
                i++;
            }

            if (position.Equals(default))
                Debug.LogError("No valid Position found");

            return position;
        }

        public bool IsWalkable(Vector3 position)
        {
            float height = 10f;
            position.y = height;

            RaycastHit hit;

            if (Physics.Raycast(position, Vector3.down, out hit, height, spawnableLayers))
                return true;
            else
                return false;
        }
    }
}
