using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project002
{
    public class SwordBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // ��ũ��Ʈ -> animator�� locomotion�� �߰���
            var controller = animator.transform.root.GetComponent<PlayerController>();
            controller.comboCount = 0;
            animator.SetInteger("ComboCount", 0);
        }

    }
}