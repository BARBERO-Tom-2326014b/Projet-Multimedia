using UnityEngine;

public class inventaire : MonoBehaviour
{
    public bool hasKey = false;
    public static inventaire instance;

    private void Awake()
    {
        instance = this;
    }
}
