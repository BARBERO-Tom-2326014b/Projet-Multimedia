using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "SceneGame";

    [Header("Score")]
    [SerializeField] private int score = 0;
    public int Score => score;

    [Header("UI (optionnel selon la scène)")]
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

        // IMPORTANT: permet d'avoir le même GameManager en Menu + en Jeu
        DontDestroyOnLoad(gameObject);

        // Quand une nouvelle scène charge, on retente de récupérer les références UI si besoin
        SceneManager.sceneLoaded += OnSceneLoaded;

        UpdateScoreTexts();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si tes TextMeshProUGUI sont assignés manuellement dans l'inspector de la scène de jeu,
        // tu peux ignorer cette partie.
        // Ici on fait juste un refresh (les champs null ne casseront pas).
        UpdateScoreTexts();
    }

    // --- MENU ---
    public void StartGame()
    {
        Time.timeScale = 1f;
        ResetScore(); // optionnel: repartir à 0 au démarrage
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // --- SCORE ---
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
        Time.timeScale = 1f;
        ResetScore();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void OnGameEnd()
    {
        UpdateScoreTexts();
    }

    private void UpdateScoreTexts()
    {
        if (hudScoreText != null)
            hudScoreText.text = score.ToString();

        if (endScoreText != null)
            endScoreText.text = score.ToString();
    }
}