using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project002
{
    public class PlayerHealth : MonoBehaviour
    {
        public static PlayerHealth Instance { get; set; }

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

        public int currentHealth;
        public int maxHealth = 3;      
        
        // UI
        public Image hpImage;
        public TextMeshProUGUI text;

        // Life Sprite
        public Sprite fullHp;
        public Sprite normalHp;
        public Sprite tiredHp;
        public Sprite deadHp;


        private void Start()
        {
            currentHealth = maxHealth;           
        }

        private void Update()
        {
            if(currentHealth == 3)
            {
                hpImage.sprite = fullHp;
            }
            if (currentHealth == 2)
            {
                hpImage.sprite = normalHp;
            }
            if (currentHealth == 1)
            {
                hpImage.sprite = tiredHp;
            }
            if(currentHealth <= 0)
            {
                hpImage.sprite = deadHp;
            }
            text.text = currentHealth.ToString();
        }

        public bool IsDead()
        {
            if (currentHealth <= 0)
            {
                return true;
            }


            return false;
        }
       
    }
}
