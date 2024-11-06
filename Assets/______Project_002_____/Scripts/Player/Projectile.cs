using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 30f;
        public float lifeTime = 10f;

        public float damage = 1f;

        public GameObject hitEffect;
        EnemyHealth enemyHealth;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
           if(collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    Debug.Log("PlayerHitByBullet");
                    enemyHealth.TakeDamage(damage);
                    print("Enemy hit");
                }
                //enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }


    }
}
