using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform target;          // CameraTarget (enfant du Player) ou Player
    [Header("Paramètres d'Orbite")]
    [SerializeField] private float distance = 5f;       // Distance derrière le joueur
    [SerializeField] private float heightOffset = 0.0f; // Décalage vertical supplémentaire
    [SerializeField] private Vector2 pitchLimits = new Vector2(-20f, 60f);
    [SerializeField] private float sensX = 180f;
    [SerializeField] private float sensY = 140f;
    [SerializeField] private bool lockCursor = false;

    private float yaw;
    private float pitch;
    private Vector2 lookInput;

    public void SetLookInput(Vector2 look) => lookInput = look;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var camT = player.transform.Find("CameraTarget");
                target = camT != null ? camT : player.transform;
            }
        }

        if (target != null)
            yaw = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (!target) return;

        // Mettre à jour angles
        yaw   += lookInput.x * sensX * Time.deltaTime;
        pitch -= lookInput.y * sensY * Time.deltaTime;
        pitch  = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 focusPoint = target.position + Vector3.up * heightOffset;
        Vector3 camPos = focusPoint - rot * Vector3.forward * distance;

        transform.SetPositionAndRotation(camPos, rot);
    }
}