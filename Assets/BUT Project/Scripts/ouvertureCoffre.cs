using UnityEngine;

public class ouvertureCoffre : MonoBehaviour
{
    public AudioClip openSound;
    public Animator animator;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && inventaire.instance.hasKey && !isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("Press");

        AudioSource.PlayClipAtPoint(openSound, transform.position);
    }
}
