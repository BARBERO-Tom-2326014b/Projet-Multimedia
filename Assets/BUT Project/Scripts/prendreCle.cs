using UnityEngine;

public class prendreCle : MonoBehaviour
{
    public AudioClip keySound;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Key"))
        {
            Debug.Log("CLE RECUPEREE");

            inventaire.instance.hasKey = true;

            if (keySound != null)
                AudioSource.PlayClipAtPoint(keySound, hit.transform.position);

            Destroy(hit.gameObject);
        }
    }
}
