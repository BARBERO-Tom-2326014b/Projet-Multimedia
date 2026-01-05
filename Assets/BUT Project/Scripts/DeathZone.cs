using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject defeatPopup; // Panel de défaite (UI)

    private void OnTriggerEnter(Collider other)
    {
        // On réagit seulement si c'est le joueur
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Le joueur est tombé dans la zone de mort");

        // Afficher la popup de défaite
        if (defeatPopup != null)
        {
            defeatPopup.SetActive(true);
        }

        // Bloquer les contrôles du joueur
        var playerMovement = other.GetComponent<BUT.PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        var characterController = other.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Prévenir le GameManager (pour mettre à jour le score final dans le panel)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnd();
        }

        // Optionnel : mettre le jeu en pause
        // Time.timeScale = 0f;
    }
}