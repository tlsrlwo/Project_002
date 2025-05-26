using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                  
                    UnlockNewLevel();
                    SceneManager.LoadScene(0);
                   // ����� unlockedLevel �� ����
                   // �� ���� ������ �Ѿ
                }
            }
        }

        // ��ó : https://www.youtube.com/watch?v=2XQsKNHk1vk
        void UnlockNewLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))     // ���� ���� buildIndex �� ����� ReachedIndex�� ���ų� Ŭ��
            {
                PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);   // ReachedIndex���� ���� ���� buildIndex + 1 �� ������ ����
                PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);    // UnlockedLevel ������ + 1 ����
                PlayerPrefs.Save();
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
