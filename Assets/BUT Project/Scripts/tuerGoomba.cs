using UnityEngine;

public class tuerGoomba : MonoBehaviour
{
    public void Mourir()
    {
        Debug.Log("Goomba mort !");
        Destroy(gameObject);
    }
}
