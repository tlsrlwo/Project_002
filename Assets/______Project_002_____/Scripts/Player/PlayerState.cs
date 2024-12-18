using Project002;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Project002
{
    public class PlayerState : MonoBehaviour
    {
        public static PlayerState Instance { get; set; }

        public int characterState;

        private Animator anim;

        [Header("camera FOV")]                  // 카메라 fov
        public float defaultFOV;
        public float aimFOV;

        [Header("Rig")]                         // Rig값
        public Rig defaultWeaponPoseRig;
        public Rig AimingRig;
        public Rig swordRigLayer;

        [Header("Weapon")]
        public GameObject gunWeapon;
        public GameObject swordWeapon;
        private Weapon currentWeapon;

        [Header("Canvas")]
        public GameObject weaponAmmoCanvas;

        public int attackComboCount = 0;
        public bool isSwordAttacking;


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

            var weaponGameObject = TransformUtility.FindGameObjectWithTag(gunWeapon, "Weapon");
            currentWeapon = weaponGameObject.GetComponent<Weapon>();

            gunWeapon.gameObject.SetActive(false);
        }

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            defaultWeaponPoseRig.weight = 0;
            AimingRig.weight = 0;
            
        }

        private void Update()
        {
            RigControlByState();
            ChangeState();
            weaponAmmoCanvas.gameObject.SetActive(characterState == 1);     // 캐릭터 스테이트가 1 일 때만 총알 UI 화면에 표시 

            SwordAttack();
        }

        void RigControlByState()
        {
            if (characterState == 1 && Input.GetKey(KeyCode.Mouse1))
            {
                defaultWeaponPoseRig.weight = 0;
                AimingRig.weight = 1;
                anim.SetBool("isAiming", true);
                //aimCanvas.gameObject.SetActive(true);

                //zoom in
                CameraManager.Instance.TargetFOV = aimFOV;
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (ShouldFire()) currentWeapon.Shoot();
                }
                else
                {
                    currentWeapon.isFiring = false;
                    WeaponAmmo.Instance.AmmoUpdateTimer();
                }

            }
            if (characterState == 1 && Input.GetKeyUp(KeyCode.Mouse1))
            {
                currentWeapon.isFiring = false;
                defaultWeaponPoseRig.weight = 1;
                AimingRig.weight = 0;
                anim.SetBool("isAiming", false);
                //aimCanvas.gameObject.SetActive(false);                

                CameraManager.Instance.TargetFOV = defaultFOV;
            }
        }

        private void SwordAttack()
        {
            PlayerController controller = transform.GetComponent<PlayerController>();

            if(controller.isGrounded && characterState == 2 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                isSwordAttacking = true;

                if(attackComboCount == 0)
                {
                    anim.SetTrigger("Trigger_Attack");
                    attackComboCount++;
                }
                else
                {
                    attackComboCount++;
                    anim.SetInteger("ComboCount", attackComboCount);
                }
            }
        }


        private void ChangeState()
        {
            if (characterState == 0)
            {
                // 무기없는 상태. 총 애니메이션 layerweight 0. 무기자세ik 0 
                anim.SetLayerWeight(1, 0f);
                anim.SetLayerWeight(2, 0f);
                defaultWeaponPoseRig.weight = 0;
                swordRigLayer.weight = 0;
                gunWeapon.gameObject.SetActive(false);
                swordWeapon.gameObject.SetActive(false);

            }
            if (characterState == 1)
            {
                // 총 상태. 총 애니메이션 layerweight 1, 무기자세 1
                anim.SetLayerWeight(1, 1f);
                anim.SetLayerWeight(2, 0f);
                defaultWeaponPoseRig.weight = 1;
                swordRigLayer.weight = 0;
                gunWeapon.gameObject.SetActive(true);
                swordWeapon.gameObject.SetActive(false);

            }
            if(characterState == 2)
            {
                // 검 상태
                anim.SetLayerWeight(2, 1f);
                anim.SetLayerWeight(1, 0f);
                defaultWeaponPoseRig.weight = 0;
                swordRigLayer.weight = 1;
                swordWeapon.gameObject.SetActive(true);
                gunWeapon.gameObject.SetActive(false);
            }

        }

        private bool ShouldFire()
        {
            if (WeaponAmmo.Instance.currentAmmo == 0)
            {
                return false;
            }

            return true;
        }

    }
}
