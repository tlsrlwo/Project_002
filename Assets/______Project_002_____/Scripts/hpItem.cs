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
                // �÷��̾� hp�� 3���� ������ hp����, ������Ʈ ����
                if(PlayerHealth.Instance.currentHealth < PlayerHealth.Instance.maxHealth)
                {
                    PlayerHealth.Instance.currentHealth++;
                    Destroy(transform.gameObject);
                }
                
                // �÷��̾� hp�� 3�̰ų� 3���� ũ�� �Ծ 3���� �������� �ʵ���, ������Ʈ ���� �ȵ�
                if(PlayerHealth.Instance.currentHealth >= PlayerHealth.Instance.maxHealth)
                {
                    PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
                }
                
            }
        }
    }
}
