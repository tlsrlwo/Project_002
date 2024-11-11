using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Project002
{
    public class uiButtons : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(() =>
                LevelOneTutorialCanvas.Instance.isOpen = false
            );
        }




    }
}
