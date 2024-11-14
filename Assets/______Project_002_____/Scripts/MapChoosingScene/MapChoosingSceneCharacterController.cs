using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project002
{
    public class MapChoosingSceneCharacterController : MonoBehaviour
    {
        // References
        private Vector2 move;
        public float moveSpeed;
        public float speed;
        private float speedChangeRate;

        private float animationBlend;

        private float targetRotation;
        private float rotationVelocity;
        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;


        // GroundCheck
        [SerializeField] float groundYOffset;
        [SerializeField] LayerMask groundMask;
        Vector3 spherePos;
        
        // Gravity
        [SerializeField] float gravity = -9.81f;
        [SerializeField] float jumpForce = 10;
        [HideInInspector] public bool jumped;
        Vector3 velocity;

        public Transform levelList;
        public List<Transform> levelEntrance = new List<Transform>();

        private CharacterController controller;
        private Camera mainCamera;
        private Animator anim;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            anim = GetComponentInChildren<Animator>(); 
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            Movement();
            Gravity();
        }


        private void Movement()
        {
            // �÷��̾� �̵� input��
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            move = new Vector2(horizontal, vertical);


            // ��ǲ���� ������ targetSpeed�� 0���� ����
            if (move == Vector2.zero) moveSpeed = 0.0f;
            else moveSpeed = 4.5f;

            // �÷��̾��� ���� �̵��ӵ��� (������ ����)
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
                        
            float inputMagnitude = move.magnitude;

            float speedOffset = 0.1f;            

            // targetSpeed���� ���� ���Ӱ� ���� ( �ڿ������� �ӵ� ��ȭ�� ���� )
            if (currentHorizontalSpeed < moveSpeed - speedOffset ||
                currentHorizontalSpeed > moveSpeed + speedOffset)
            {
                // �������� ������ٴ� ����� ����� �����Ͽ� �� �������� �ӵ� ��ȭ�� �����մϴ�.               
                speed = Mathf.Lerp(currentHorizontalSpeed, moveSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

                // �ӵ��� �Ҽ��� 3�ڸ����� �ݿø�
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = moveSpeed;
            }
          
            // normalise input direction
            Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

            if (move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // Controller �̵�
            controller.Move(targetDirection.normalized * (moveSpeed * Time.deltaTime));
            // +new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            anim.SetFloat("Speed", moveSpeed);
        }

        public bool IsGrounded()
        {
            spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
            if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
            return false;
        }
        void Gravity()
        {
            if (!IsGrounded())
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else if (velocity.y < 0)
            {
                velocity.y = -2;
            }
            controller.Move(velocity * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
        }
    }
}
