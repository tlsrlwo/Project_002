using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project002
{
    public class AmmoCounterUI : MonoBehaviour
    {
        public Canvas ammoCounterCanvas;
        public Image fillImage;
        private Image fillBackground;

        public GameObject reloadingText;

        private float currentAmmo;
        private float maxAmmo;
        private float ammoFillImageSharpness = 10f;
                
        void Update()
        {
            currentAmmo = WeaponAmmo.Instance.currentAmmo;
            maxAmmo = WeaponAmmo.Instance.maxAmmo;

            float currentPercentage = currentAmmo / maxAmmo;

            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, currentPercentage, Time.deltaTime * ammoFillImageSharpness); //(float)currentPercentage; 

            if(WeaponAmmo.Instance.isCharging)
            {
                reloadingText.SetActive(true);
            }
            else
            {
                reloadingText.SetActive(false);
            }

        }
    }
}
