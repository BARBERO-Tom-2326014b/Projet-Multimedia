using UnityEngine;

public class pickupEpee : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat combat = other.GetComponent<PlayerCombat>();

            if (combat != null)
            {
                combat.EquiperEpee();   // ✅ ici seulement
                Destroy(gameObject);    // enlève l'épée du sol
            }
        }
    }
}
