using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class GunDescription : MonoBehaviour
    {
        public GameObject gunDescriptionUI;


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                gunDescriptionUI.SetActive(true);
                LevelOneTutorialCanvas.Instance.isOpen = true;
            }
        }
    }
}
