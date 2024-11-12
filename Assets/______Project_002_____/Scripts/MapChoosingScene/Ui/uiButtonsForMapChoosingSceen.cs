using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project002
{
    public class uiButtonsForMapChoosingSceen : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(() =>
                MapChoosingSceneCanvasManager.Instance.isOpen = false
            );
        }
    }
}
