using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project002
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        // Animations
        [Header("Animations")]
        Animator anim;
        private float horizontal, vertical;
        public int characterState = 0;

        // NavMesh
        [Header("NavMeshAgent")]
        private NavMeshAgent nav;
        private float roamTimer;
        public float maxRoamDistance;

        // Start is called before the first frame update
        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            nav = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            Animations();
            Roam();
        }

        private void Roam()
        {
            if (characterState != 0) return;

            if (Time.time > roamTimer)
            {
                float a = Random.Range(0, 2);

                roamTimer = Time.time + 20;     // 20초짜리 타이머 생성
                nav.SetDestination(new Vector3(transform.position.x + Random.Range(maxRoamDistance / 2, maxRoamDistance) * (a == 1 ? 1 : -1), 0,
                    transform.position.z + Random.Range(maxRoamDistance / 2, maxRoamDistance) * (a == 1 ? 1 : -1)));
            }

        }

        private void Animations()
        {
            anim.SetFloat("hInput", horizontal);
            anim.SetFloat("vInput", vertical);
            anim.SetInteger("State", characterState);
        }

        private string GetCharState()
        {
            switch (characterState)
            {
                case 0:
                    return "Peaceful";
                    break;
                case 1:
                    return "Combat";
                    break;

            }
            return null;
        }

    }
}