using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project002
{
    public class EnemyHealth : MonoBehaviour
    {
        public float currentHealth;
        public float maxHealth;
        //RagdollManager ragdollManager;
        [HideInInspector] public bool isDead;

        public Image fillImage;


        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            float percentage = currentHealth / maxHealth;

            fillImage.fillAmount = percentage;
        }


        public void TakeDamage(float damage)
        {
            if (currentHealth > 0)
            {
                currentHealth -= damage;
                if (currentHealth <= 0) EnemyDeath();
                else Debug.Log("Hit");
            }

        }
        void EnemyDeath()
        {
            //ragdollManager.TriggerRagdoll();
            Debug.Log("Death");
        }
    }
}
