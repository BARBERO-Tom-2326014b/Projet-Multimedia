using UnityEngine;

public class bruitDeplacement : MonoBehaviour
{
    public CharacterController controller;
    public AudioSource audioSource;
    public AudioClip footstepSound;

    public float stepRate = 0.5f;
    private float stepTimer;

    void Update()
    {
        
        if (controller.isGrounded && controller.velocity.magnitude > 0.2f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(footstepSound, 3f);

                stepTimer = stepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}
