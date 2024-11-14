using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;



namespace Project002
{
    public class LevelOnePlayerController : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(aimTarget.position, 0.1f);
        }

        public bool IsGrounded
        {
            set => isGrounded = value;
        }


        #region References
        CharacterController controller;
        Camera mainCamera;
        [HideInInspector] public Animator anim;

        // 이동
        [Header("Movement")]
        public float moveSpeed = 3.0f;
        public float sprintSpeed = 5.0f;
        public float speedChangeRate = 10.0f;
        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;

        // 카메라
        [Header("Camera")]
        public float cameraAngleOverride = 0.0f;
        public GameObject cinemachineCameraTarget;

        // Animation
        [Header("Animation Related")]
        public int comboCount = 0;
        public int characterState = 0;

        // Jump & Gravity    
        [Header("Jump Gravity")]
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;
        public float Gravity = -15.0f;
        public float JumpHeight = 1.2f;

        [Header("Weapon Aiming")]
        public Transform aimTarget;
        public LayerMask aimingLayer;

        // Strafe
        private bool isStrafe;

        // Player Movement
        private bool isSprint = false;
        private Vector2 move;
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;

        private bool isEnableMovement = true;

        // Camera
        private Vector2 look;
        private const float _threshold = 0.01f;
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;
        private float cameraHorizontalSpeed = 2.0f;
        private float cameraVerticalSpeed = 2.0f;
        // Camera Clamping
        private float topClamp = 70.0f;
        private float bottomClamp = -30.0f;

        //Gravity
        public bool isGrounded;
        private float JumpTimeout = 0.50f;       //Time required to pass before being able to jump again. Set to 0f to instantly jump again        
        private float FallTimeout = 0.15f;       //Time required to pass before entering the fall state. Useful for walking down stairs
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private bool isJump = false;

        #endregion

        private void Awake()
        {
            // 컴포넌트
            controller = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            mainCamera = Camera.main;

        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 점프 관련 타이머 초기화
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

        }


        private void Update()
        {
            // 플레이어 이동 input값
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            move = new Vector2(horizontal, vertical);

            anim.SetFloat("Speed", animationBlend);
            anim.SetFloat("hInput", move.x);
            anim.SetFloat("vInput", move.y);
            anim.SetFloat("Strafe", isStrafe ? 1 : 0);
            anim.SetBool("Grounded", isGrounded);

            if (!PlayerState.Instance.isSwordAttacking && !LevelOneTutorialCanvas.Instance.isOpen)
            {
                Movement();
            }
            JumpAndGravity();
            GroundCheck();
        }

        private void LateUpdate()
        {
            if (!LevelOneTutorialCanvas.Instance.isOpen)
            {
                CameraRotation();
            }
        }

        private void Movement()
        {
            // isSprint에 따라 스피드 변화
            float targetSpeed = isSprint ? sprintSpeed : moveSpeed;

            // 인풋값이 없으면 targetSpeed를 0으로 설정
            if (move == Vector2.zero) targetSpeed = 0.0f;

            // 플레이어의 수평 이동속도값 (점프가 수직)
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = move.magnitude;

            // targetSpeed값에 맞춰 감속과 가속 ( 자연스러운 속도 변화를 위한 )
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 선형적인 결과보다는 곡선적인 결과를 생성하여 더 유기적인 속도 변화를 제공합니다.               
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

                // 속도를 소숫점 3자리까지 반올림
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }


            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

            if (move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                if (!isStrafe || PlayerState.Instance.characterState == 2)
                {
                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // Controller 이동
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            isSprint = Input.GetKey(KeyCode.LeftShift);
            if (isSprint)
            {
                anim.SetFloat("MotionSpeed", 1.2f);
            }
            else
            {
                anim.SetFloat("MotionSpeed", 1f);
            }

            // strafe
            isStrafe = Input.GetKey(KeyCode.Mouse1);
            if (PlayerState.Instance.characterState != 2)
            {
                if (isStrafe)
                {
                    Vector3 cameraForward = Camera.main.transform.forward.normalized;
                    cameraForward.y = 0;
                    transform.forward = cameraForward;      // 캐릭터의 방향을 카메라의 앞 방향으로 고정

                    WeaponAiming();
                }
            }
        }
        private void JumpAndGravity()
        {
            isJump = Input.GetKeyDown(KeyCode.Space);

            if (isGrounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // 애니메이션

                anim.SetBool("Jump", false);
                anim.SetBool("Falling", false);


                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }

                // Jump
                if (isJump && _jumpTimeoutDelta <= 0.0f)
                {
                    verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    anim.SetBool("Jump", true);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    anim.SetBool("Falling", true);
                    anim.SetBool("Grounded", true);
                }

                // if we are not grounded, do not jump
                //isJump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void CameraRotation()
        {
            // 카메라 input값
            float hMouse = Input.GetAxis("Mouse X");
            float vMouse = Input.GetAxis("Mouse Y") * -1; //-1을 곱하는 이유는 상하반전을 일으킴(마우스 위아래 움직임)
            look = new Vector2(hMouse, vMouse);

            // if there is an input and camera position is not fixed
            if (look.sqrMagnitude >= _threshold)
            {
                // Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;

                cinemachineTargetYaw += look.x * deltaTimeMultiplier * cameraHorizontalSpeed;
                cinemachineTargetPitch += look.y * deltaTimeMultiplier * cameraVerticalSpeed;
            }

            // clamp our rotations so our values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride, cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void GroundCheck()
        {
            // 플레이어 발바닥 위치
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);

            isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers);  //QueryTriggerInteraction.Ignore        
        }


        private void WeaponAiming()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 aimingTargetPosition = Camera.main.transform.position + Camera.main.transform.forward * 1000f;

            // Raycast 가 성공하면 aimingTargetPosition 을 Raycast가 성공한 지점으로 설정
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, aimingLayer))
            {
                if (hitInfo.transform.root != transform)
                {
                    aimingTargetPosition = hitInfo.point;
                }
            }

            // aimingTargetPosition을 aimTarget의 위치로 설정
            aimTarget.position = aimingTargetPosition;
        }

        private bool ShouldFire()
        {
            if (WeaponAmmo.Instance.currentAmmo == 0)
            {
                return false;
            }

            return true;
        }


        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Gizmos.color = transparentGreen;

            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

    }
}