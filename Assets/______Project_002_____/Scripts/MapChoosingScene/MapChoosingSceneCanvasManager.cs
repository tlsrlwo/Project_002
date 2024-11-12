using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class MapChoosingSceneCanvasManager : MonoBehaviour
    {
        public static MapChoosingSceneCanvasManager Instance { get;  set; } 

        public GameObject pauseScreenUI;

        public bool isOpen;


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
            // esc 창 여는 부분
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!isOpen)
                {
                    OpenPauseScreen();
                }
                else
                {
                    ClosePauseScreen();
                }
            }

        }

        void OpenPauseScreen()
        {
            pauseScreenUI.SetActive(true);
            isOpen = true;
        }

        void ClosePauseScreen()
        {
            pauseScreenUI.SetActive(false);
            isOpen = false;
        }


    }
}
