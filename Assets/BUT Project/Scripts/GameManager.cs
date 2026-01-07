using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "SimpleSceneGame";

    [Header("Score")]
    [SerializeField] private int score = 0;
    public int Score => score;

    [Header("UI (auto-binding via noms de ta Hierarchy)")]
    [SerializeField] private TextMeshProUGUI hudScoreText; // Canvas/HUD_Score/Value
    [SerializeField] private TextMeshProUGUI endScoreText; // Canvas/Panel_Victory/ScoreFinal

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        RebindUI();
        UpdateScoreTexts();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindUI();
        UpdateScoreTexts();
    }

    private void RebindUI()
    {
        // 1) HUD Score: Canvas/HUD_Score/Value
        hudScoreText = FindTMP("Canvas/HUD_Score/Value");

        // 2) Score final (victoire): Canvas/Panel_Victory/ScoreFinal
        endScoreText = FindTMP("Canvas/Panel_Victory/ScoreFinal");
    }

    private TextMeshProUGUI FindTMP(string hierarchyPath)
    {
        var go = GameObject.Find(hierarchyPath);
        return go != null ? go.GetComponent<TextMeshProUGUI>() : null;
    }

    // --- MENU ---
    public void StartGame()
    {
        Time.timeScale = 1f;
        ResetScore();
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
    SceneManager.LoadScene(gameSceneName);
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

    public void BackToMenu()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
}
    public void EndGame(bool isVictory)
{
    Debug.Log($"EndGame appelé | Victoire : {isVictory} | Score final : {score}");

    Time.timeScale = 0f;

    // Trouver EndGameUI dans la scène
    var endGameUI = FindFirstObjectByType<BUT.EndGameUI>();

    if (endGameUI != null)
    {
        Debug.Log($"EndGameUI trouvé : Panneau affiché - {(isVictory ? "Victory" : "Defeat")}");

        if (isVictory)
        {
            endGameUI.ShowVictory();
        }
        else
        {
            endGameUI.ShowDefeat();
        }
    }
    else
    {
        Debug.LogError("EndGameUI introuvable dans la scène !");
    }
}
}