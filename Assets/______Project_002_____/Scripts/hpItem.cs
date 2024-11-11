using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class hpItem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // 플레이어 hp가 3보다 적으면 hp증가, 오브젝트 삭제
                if(PlayerHealth.Instance.currentHealth < PlayerHealth.Instance.maxHealth)
                {
                    PlayerHealth.Instance.currentHealth++;
                    Destroy(transform.gameObject);
                }
                
                // 플레이어 hp가 3이거나 3보다 크면 먹어도 3에서 증가하지 않도록, 오브젝트 삭제 안됨
                if(PlayerHealth.Instance.currentHealth >= PlayerHealth.Instance.maxHealth)
                {
                    PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
                }
                
            }
        }
    }
}
