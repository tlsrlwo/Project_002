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
                    if (LevelManager.Instance.currentStageLevel < 3)
                    {
                        LevelManager.Instance.currentStageLevel++;
                        StartCoroutine(LoadNextScene());
                        SaveManager.Instance.SaveGame();
                        
                        //LevelManager.Instance.playerbody.transform.position = LevelManager.Instance.levelEntrance[LevelManager.Instance.currentStageLevel].transform.position;
                    }
                }
            }
        }

        IEnumerator LoadNextScene()
        {
            SceneManager.LoadScene("MapChoosingScene");
            yield return new WaitForSeconds(1f);
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
