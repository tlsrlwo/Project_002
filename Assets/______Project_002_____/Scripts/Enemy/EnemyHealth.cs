using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class EnemyHealth : MonoBehaviour
    {
        public float health;
        //RagdollManager ragdollManager;
        [HideInInspector] public bool isDead;

        private void Start()
        {
            //ragdollManager = GetComponent<RagdollManager>();
        }

        public void TakeDamage(float damage)
        {
            if (health > 0)
            {
                health -= damage;
                if (health <= 0) EnemyDeath();
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
