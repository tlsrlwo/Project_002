using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class WeaponSpawner : MonoBehaviour
    {
        public ParticleSystem weaponItemEffect;
        public ParticleSystem secondWeaponItemEffect;

        private void Update()
        {
            //weaponItemEffect.gameObject.SetActive(true);
            //secondWeaponItemEffect.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                PlayerState.Instance.characterState = 1;
                Destroy(transform.gameObject);
            }
        }
    }
}
