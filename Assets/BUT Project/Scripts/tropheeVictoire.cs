using UnityEngine;

public class tropheeVictoire : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float vitesseRotation = 100f;

    // Axe de rotation en local (ex: (0,1,0) pour Y, (0,0,1) pour Z)
    [SerializeField] private Vector3 axeRotationLocal = Vector3.up;

    // Le visuel à faire tourner (souvent l'enfant "Model")
    [SerializeField] private Transform visualToRotate;

    private bool triggered = false;

    private void Awake()
    {
        if (visualToRotate == null)
            visualToRotate = transform;

        // Sécurité: si quelqu'un met (0,0,0)
        if (axeRotationLocal.sqrMagnitude < 0.0001f)
            axeRotationLocal = Vector3.up;
    }

    private void Update()
    {
        // Tourne sur l'axe local choisi
        visualToRotate.Rotate(axeRotationLocal.normalized, vitesseRotation * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        Debug.Log("VICTOIRE !");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame(true);
        }
        else
        {
            Debug.LogError("Relancez le jeu depuis le main menu pour activer le GameManager.");
            Time.timeScale = 0f;
        }

        Destroy(gameObject);
    }
}