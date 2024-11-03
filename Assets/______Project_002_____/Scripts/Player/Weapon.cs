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

        public static Weapon Instance { get; set; }


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

        // 총 관련
        public ParticleSystem muzzleFlash;
        public ParticleSystem hitEffect;
        public Transform firePosition;
        public Transform aimPosition;
        public TrailRenderer tracerEffect;

        public LayerMask hitLayer;

        public float damage = 10;
        public float bulletVelocity;

        public Transform bulletPrefab;
        public float range = 1000f;

        public float fireRate = 0.1f;
        public float lastShootTime = 0;

        public bool isFiring = false;

        // Ray
        private Ray ray;
        private RaycastHit hitInfo;

      

        public void Shoot()
        {
            // 0.1초에 한발씩 
            if (Time.time > lastShootTime + fireRate)
            {
                lastShootTime = Time.time;

                WeaponAmmo.Instance.currentAmmo--;
                isFiring = true;
                muzzleFlash.Emit(1);

                ray.origin = firePosition.position;
                ray.direction = aimPosition.position - firePosition.position;

                // 총알 효과
                var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
                tracer.AddPosition(ray.origin);

                // 실제 총알
                var newBullet = Instantiate(bulletPrefab);

                var cameraTransfrom = Camera.main.transform;
                newBullet.transform.SetPositionAndRotation(cameraTransfrom.position, cameraTransfrom.rotation);
                newBullet.gameObject.SetActive(true);

                if (Physics.Raycast(ray, out hitInfo, range, hitLayer))
                {
                    //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
                    hitEffect.transform.position = hitInfo.point;
                    hitEffect.transform.forward = hitInfo.normal;
                    hitEffect.Emit(1);          // Instantiate 보다 Emit이 덜 expensive함

                    tracer.transform.position = hitInfo.point;
                }
            }
        } 

    




    }
}
