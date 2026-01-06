using UnityEngine;

public class BackToMenuButton : MonoBehaviour
{
    public void BackToMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BackToMenu();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance est null : impossible de retourner au menu.");
        }
    }
}