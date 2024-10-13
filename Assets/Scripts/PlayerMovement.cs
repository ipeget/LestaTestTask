using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    [Space]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Space]
    [SerializeField] private Transform mainCamera;

    [Space]
    [SerializeField] private Animator animator;

    private AreaSensor groundSensor;
    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float rotationSmoothTime = 0.06f;

    private bool OnGround => groundSensor.Trigger;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundSensor = new SphereSensor(groundCheckRadius, groundCheck, groundLayer);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        InputHandler();
        Jump();
        AnimationHandler();
        Rotation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (moveDirection != Vector3.zero)
        {
            Vector3 targetPosition = transform.position + moveSpeed * Time.fixedDeltaTime * moveDirection;
            rb.MovePosition(targetPosition);
        }
    }

    private void Rotation()
    {
        if (moveDirection != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float YRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0, YRotation, 0);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jumping");
        }
    }

    private void InputHandler()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = mainCamera.transform.forward.normalized;
        Vector3 right = mainCamera.transform.right.normalized;

        forward.y = 0;
        right.y = 0;

        moveDirection = forward * moveZ + right * moveX;
    }

    private void AnimationHandler()
    {
        animator.SetBool("OnGround", OnGround);
        animator.SetFloat("Speed", moveDirection.magnitude);

        if (!OnGround)
            animator.ResetTrigger("Jumping");
    }

}
