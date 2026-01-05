using UnityEngine;

public class PorteTeleporteur : MonoBehaviour
{
    [Header("Destination (l'autre porte)")]
    public Transform destinationDoor;

    [Header("Distance d'interaction")]
    public float interactionDistance = 3f;

    [Header("Décalage (local) à l'arrivée")]
    [Tooltip("Décalage appliqué dans le repère de la porte de destination (ex: z=2 => 2m devant destinationDoor)")]
    public Vector3 spawnOffset = new Vector3(0f, 0f, 2f);

    [Header("Touche pour activer")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Anti spam")]
    public float cooldownSeconds = 0.5f;

    [Header("Debug")]
    public bool debugLogs = false;

    private Transform player;
    private CharacterController playerController;
    private static float s_nextAllowedTime = 0f;
    private static int s_lastTeleportFrame = -1;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
        {
            Debug.LogError("[PorteTeleporteur] Aucun objet avec le tag 'Player' trouvé !");
            return;
        }

        player = p.transform.root;

        playerController = player.GetComponent<CharacterController>();
        if (playerController == null)
        {
            playerController = p.GetComponent<CharacterController>();
            if (playerController != null) player = p.transform;
        }

        Debug.Log("[PorteTeleporteur] Joueur trouvé : " + player.name);
    }

    void Update()
    {
        if (player == null || destinationDoor == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (Time.time < s_nextAllowedTime) return;
        if (s_lastTeleportFrame == Time.frameCount) return;

        if (dist <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            s_lastTeleportFrame = Time.frameCount;
            s_nextAllowedTime = Time.time + cooldownSeconds;
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        Vector3 targetPos = destinationDoor.TransformPoint(spawnOffset);

        if (debugLogs)
        {
            Debug.Log($"[PorteTeleporteur] '{name}' -> '{destinationDoor.name}' | destPos={destinationDoor.position} | targetPos={targetPos}");
        }

        if (playerController != null)
        {
            playerController.enabled = false;
            player.position = targetPos;
            playerController.enabled = true;
        }
        else
        {
            player.position = targetPos;
        }

        if (debugLogs)
        {
            Debug.Log($"[PorteTeleporteur] Téléporté vers {player.position}");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
