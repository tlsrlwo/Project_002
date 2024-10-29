using RPGCharacterAnims.Lookups;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region References
    CharacterController controller;
    Camera mainCamera;
    [HideInInspector] public Animator anim;    

    // �̵�
    [Header("Movement")]
    public float moveSpeed = 3.0f;
    public float sprintSpeed = 5.0f;
    public float speedChangeRate = 10.0f;        
    [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;

    // ī�޶�
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

    [Header("Weapon")]
    public GameObject weapon1;

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
    private bool isGrounded;
    private float JumpTimeout = 0.50f;       //Time required to pass before being able to jump again. Set to 0f to instantly jump again        
    private float FallTimeout = 0.15f;       //Time required to pass before entering the fall state. Useful for walking down stairs
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private bool isJump = false;

    

    #endregion
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;        
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;        

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }



    private void Update()
    {
        // �÷��̾� �̵� input��
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        move = new Vector2(horizontal, vertical);

        // �÷��̾��� �̵�(PlayerController)
        JumpAndGravity();
        GroundCheck();
        Movement();
        ChangeState();

        if (characterState == 1)
        {
            weapon1.SetActive(true);

            //����
            if (comboCount == 0 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetTrigger("Sword_Attack");
                comboCount++;                
            }    
        }
        else
        {
            weapon1.SetActive(false);
        }

        anim.SetFloat("Speed", animationBlend);
        anim.SetFloat("hInput", move.x);
        anim.SetFloat("vInput", move.y);
        anim.SetFloat("Strafe", isStrafe ? 1 : 0);        
        anim.SetBool("Grounded", isGrounded);      
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Movement()
    {       

        // isSprint�� ���� ���ǵ� ��ȭ
        float targetSpeed = isSprint ? sprintSpeed : moveSpeed;

        // ��ǲ���� ������ targetSpeed�� 0���� ����
        if (move == Vector2.zero) targetSpeed = 0.0f;

        // �÷��̾��� ���� �̵��ӵ��� (������ ����)
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = move.magnitude;

        // targetSpeed���� ���� ���Ӱ� ���� ( �ڿ������� �ӵ� ��ȭ�� ���� )
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // �������� ������ٴ� ����� ����� �����Ͽ� �� �������� �ӵ� ��ȭ�� �����մϴ�.               
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

            // �ӵ��� �Ҽ��� 3�ڸ����� �ݿø�
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

            if (!isStrafe)
            {
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // Controller �̵�
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
        isStrafe = Input.GetKey(KeyCode.Mouse1);

        if (isStrafe)
        {
            Vector3 cameraForward = Camera.main.transform.forward.normalized;
            cameraForward.y = 0;
            transform.forward = cameraForward;      // ĳ������ ������ ī�޶��� �� �������� ����
        }

    }
    private void JumpAndGravity()
    {
        isJump = Input.GetKeyDown(KeyCode.Space);

        if (isGrounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // �ִϸ��̼�
            
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
        // ī�޶� input��
        float hMouse = Input.GetAxis("Mouse X");
        float vMouse = Input.GetAxis("Mouse Y") * -1; //-1�� ���ϴ� ������ ���Ϲ����� ����Ŵ(���콺 ���Ʒ� ������)
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
        // �÷��̾� �߹ٴ� ��ġ
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);

        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers);  //QueryTriggerInteraction.Ignore        
    }

 
  private void ChangeState()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            characterState = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            characterState = 0;
        }

    }

    



    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Gizmos.color = transparentGreen;
                
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }

}
