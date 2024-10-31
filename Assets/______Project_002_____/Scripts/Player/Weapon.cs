using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class Weapon : MonoBehaviour
    {
        public GameObject muzzleFlash;
        public Transform firePosition;
        public Transform bulletPrefab;

        public bool isFiring = false;
            

        public void Shoot()
        {
            isFiring = true;
        }
    }
}
