using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 스크립트 출처 : https://www.youtube.com/watch?v=2XQsKNHk1vk

namespace Project002
{
    public class LevelMenu : MonoBehaviour
    {
        public Button[] buttons;
        public GameObject levelButtons;

        private void Awake()
        {

            ButtonsToArray();

            int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevel", 1);    // 첫번째 레벨은 게임실행부터 잠금해제

            for(int i = 0; i < buttons.Length; i++)     // 모든 레벨 버튼을 상호작용 불가로 설정한 뒤
            {
                buttons[i].interactable = false;
            }
            for(int i = 0; i < unlockedLevels; i++)     // 해제 된 unlockedLevel 갯수만큼 상호작용 다시 허용
            {
                buttons[i].interactable = true;
            }
        }

        public void OpenScene(int levelNum)
        {
            string levelName = "Level" + levelNum;
            SceneManager.LoadScene(levelName);
        }

        public void ButtonsToArray()
        {
            int childCount = levelButtons.transform.childCount;
            buttons = new Button[childCount];
            for(int i = 0; i < childCount; i++)
            {
                buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
            }
        }
    }
}
