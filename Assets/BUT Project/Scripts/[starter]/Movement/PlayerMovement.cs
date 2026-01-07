using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BUT
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Movement m_Movement;

        float m_CurrentSpeed;
        public float CurrentSpeed
        {
            set
            {
                if (m_CurrentSpeed == value) return;
                m_CurrentSpeed = value;
                OnSpeedChange?.Invoke(m_CurrentSpeed);
            }
            get => m_CurrentSpeed;
        }

        bool m_IsSprinting;
        public bool IsSprinting { set => m_IsSprinting = value; get => m_IsSprinting; }

        private bool m_IsMoving;
        public bool IsMoving
        {
            set
            {
                if (m_IsMoving == value) return;
                m_IsMoving = value;
                OnMovingChange?.Invoke(m_IsMoving);
            }
            get => m_IsMoving;
        }

        private Vector3 m_Direction;
        public Vector3 Direction { set => m_Direction = value; get => m_Direction; }

        public Vector3 FullDirection => (GroundRotationOffset * Direction * CurrentSpeed + Vector3.up * GravityVelocity);

        private Quaternion m_GroundRotationOffset;
        public Quaternion GroundRotationOffset { set => m_GroundRotationOffset = value; get => m_GroundRotationOffset; }

        public const float GRAVITY = -9.31f;

        private float m_GravityVelocity;
        public float GravityVelocity { set => m_GravityVelocity = value; get => m_GravityVelocity; }

        private int m_JumpNumber;
        public int JumpNumber { set => m_JumpNumber = value; get => m_JumpNumber; }

        [SerializeField] float m_RayLenght;
        [SerializeField] LayerMask m_RayMask;

        [Header("Camera")]
        [SerializeField] private OrbitCamera m_OrbitCamera;

        // Look (souris) et Move (ZQSD)
        private Vector2 m_LookInput;
        private Vector2 m_MovementInput;

        RaycastHit m_Hit;

        private bool m_IsGrounded;
        public bool IsGrounded
        {
            set
            {
                if (IsGrounded == value) return;
                m_IsGrounded = value;
                OnGroundedChange?.Invoke(m_IsGrounded);
            }
            get => m_IsGrounded;
        }

        private CharacterController m_CharacterController;
        private Vector3 m_MovementDirection;

        public UnityEvent<float> OnSpeedChange;
        public UnityEvent<bool> OnMovingChange;
        public UnityEvent<bool> OnGroundedChange;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();

            // Auto-assign OrbitCamera
            if (m_OrbitCamera == null && Camera.main != null)
                m_OrbitCamera = Camera.main.GetComponent<OrbitCamera>();
        }

        private void OnDisable()
        {
            IsMoving = false;
        }

        private void OnEnable()
        {
            StartCoroutine(Moving());
        }

        // =========================
        // PlayerInput "Send Messages"
        // Les actions doivent s'appeler: Move, Look, Jump, Sprint
        // =========================
        public void OnMove(InputValue value)
        {
            m_MovementInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            m_LookInput = value.Get<Vector2>();
            // Debug temporaire si besoin :
            // Debug.Log($"Look input: {m_LookInput}");
        }

        public void OnSprint(InputValue value)
        {
            IsSprinting = value.Get<float>() > 0.5f;
        }

        public void OnJump(InputValue value)
        {
            if (!value.isPressed) return;

            if (!m_CharacterController.isGrounded && JumpNumber >= m_Movement.MaxJumpNumber) return;
            if (JumpNumber == 0) StartCoroutine(WaitForLanding());
            JumpNumber++;

            if (m_Movement.MinimazeJumpPower) GravityVelocity += m_Movement.JumpPower / JumpNumber;
            else GravityVelocity += m_Movement.JumpPower;
        }

        // =========================
        // (Optionnel) si tu utilises encore Invoke Unity Events quelque part
        // Tu peux garder ces méthodes, elles ne gênent pas.
        // =========================
        public void SetInputMove(InputAction.CallbackContext _context)
        {
            m_MovementInput = _context.ReadValue<Vector2>();
        }

        public void SetInputLook(InputAction.CallbackContext _context)
        {
            m_LookInput = _context.ReadValue<Vector2>();
            // Debug temporaire :
            // Debug.Log($"Look input: {m_LookInput}");
        }

        public void SetInputSprint(InputAction.CallbackContext _context)
        {
            IsSprinting = _context.started || _context.performed;
        }

        public void SetInputJump(InputAction.CallbackContext _context)
        {
            if (!_context.started || (!m_CharacterController.isGrounded && JumpNumber >= m_Movement.MaxJumpNumber)) return;
            if (JumpNumber == 0) StartCoroutine(WaitForLanding());
            JumpNumber++;

            if (m_Movement.MinimazeJumpPower) GravityVelocity += m_Movement.JumpPower / JumpNumber;
            else GravityVelocity += m_Movement.JumpPower;
        }

        IEnumerator Moving()
        {
            while (enabled)
            {
                if (m_MovementInput.magnitude > 0.1f)
                {
                    if (!IsMoving) IsMoving = true;
                    // clamp input magnitude
                    m_MovementInput = Vector3.ClampMagnitude(m_MovementInput, 1);
                }
                else if (IsMoving)
                {
                    IsMoving = false;
                }

                // Envoyer l'input souris à la caméra orbitale
                if (m_OrbitCamera != null)
                    m_OrbitCamera.SetLookInput(m_LookInput);

                ManageDirection();
                ManageGravity();
                if (IsMoving) ApplyRotation();
                ApplyMovement();

                yield return new WaitForFixedUpdate();
            }
        }

        IEnumerator WaitForLanding()
        {
            yield return new WaitUntil(() => !m_CharacterController.isGrounded);
            yield return new WaitUntil(() => m_CharacterController.isGrounded);
            JumpNumber = 0;
        }

        private void ManageDirection()
        {
            // Direction dans l'espace input (x = gauche/droite, y = avant/arrière)
            m_MovementDirection = new Vector3(m_MovementInput.x, 0f, m_MovementInput.y);

            if (Camera.main != null)
            {
                // Axes caméra projetés sur le plan horizontal
                Vector3 camForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
                Vector3 camRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

                // Direction relative à la caméra
                m_MovementDirection = camForward * m_MovementInput.y + camRight * m_MovementInput.x;
            }

            // Rester sur le plan horizontal
            m_MovementDirection.y = 0f;
            m_MovementDirection.Normalize();

            Debug.DrawRay(transform.position, -transform.up * m_RayLenght, Color.red);

            if (Physics.Raycast(transform.position, -transform.up, out m_Hit, m_RayLenght, m_RayMask))
            {
                IsGrounded = true;
                float angleOffset = Vector3.SignedAngle(transform.up, m_Hit.normal, transform.right);
                GroundRotationOffset = Quaternion.AngleAxis(angleOffset, transform.right);
                Debug.DrawRay(transform.position, GroundRotationOffset * m_MovementDirection, Color.green);
            }
            else
            {
                IsGrounded = m_CharacterController.isGrounded;
                GroundRotationOffset = Quaternion.identity;
            }

            Direction = m_MovementDirection;

            // calculate speed according to input force
            CurrentSpeed =
                (IsSprinting ? m_Movement.SprintFactor : 1f) *
                m_Movement.MaxSpeed *
                m_Movement.SpeedFactor.Evaluate(m_MovementInput.magnitude);
        }

        public void ApplyRotation()
        {
            if (!IsMoving) return;
            if (Direction.sqrMagnitude < 0.0001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(Direction, transform.up);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                m_Movement.MaxAngularSpeed * Mathf.Deg2Rad *
                m_Movement.AngularSpeedFactor.Evaluate(Direction.magnitude) * Time.deltaTime
            );
        }

        public void ApplyMovement()
        {
            Debug.DrawRay(transform.position, FullDirection, Color.yellow);
            m_CharacterController.Move(FullDirection * Time.deltaTime);
        }

        private void ManageGravity()
        {
            if (m_CharacterController.isGrounded && GravityVelocity < 0.0f)
            {
                GravityVelocity = -1;
            }
            else
            {
                GravityVelocity += GRAVITY * m_Movement.GravityMultiplier * Time.deltaTime;
            }
        }
    }
}