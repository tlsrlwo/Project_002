using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class Weapon : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePosition.position, aimPosition.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * range);
        }


        // รั ฐüทร
        public ParticleSystem muzzleFlash;
        public Transform firePosition;
        public Transform aimPosition;

        public Transform bulletPrefab;
        public float range = 100f;

        public bool isFiring = false;

        private Ray ray;
        private RaycastHit hitInfo;


        public void Shoot()
        {
            isFiring = true;
            muzzleFlash.Emit(1);

            ray.origin = firePosition.position;
            ray.direction = aimPosition.position - firePosition.position;

            if(Physics.Raycast(ray, out hitInfo, range))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            }
        }
   

    }
}
