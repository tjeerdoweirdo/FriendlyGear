using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public float spawnInterval = 10f;
        public int maxAlive = 10;
        public Transform spawnPoint;

        float nextSpawn;
        int alive;

        void Update()
        {
            if (!enemyPrefab) return;
            if (alive >= maxAlive) return;
            if (Time.time >= nextSpawn)
            {
                var pos = spawnPoint ? spawnPoint.position : transform.position;
                var rot = spawnPoint ? spawnPoint.rotation : transform.rotation;
                var go = Instantiate(enemyPrefab, pos, rot);
                alive++;
                nextSpawn = Time.time + Mathf.Max(1f, spawnInterval);
            }
        }
    }
}
