using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class TitleScene : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;            
        }

        private void Update()
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }    
        }
    }
}
