using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class TESTweaponSpawner : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController.Instance.characterState = 0;
            }
        }
    }
}
