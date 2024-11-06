using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project002
{
    public class EnemyController : MonoBehaviour
    {
        enum AIState
        {
            Idle,
            Patrolling,
            Chasing,
            Attacking
        }

        [Header("Patrol")]
        [SerializeField] private Transform wayPoints;
        public float waitAtPoint = 2f;
        private int currentWayPoint;
        [SerializeField] private float waitCounter;

        [Header("Components")]
        private NavMeshAgent agent;

        [Header("AIState")]
        private AIState currentState;

        [Header("Chasing")]
        public float chaseRange;

        public float suspiciousTime = 1f;
        public float timeSinceLastSawPlayer;

        [Header("Attacking")]
        public float attackRange = 1f;
        public float attackTime = 2f;
        public float timeToAttack;

        //private List<Transform> enemyWayPoints = new List<Transform>();
        private GameObject player;
        private Animator anim;

        //private float distanceToPlayer;

        private void Awake()
        {           
            //distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponentInChildren<Animator>();

            waitCounter = waitAtPoint;
            timeSinceLastSawPlayer = suspiciousTime;
            timeToAttack = 0.5f;
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            switch (currentState)
            {
                case AIState.Idle:
                    enemyIdleState();
                    break;

                case AIState.Patrolling:
                    enemyPatrollingState();
                    break;

                case AIState.Chasing:
                    enemyChasingState();
                    break;

                case AIState.Attacking:
                    enemyAttackingState();
                    break;
            }
        }

    

        private void enemyIdleState()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            anim.SetBool("isWalking", false);

            // 기다림시간(waitCounter)가 0 보다 크면 시간 감소. patrolling 상태로 전환.
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
            }
            else
            {
                currentState = AIState.Patrolling;
                anim.SetBool("isWalking", true);
                agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
            }
            if (distanceToPlayer <= chaseRange)
            {
                currentState = AIState.Chasing;
                anim.SetBool("isWalking", true);
            }
        }

        private void enemyPatrollingState()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // 다음 목적지까지의 거리가 0.2 보다 작으면 목적지(currentWayPoint) 증가, idle 상태로 전환
            if (agent.remainingDistance <= 0.2f)
            {
                currentWayPoint++;
                if (currentWayPoint >= wayPoints.childCount)
                {
                    currentWayPoint = 0;
                }
                currentState = AIState.Idle;
                anim.SetBool("isWalking", false);
                waitCounter = waitAtPoint;
            }
            if (distanceToPlayer <= chaseRange)
            {
                
                currentState = AIState.Chasing;
                anim.SetBool("isWalking", true);
            }
        }

        private void enemyChasingState()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // 플레이어의 위치로 이동
            agent.SetDestination(player.transform.position);
            

            if (distanceToPlayer > chaseRange)              // 추격범위보다 플레이어가 멀어지면 
            {
                agent.isStopped = true;                     // 기존경로 중지 (추격중지)
                agent.velocity = Vector3.zero;              // 속도 0으로 설정
                timeSinceLastSawPlayer -= Time.deltaTime;   // 플레이어 목격 타이머 감소
                anim.SetBool("isWalking", false);

                if (timeSinceLastSawPlayer <= 0)
                {
                    waitCounter = 0;
                    currentState = AIState.Idle;            // Idle 로 돌아가면 다시 원래의 위치로 돌아가서 순찰모드 on
                    timeSinceLastSawPlayer = suspiciousTime;
                    anim.SetBool("isWalking", true);
                    agent.isStopped = false;                // 다시 움직임
                }              
            }
            if(distanceToPlayer < attackRange)
            {
                currentState = AIState.Attacking;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
            }         
        }

        private void enemyAttackingState()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            transform.LookAt(player.transform.position, Vector3.up);
            timeToAttack -= Time.deltaTime;

            if(timeToAttack <= 0)
            {
                anim.SetTrigger("Attack");
                timeToAttack = attackTime;
            }
            if(distanceToPlayer >= attackRange)
            {
                currentState = AIState.Chasing;
                agent.isStopped = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

    }
}
