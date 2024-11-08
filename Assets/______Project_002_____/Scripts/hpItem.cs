using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class hpItem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth.Instance.currentHealth++;
                if(PlayerHealth.Instance.currentHealth >= PlayerHealth.Instance.maxHealth)
                {
                    PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
                }
            }
        }
    }
}
