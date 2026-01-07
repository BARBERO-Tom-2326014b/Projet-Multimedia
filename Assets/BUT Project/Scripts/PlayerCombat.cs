using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float porteeAttaque = 2f;
    public float rayonAttaque = 1f;

    public GameObject epeeMain;   // épée dans la main
    private bool aUneEpee = false;

    void Start()
    {
        aUneEpee = false;

        if (epeeMain != null)
            epeeMain.SetActive(false); // ❌ épée cachée au départ
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attaquer();
        }
    }

    void Attaquer()
{
    if (!aUneEpee) return;

    // Centre de la zone d’attaque (devant + un peu vers le haut)
    Vector3 centre = transform.position 
                   + transform.forward * porteeAttaque 
                   + Vector3.up * 0.5f;

    Collider[] hits = Physics.OverlapSphere(centre, rayonAttaque);

    foreach (Collider col in hits)
    {
        if (col.CompareTag("Enemy"))
        {
            GoombaMovement g = col.GetComponent<GoombaMovement>();
            if (g != null)
            {
                g.Mourir();
            }
        }
    }
}


    // ✅ appelée UNIQUEMENT quand on ramasse l'épée
    public void EquiperEpee()
    {
        aUneEpee = true;

        if (epeeMain != null)
            epeeMain.SetActive(true);
    }
}
