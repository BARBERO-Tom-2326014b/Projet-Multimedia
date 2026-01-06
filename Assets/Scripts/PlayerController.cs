using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform cameraTransform;      // Assigné automatiquement si laissé vide
    [SerializeField] private OrbitCamera orbitCamera;        // Script caméra (SetLookInput)
    [SerializeField] private Animator animator;              // Optionnel: animations

    [Header("Déplacement")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float rotationSmoothing = 12f;

    [Header("Saut / Gravité")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool sprintHeld;
    private bool jumpPressed;
    private float verticalVel;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (orbitCamera == null && Camera.main != null)
            orbitCamera = Camera.main.GetComponent<OrbitCamera>();
    }

    // Méthodes appelées automatiquement par PlayerInput (Send Messages)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        sprintHeld = value.Get<float>() > 0.5f;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed) jumpPressed = true;
    }

    private void Update()
    {
        // Donner l'input de look à la caméra
        if (orbitCamera != null)
            orbitCamera.SetLookInput(lookInput);

        bool grounded = controller.isGrounded;
        if (grounded && verticalVel < 0f)
            verticalVel = -2f; // maintient un léger contact avec le sol

        // Direction relative à la caméra (ignore Y)
        Vector3 camFwd = cameraTransform ? Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized : Vector3.forward;
        Vector3 camRight = cameraTransform ? Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized : Vector3.right;

        Vector3 moveDir = camFwd * moveInput.y + camRight * moveInput.x;
        float targetSpeed = (sprintHeld ? sprintSpeed : walkSpeed) * Mathf.Clamp01(moveDir.magnitude);
        Vector3 horizontal = moveDir.sqrMagnitude > 0.0001f ? moveDir.normalized * targetSpeed : Vector3.zero;

        // Rotation vers la direction de déplacement
        if (horizontal.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(horizontal.x, horizontal.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSmoothing * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        // Saut
        if (jumpPressed && grounded)
            verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
        jumpPressed = false;

        // Gravité
        verticalVel += gravity * Time.deltaTime;

        Vector3 velocity = horizontal + Vector3.up * verticalVel;
        controller.Move(velocity * Time.deltaTime);

        // Animation (utilise Speed, IsGrounded, Move)
        if (animator)
        {
            float planarSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
            animator.SetFloat("Speed", planarSpeed);          // Float existant dans ton Animator
            animator.SetBool("IsGrounded", grounded);         // Bool existant (pas "Grounded")
            bool moving = planarSpeed > 0.1f;                 // Seuil ajustable pour passer Idle -> Walking
            animator.SetBool("Move", moving);                 // Bool existant (si utilisé par transitions)
        }
    }
}