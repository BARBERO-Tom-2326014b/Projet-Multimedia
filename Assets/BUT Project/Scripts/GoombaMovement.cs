using UnityEngine;

public class GoombaMovement :  MonoBehaviour
{
    [Header("Déplacement")]
    [Tooltip("Vitesse de déplacement du Goomba")]
    public float vitesse = 2f;
    
    [Tooltip("Distance maximale de déplacement à partir du point de départ")]
    public float distanceMax = 5f;
    
    [Header("Optionnel")]
    [Tooltip("Axe de déplacement (X, Y ou Z)")]
    public enum AxeDeplacement { X, Y, Z }
    public AxeDeplacement axe = AxeDeplacement.X;
    
    [Tooltip("Retourner le sprite selon la direction (pour un visuel correct)")]
    public bool retournerSprite = true;
    
    // Variables privées
    private Vector3 positionDepart;
    private float direction = 1f; // 1 pour droite, -1 pour gauche
    
    void Start()
    {
        // Enregistrer la position de départ
        positionDepart = transform.position;
    }
    
    void Update()
    {
        // Calculer le déplacement
        Vector3 deplacement = Vector3.zero;
        
        switch (axe)
        {
            case AxeDeplacement.X:
                deplacement = Vector3.right * direction * vitesse * Time.deltaTime;
                break;
            case AxeDeplacement.Y:
                deplacement = Vector3.up * direction * vitesse * Time.deltaTime;
                break;
            case AxeDeplacement.Z: 
                deplacement = Vector3.forward * direction * vitesse * Time. deltaTime;
                break;
        }
        
        // Déplacer le Goomba
        transform. Translate(deplacement);
        
        // Vérifier si le Goomba a atteint la distance maximale
        float distanceParcourue = 0f;
        
        switch (axe)
        {
            case AxeDeplacement. X:
                distanceParcourue = Mathf.Abs(transform. position.x - positionDepart.x);
                break;
            case AxeDeplacement. Y:
                distanceParcourue = Mathf. Abs(transform.position.y - positionDepart.y);
                break;
            case AxeDeplacement.Z:
                distanceParcourue = Mathf.Abs(transform. position.z - positionDepart.z);
                break;
        }
        
        // Inverser la direction si la distance max est atteinte
        if (distanceParcourue >= distanceMax)
        {
            direction *= -1;
            
            // Retourner le sprite si activé
            if (retournerSprite)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
    
    // Pour visualiser la zone de déplacement dans l'éditeur Unity
    private void OnDrawGizmosSelected()
    {
        Vector3 positionRef = Application.isPlaying ? positionDepart : transform.position;
        
        Gizmos.color = Color. yellow;
        
        switch (axe)
        {
            case AxeDeplacement.X:
                Gizmos.DrawLine(positionRef + Vector3.left * distanceMax, 
                               positionRef + Vector3.right * distanceMax);
                break;
            case AxeDeplacement.Y: 
                Gizmos.DrawLine(positionRef + Vector3.down * distanceMax, 
                               positionRef + Vector3.up * distanceMax);
                break;
            case AxeDeplacement.Z: 
                Gizmos.DrawLine(positionRef + Vector3.back * distanceMax, 
                               positionRef + Vector3.forward * distanceMax);
                break;
        }
    }
}