using UnityEngine;

public class TournePiece : MonoBehaviour
{
    // Vitesse de rotation (modifiable dans l'inspecteur)
    public float vitesseRotation = 100f;

    void Update()
    {
        transform.Rotate(0f, 0f, vitesseRotation * Time.deltaTime);
    }
}
