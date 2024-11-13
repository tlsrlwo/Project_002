using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project002
{
    public class MapEntrance : MonoBehaviour
    {
        public string levelName;
        
        public GameObject pressEButton;
        public GameObject enterLevelGameObject;

     


        private void Update()
        {        
            if (pressEButton && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(levelName);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                pressEButton.SetActive(true);
                enterLevelGameObject.SetActive(true);
                print(levelName);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                pressEButton.SetActive(false);
                enterLevelGameObject.SetActive(false);
            }
        }
    }
}
