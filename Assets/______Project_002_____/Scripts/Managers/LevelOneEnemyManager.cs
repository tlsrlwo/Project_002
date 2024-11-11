using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class LevelOneEnemyManager : MonoBehaviour
    {
        public static LevelOneEnemyManager Instance { get; set; }

        public int enemyCount;
        private int enemyFullAmount;
        private List<GameObject> allEnemies = new List<GameObject>();
        public Transform enemies;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            foreach (Transform enemy in enemies.transform)
            {
                allEnemies.Add(enemy.gameObject);
            }
            enemyCount = allEnemies.Count;
        }


    }
}
