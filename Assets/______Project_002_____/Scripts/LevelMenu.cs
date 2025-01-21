using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ��ũ��Ʈ ��ó : https://www.youtube.com/watch?v=2XQsKNHk1vk

namespace Project002
{
    public class LevelMenu : MonoBehaviour
    {
        public Button[] buttons;
        public GameObject levelButtons;

        private void Awake()
        {

            ButtonsToArray();

            int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevel", 1);    // ù��° ������ ���ӽ������ �������

            for(int i = 0; i < buttons.Length; i++)     // ��� ���� ��ư�� ��ȣ�ۿ� �Ұ��� ������ ��
            {
                buttons[i].interactable = false;
            }
            for(int i = 0; i < unlockedLevels; i++)     // ���� �� unlockedLevel ������ŭ ��ȣ�ۿ� �ٽ� ���
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
