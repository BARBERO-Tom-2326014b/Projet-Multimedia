using UnityEngine;

public class tropheeVictoire : MonoBehaviour
{
    public float vitesseRotation = 100f;

    private bool triggered = false;

    void Update()
    {
        // Rotation permanente
        transform.Rotate(0f, vitesseRotation * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        Debug.Log("VICTOIRE !");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame(true); // Victoire
        }
        else
        {
            Debug.LogError("Relancez le jeu depuis le main menu pour activer le GameManager.");
            // Fallback si jamais il n'existe pas
            Time.timeScale = 0f;
        }

        // Optionnel : enlever le trophée après contact
         Destroy(gameObject);
    }
}