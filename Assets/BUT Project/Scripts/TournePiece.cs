using UnityEngine;

public class TournePiece : MonoBehaviour
{
    [Header("Rotation")]
    public float vitesseRotation = 100f;

    [Header("Points")]
    public int points = 5;

    [Header("Optionnel")]
    public AudioClip pickupSound;

    void Update()
    {
        transform.Rotate(0f, 0f, vitesseRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Ajouter 5 points
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(points);
        }
        else
        {
            Debug.LogError("Score non ajouté : GameManager est introuvable.");
        }

        // Jouer un son optionnel
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // Supprimer la pièce
        Destroy(gameObject);
    }
}