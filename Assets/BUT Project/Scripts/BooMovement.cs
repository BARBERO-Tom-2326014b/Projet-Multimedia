using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BooMovement : MonoBehaviour
{
    public enum AxeDeplacement { X, Z }

    [Header("Déplacement horizontal (aller-retour)")]
    public float vitesse = 2f;
    public float distanceMax = 5f;
    public AxeDeplacement axe = AxeDeplacement.X;

    [Header("Vol (oscillation verticale)")]
    [Tooltip("Hauteur moyenne du vol par rapport à la position de départ.")]
    public float hauteurBase = 1.0f;

    [Tooltip("Amplitude de l'oscillation (0.3 = monte/descend de ±0.3).")]
    public float amplitude = 0.3f;

    [Tooltip("Vitesse de l'oscillation verticale.")]
    public float frequence = 1.5f;

    [Header("Obstacles (optionnel)")]
    public LayerMask obstacleLayers;
    public float distanceDetectionObstacle = 0.5f;

    [Header("Visuel")]
    public bool tournerModele = true;

    private Rigidbody rb;
    private int direction = 1;

    // bornes horizontal
    private float minAxe;
    private float maxAxe;

    // référence verticale
    private float yDepart;
    private float tempsDepart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.isKinematic = true;
        rb.useGravity = false;

        Vector3 startPos = rb.position;

        float startAxe = (axe == AxeDeplacement.X) ? startPos.x : startPos.z;
        minAxe = startAxe - distanceMax;
        maxAxe = startAxe + distanceMax;

        yDepart = startPos.y;
        tempsDepart = Time.time;

        AppliquerRotation();
    }

    private void FixedUpdate()
    {
        // Direction horizontale
        Vector3 forward = (axe == AxeDeplacement.X) ? Vector3.right : Vector3.forward;
        forward *= direction;

        // Demi-tour si obstacle devant
        if (obstacleLayers.value != 0 && DetecterObstacleDevant(forward))
        {
            ChangerDeSens();
            forward = ((axe == AxeDeplacement.X) ? Vector3.right : Vector3.forward) * direction;
        }

        // Calcul prochaine position horizontale
        Vector3 nextPos = rb.position + forward * vitesse * Time.fixedDeltaTime;

        // Clamp + demi-tour sur bornes
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

        // Oscillation verticale (vol)
        float t = (Time.time - tempsDepart) * frequence;
        float yCible = yDepart + hauteurBase + Mathf.Sin(t) * amplitude;
        nextPos.y = yCible;

        rb.MovePosition(nextPos);
    }

    private bool DetecterObstacleDevant(Vector3 forward)
    {
        // Ray à mi-hauteur pour éviter de "voir" le sol
        Vector3 origin = rb.position + Vector3.up * 0.5f;
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