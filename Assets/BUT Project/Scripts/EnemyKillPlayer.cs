using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject defeatPopup;         // Panel de défaite (UI)
    [SerializeField] private float stompHeightOffset = 0.5f; // marge pour considérer que le joueur est "au-dessus"
    [SerializeField] private float bounceForce = 5f;         // force de rebond du joueur après avoir tué le Goomba
    [SerializeField] private int scoreValue = 10;            // points gagnés en tuant ce Goomba

    private bool isDead = false; // éviter plusieurs morts en cascade si plusieurs triggers

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        // On ne réagit qu'au joueur
        if (!other.CompareTag("Player"))
            return;

        Transform player = other.transform;

        // Comparer la hauteur du joueur et du Goomba
        float playerY = player.position.y;
        float goombaY = transform.position.y;

        bool playerIsAbove = playerY > goombaY + stompHeightOffset;

        if (playerIsAbove)
        {
            // Le joueur saute SUR le Goomba -> le Goomba meurt, le joueur survit
            Debug.Log("Goomba tué par le dessus");

            isDead = true;

            // Ajouter des points au score global
            if (GameManager.Instance != null)
            {
                Debug.Log($"BOO KILL -> AddScore({scoreValue})");
                GameManager.Instance.AddScore(scoreValue);
            }
            else
            {
                Debug.LogError("Relancez le jeu depuis le main menu pour activer le GameManager.");
            }

            // Petit rebond du joueur
            var playerMovement = player.GetComponent<BUT.PlayerMovement>();
            if (playerMovement != null)
            {
                // On "pousse" un peu vers le haut
                playerMovement.GravityVelocity = Mathf.Sqrt(Mathf.Abs(BUT.PlayerMovement.GRAVITY) * bounceForce);
            }

            // Ici tu peux jouer une animation / effet / son, puis détruire le Goomba
            Destroy(gameObject); // détruit le Goomba dans la scène
        }
        else
        {
            // Le joueur touche le Goomba par le côté -> le joueur meurt
            Debug.Log("Le joueur est mort : touché par le côté du Goomba");

            if (defeatPopup != null)
                defeatPopup.SetActive(true);

            // Bloquer les contrôles du joueur
            var playerMovement = player.GetComponent<BUT.PlayerMovement>();
            if (playerMovement != null)
                playerMovement.enabled = false;

            var characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
                characterController.enabled = false;

            // Prévenir le GameManager que la partie est finie (pour mettre à jour le score final)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndGame(false);
            }

            // Optionnel : pause du jeu
            // Time.timeScale = 0f;
        }
    }
}