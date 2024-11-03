using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{

    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; set; }

        private Weapon weapon;
        public GameObject gunWeapon;
        public bool hasWeapon;


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

            // ¹«±â
            var weaponGameObject = TransformUtility.FindGameObjectWithTag(gunWeapon, "Weapon");
            weapon = weaponGameObject.GetComponent<Weapon>();

            if(weapon)
            {
                EquipWeapon(weapon);
                
            }
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            weapon = newWeapon;
            hasWeapon = true;
        }

    }
}