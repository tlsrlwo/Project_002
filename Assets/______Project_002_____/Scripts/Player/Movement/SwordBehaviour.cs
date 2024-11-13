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
            PlayerState playerState = animator.transform.root.GetComponent<PlayerState>();
            //PlayerController controller = animator.transform.root.GetComponent<PlayerController>();
            playerState.attackComboCount = 0;
            playerState.isSwordAttacking = false;
            animator.SetInteger("ComboCount", 0);

            //controller.IsEnableMovement = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerState playerState = animator.transform.root.GetComponent<PlayerState>();
            //PlayerController controller = animator.transform.root.GetComponent<PlayerController>();

            //controller.IsEnableMovement = false;
        }

    }
}