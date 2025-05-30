using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

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

        public bool isCharging;

        public float chargeTimerDelta = 0.0f;
        private float chargeTimerDelay = 2.0f;

        // Ammo Canvas 

        public Canvas ammoCanvas;
        private Image ammoFill;


        private void Start()
        {
            currentAmmo = maxAmmo;            
        }

        private void Update()
        {       
            if (!Weapon.Instance.isFiring && chargeTimerDelta > 0.0f && currentAmmo < maxAmmo)        // 쏘고 있지 않고, 마지막 발사 시간에서 2초가 지났고, 현재총알이 최대총알보다 적으면
            {
                chargeTimerDelta -= Time.deltaTime;
                
                if(Input.GetMouseButtonUp(1))
                {
                    isCharging = true;
                }
                if (chargeTimerDelta < 0.0f)
                {
                    StartCoroutine(WeaponChargingTime());
                }
            }
            if(currentAmmo >= maxAmmo)
            {
                isCharging = false;
            }

        }

        private void AmmoCanvasControl()
        {
            float currentPercentage = (float)currentAmmo / maxAmmo;
            ammoFill.fillAmount = 0.5f;            
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
