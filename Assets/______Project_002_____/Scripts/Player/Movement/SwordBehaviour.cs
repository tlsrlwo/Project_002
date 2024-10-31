using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project002
{
    public class SwordBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 스크립트 -> animator의 locomotion에 추가됨
            var controller = animator.transform.root.GetComponent<PlayerController>();
            controller.comboCount = 0;
            animator.SetInteger("ComboCount", 0);
        }

    }
}