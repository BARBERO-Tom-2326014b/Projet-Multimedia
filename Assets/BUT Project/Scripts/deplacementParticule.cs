using UnityEngine;

public class deplacementParticule : MonoBehaviour
{
    public CharacterController controller;
    public ParticleSystem particles;

    void Update()
    {
        if (controller.isGrounded && controller.velocity.sqrMagnitude > 0.01f)
        {
            if (!particles.isPlaying)
                particles.Play();
        }
        else
        {
            if (particles.isPlaying)
                particles.Stop();
        }
    }
}
