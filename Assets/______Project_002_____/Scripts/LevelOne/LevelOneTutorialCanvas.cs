using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class LevelOneTutorialCanvas : MonoBehaviour
    {
        // UI 들 관리해주는 스크립트
        public static LevelOneTutorialCanvas Instance { get; set; }

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

        // 캔버스 & 버튼
        public GameObject howToMoveUI;
        public GameObject howToLookUI;
        public GameObject howToShootUI;
        public GameObject pauseScreenUI;


        public bool isOpen = false;

        void Start() 
        {
            isOpen = false;
            howToLookUI.SetActive(false);
            howToShootUI.SetActive(false);
            StartCoroutine(ShowTutorialCanvas());
        }

        private void Update()
        {
            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0.001f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Time.timeScale = 1.0f;
            }
            OpenPauseScreen();
    
        }

        IEnumerator ShowTutorialCanvas()
        {
            yield return new WaitForSeconds(3f);
            howToMoveUI.SetActive(true);

            isOpen = true;
        }

 
        void OpenPauseScreen()
        {
            if(!isOpen && Input.GetKeyDown(KeyCode.Tab))
            {
                pauseScreenUI.SetActive(true);
                isOpen = true;
            }
          
        }

   

    }
}
