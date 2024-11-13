using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class SwordItem : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerState.Instance.characterState = 2;
                Destroy(transform.gameObject);
            }
        }
    }
}
