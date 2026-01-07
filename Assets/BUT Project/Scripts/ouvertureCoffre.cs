using UnityEngine;

public class ouvertureCoffre : MonoBehaviour
{
    public AudioClip openSound;
    public Animator animator;

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

    AudioSource.PlayClipAtPoint(openSound, transform.position);

    GameManager.Instance.EndGame(true); // Victoire : la partie se termine
}
}