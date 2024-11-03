using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Project002
{
    public class WeaponAmmo : MonoBehaviour
    {
        public static WeaponAmmo Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public int currentAmmo;
        public int maxAmmo;

        public float chargeTimerDelta = 0.0f;
        private float chargeTimerDelay = 3.0f;
        



        private void Start()
        {
            currentAmmo = maxAmmo;
            
        }

        private void Update()
        {       
            if (!Weapon.Instance.isFiring && chargeTimerDelta > 0.0f && currentAmmo < maxAmmo)        // 쏘고 있지 않고, 마지막 발사 시간에서 2초가 지났고, 현재총알이 최대총알보다 적으면
            {
                chargeTimerDelta -= Time.deltaTime;
                if(chargeTimerDelta < 0.0f )
                {
                    StartCoroutine(WeaponChargingTime());
                }
            }
        }

        private IEnumerator WeaponChargingTime()
        {
            yield return null;
            currentAmmo = maxAmmo;
            
        }

        public void AmmoUpdateTimer()
        {
            if (Input.GetKey(KeyCode.Mouse0) == false)
            {
                chargeTimerDelta = chargeTimerDelay;
                float currentPercentage = (float)currentAmmo / maxAmmo;
            }
        }

    }
}
