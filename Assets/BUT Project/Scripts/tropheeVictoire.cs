using UnityEngine;

public class tropheeVictoire : MonoBehaviour
{
    public float vitesseRotation = 100f;

    void Update()
    {
        // Rotation permanente
        transform.Rotate(0f, vitesseRotation * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("VICTOIRE !");
            Time.timeScale = 0f;
        }
    }
}
