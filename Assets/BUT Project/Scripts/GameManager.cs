using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- SCENES ---
    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "SimpleSceneGame";

    // --- SCORE ---
    [Header("Score")]
    [SerializeField] private int score = 0;
    public int Score => score; // Score actuel
    private int finalScore;    // Sauvegarde du score final avant l'UI

    // --- UI ---
    [Header("UI (auto-binding via noms de ta Hierarchy)")]
    [SerializeField] private TextMeshProUGUI hudScoreText; // Canvas/HUD_Score/Value
    [SerializeField] private TextMeshProUGUI endScoreText; // Canvas/Panel_Victory/ScoreFinal

    // === AWAKE & INITIALISATION ===
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
        // Bind HUD Score UI
        hudScoreText = FindTMP("Canvas/HUD_Score/Value");

        // Bind Score Final (victory/defeat UI)
        endScoreText = FindTMP("Canvas/Panel_Victory/ScoreFinal");
    }

    private TextMeshProUGUI FindTMP(string hierarchyPath)
    {
        var go = GameObject.Find(hierarchyPath);
        return go != null ? go.GetComponent<TextMeshProUGUI>() : null;
    }

    // === GAME START / END ===
    public void StartGame()
    {
        Debug.Log("Game démarre !");
        Time.timeScale = 1f;
        ResetScore();
        SceneManager.LoadScene(gameSceneName);
    }

    public void RestartGame()
    {
        Debug.Log("Game redémarre !");
        Time.timeScale = 1f;
        ResetScore();
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitter jeu.");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void BackToMenu()
    {
        Debug.Log("Retour au menu principal.");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void EndGame(bool isVictory)
    {
        // Sauvegarder le score final avant de réinitialiser quoi que ce soit
        finalScore = score;

        // Pause du jeu
        Time.timeScale = 0f;

        // Affichage de l'UI correspondant
        var endGameUI = FindFirstObjectByType<BUT.EndGameUI>();
        if (endGameUI != null)
        {
            Debug.Log($"Panneau actif : {(isVictory ? "Victoire" : "Défaite")}");

            if (isVictory)
                endGameUI.ShowVictory();
            else
                endGameUI.ShowDefeat();
        }

        Debug.Log($"Fin de partie | Victoire : {isVictory} | Score final : {finalScore}");
    }

    private void UpdateScoreTexts()
    {
        if (hudScoreText != null)
            hudScoreText.text = score.ToString();

        if (endScoreText != null)
            endScoreText.text = finalScore.ToString(); // Affiche le score final
    }

    // === SCORE MANAGEMENT ===
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score ajouté : {amount} | Nouveau score : {score}");
        UpdateScoreTexts();

        // Vérification des 100 points pour la victoire
        if (score >= 100)
        {
            Debug.Log("Victoire ! Le score a atteint 100 points.");
            EndGame(true); // Appeler la victoire
        }
    }

    public void ResetScore()
    {
        Debug.Log($"Score avant réinitialisation : {score}");
        score = 0;
        UpdateScoreTexts();
    }
}