using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GoombaMovement : MonoBehaviour
{
    public enum AxeDeplacement { X, Z }

    [Header("Déplacement")]
    public float vitesse = 2f;
    public float distanceMax = 5f;
    public AxeDeplacement axe = AxeDeplacement.X;

    [Header("Obstacles (optionnel)")]
    public LayerMask obstacleLayers;
    public float distanceDetectionObstacle = 0.5f;

    [Header("Visuel")]
    public bool tournerModele = true;

    private Rigidbody rb;
    private int direction = 1;

    // bornes
    private float minAxe;
    private float maxAxe;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Si tu es en kinematic, le mouvement se fait via MovePosition
        // Si tu veux rester en dynamique, mets rb.isKinematic = false et adapte.
        rb.isKinematic = true;
        rb.useGravity = false;

        float startAxe = (axe == AxeDeplacement.X) ? rb.position.x : rb.position.z;
        minAxe = startAxe - distanceMax;
        maxAxe = startAxe + distanceMax;

        AppliquerRotation();
    }

    private void FixedUpdate()
    {
        Vector3 forward = (axe == AxeDeplacement.X) ? Vector3.right : Vector3.forward;
        forward *= direction;

        // Demi-tour si obstacle devant (si tu utilises obstacleLayers)
        if (obstacleLayers.value != 0 && DetecterObstacleDevant(forward))
        {
            ChangerDeSens();
            forward = ((axe == AxeDeplacement.X) ? Vector3.right : Vector3.forward) * direction;
        }

        Vector3 nextPos = rb.position + forward * vitesse * Time.fixedDeltaTime;

        // Clamp + demi-tour sur bornes (plus de blocage)
        if (axe == AxeDeplacement.X)
        {
            if (nextPos.x <= minAxe)
            {
                nextPos.x = minAxe;
                direction = 1;
                AppliquerRotation();
            }
            else if (nextPos.x >= maxAxe)
            {
                nextPos.x = maxAxe;
                direction = -1;
                AppliquerRotation();
            }
        }
        else // Z
        {
            if (nextPos.z <= minAxe)
            {
                nextPos.z = minAxe;
                direction = 1;
                AppliquerRotation();
            }
            else if (nextPos.z >= maxAxe)
            {
                nextPos.z = maxAxe;
                direction = -1;
                AppliquerRotation();
            }
        }

        rb.MovePosition(nextPos);
    }

    private bool DetecterObstacleDevant(Vector3 forward)
    {
        Vector3 origin = rb.position + Vector3.up * 0.5f; // évite de "voir" le sol
        return Physics.Raycast(origin, forward, distanceDetectionObstacle, obstacleLayers, QueryTriggerInteraction.Ignore);
    }

    private void ChangerDeSens()
    {
        direction *= -1;
        AppliquerRotation();
    }

    private void AppliquerRotation()
    {
        if (!tournerModele) return;

        float yaw = (direction == 1) ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}