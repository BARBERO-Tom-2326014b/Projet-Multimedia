using System.Collections;
using UnityEngine;

public class ouvertureCoffre : MonoBehaviour
{
    public AudioClip openSound;
    public Animator animator;

    [Header("Victoire")]
    [SerializeField] private float delayBeforeWin = 3f;

    private bool isOpen = false;
    public bool IsOpened => isOpen; // Propriété publique pour vérifier si le coffre est ouvert

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && inventaire.instance.hasKey && !isOpen)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("Press");

        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);

        // Déclencher la victoire après un délai pour laisser l'anim se jouer
        StartCoroutine(WinAfterDelay());
    }

    private IEnumerator WinAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeWin);

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
        else
            Debug.LogError("Relancez le jeu depuis le main menu pour activer le GameManager.");
    }
}