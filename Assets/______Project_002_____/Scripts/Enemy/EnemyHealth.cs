using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Project002
{
    public class EnemyHealth : MonoBehaviour
    {
        public float currentHealth;
        public float maxHealth;
        //RagdollManager ragdollManager;
        [HideInInspector] public bool isDead;

        private Animator anim;

        public Canvas hpCanvas;
        public Image fillImage;
        private NavMeshAgent navMeshAgent;
        private EnemyController enemyController;
        private CapsuleCollider capsuleCollider;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemyController = GetComponent<EnemyController>();
            currentHealth = maxHealth;
            anim = GetComponentInChildren<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();  
            isDead = false;
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
                if (currentHealth <= 0)
                {
                    navMeshAgent.isStopped = true;
                    capsuleCollider.enabled = false;
                    hpCanvas.gameObject.SetActive(false);
                    enemyController.enabled = false;
                    isDead = true;
                    EnemyDeath();
                    StartCoroutine(DestroyEnemyObject());
                }               
            }
        }

        IEnumerator DestroyEnemyObject()
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }

        void EnemyDeath()
        {
            anim.SetTrigger("isDead");
            Debug.Log("Enemy Death");
        }
    }
}
