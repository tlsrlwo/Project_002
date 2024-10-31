using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{

    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; set; }

        public GameObject weapons;

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

    }
}