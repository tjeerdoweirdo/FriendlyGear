using System.Collections.Generic;
using UnityEngine;

namespace FriendlyGear.Runtime
{
    public class GameManager : MonoBehaviour
    {
        public MissionManager missionManager;
        public BaseSpawner baseSpawner;
        public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

        void Awake()
        {
            if (!missionManager) missionManager = FindObjectOfType<MissionManager>();
            if (!baseSpawner) baseSpawner = FindObjectOfType<BaseSpawner>();
        }
    }
}
