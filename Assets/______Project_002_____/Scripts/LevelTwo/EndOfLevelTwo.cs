using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project002
{
    public class EndOfLevelTwo : MonoBehaviour
    {
        private bool playerIsCollide;

        private void Update()
        {
            if (playerIsCollide)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerPrefs.SetInt("UnlockedLevel", 1);
                    PlayerPrefs.SetInt("ReachedIndex", 1);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(0);                    
                }
            }
        }

        // 출처 : https://www.youtube.com/watch?v=2XQsKNHk1vk
        void UnlockNewLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))     // 현재 씬의 buildIndex 가 저장된 ReachedIndex와 같거나 클때
            {
                PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);   // ReachedIndex값을 현재 씬의 buildIndex + 1 의 값으로 변경
                PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);    // UnlockedLevel 값에도 + 1 해줌
                PlayerPrefs.Save();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                print("Level Completed");
                playerIsCollide = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsCollide = false;
            }
        }
    }
}
