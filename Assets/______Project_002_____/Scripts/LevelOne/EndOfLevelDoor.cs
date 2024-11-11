using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class EndOfLevelDoor : MonoBehaviour
    {
        public Canvas levelFinishedCanvas;


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && LevelOneEnemyManager.Instance.enemyCount <= 0)
            {
                print("Level Completed");
            }
        }
    }
}
