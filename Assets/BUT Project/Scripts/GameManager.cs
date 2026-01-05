using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] private int score = 0;
    public int Score => score;

    [SerializeField] private TextMeshProUGUI hudScoreText; // hud_score/value
    [SerializeField] private TextMeshProUGUI endScoreText; // FinalScoreText (panel defeat/victory)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Si tu restes sur une seule scène, tu peux enlever cette ligne
        // DontDestroyOnLoad(gameObject);

        UpdateScoreTexts();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score ajouté : " + amount + " | Nouveau score = " + score);
        UpdateScoreTexts();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreTexts();
    }

    public void RestartGame()
{
    // Remet le temps à la normale au cas où tu l'aurais mis en pause
    Time.timeScale = 1f;

    // Remet le score à zéro (si tu veux repartir de 0)
    ResetScore();

    // Recharge la scène courante
    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.buildIndex);
}

    public void OnGameEnd()
    {
        // Called when defeat/victory happens
        UpdateScoreTexts();
    }

    private void UpdateScoreTexts()
    {
        if (hudScoreText != null)
        {
            hudScoreText.text = score.ToString();
        }

        if (endScoreText != null)
        {
            endScoreText.text = score.ToString();
        }
    }
}