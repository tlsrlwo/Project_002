using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project002
{
    public class EnemyHit : MonoBehaviour
    {
        public SphereCollider sphereCollider;
        

        private void Start()
        {
            
        }

        void EnemyPunch()
        {
            PlayerHealth.Instance.currentHealth--;
        }
        
        private void OnTriggetEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {               
                EnemyPunch();
            }
        }
    }
}
