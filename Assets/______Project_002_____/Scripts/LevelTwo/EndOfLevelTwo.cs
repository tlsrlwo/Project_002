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
