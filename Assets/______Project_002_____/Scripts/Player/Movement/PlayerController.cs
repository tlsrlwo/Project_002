using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // �̵�
    [Header("Movement")]
    CharacterController controller;
    [HideInInspector] public Animator anim;

    public float moveSpeed = 3.0f;
    public float sprintSpeed = 5.0f;
    public float speedChangeRate = 10.0f;

    public float hInput, vInput;
    private bool isSprint = false;
    private Vector2 move;
    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;

    [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;


    // ī�޶�
    Camera mainCamera;
    private Vector2 look; 
    private const float _threshold = 0.01f;
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    public float cameraHorizontalSpeed = 2.0f;
    public float cameraVerticalSpeed = 2.0f;

    // Camera Clamping
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;
    public GameObject cinemachineCameraTarget;
    public float cameraAngleOverride = 0.0f;



    // Strafe
    private bool isStrafe;


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
    }

    private void Update()
    {
        // �÷��̾��� �̵�(PlayerController)
        Movement();

        CameraRotation();
      

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

        anim.SetFloat("Speed", animationBlend);
        anim.SetFloat("hInput", move.x);
        anim.SetFloat("vInput", move.y);
        anim.SetFloat("Strafe", isStrafe ? 1 : 0);

    }

    private void Movement()
    {
        // �÷��̾� �̵� input��
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        move = new Vector2(horizontal, vertical);

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


}
