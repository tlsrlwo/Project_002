using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class EndOfLevelDoor : MonoBehaviour
    {
        public Canvas levelFinishedCanvas;
        public GameObject pressEButton;

        private void Update()
        {
            if(pressEButton)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    print("E is pressed");
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && LevelOneEnemyManager.Instance.enemyCount <= 0)
            {
                print("Level Completed");
                pressEButton.SetActive(true);
               
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                pressEButton.SetActive(false);
            }
        }


    }
}
