using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BUT
{
    public class EndGameUI : MonoBehaviour
    {
        [Header("Score Data")]
        [SerializeField] private Score score; // ScriptableObject Score_Game

        [Header("Victory Panel")]
        [SerializeField] private GameObject panelVictory;
        [SerializeField] private TMP_Text victoryTitle;
        [SerializeField] private TMP_Text victoryScore;
        [SerializeField] private Button victoryBtnMenu;
        [SerializeField] private Button victoryBtnQuit;

        [Header("Defeat Panel")]
        [SerializeField] private GameObject panelDefeat;
        [SerializeField] private TMP_Text defeatTitle;
        [SerializeField] private TMP_Text defeatScore;
        [SerializeField] private Button defeatBtnMenu;
        [SerializeField] private Button defeatBtnQuit;

        [Header("Chest Reference")] 
        [SerializeField] private ouvertureCoffre chest; 

        private void Awake()
        {
            // S'assurer que les panneaux sont cachés au démarrage
            if (panelVictory) panelVictory.SetActive(false);
            if (panelDefeat) panelDefeat.SetActive(false);
        }

        private void Start()
        {
            // Brancher les boutons
            if (victoryBtnMenu) victoryBtnMenu.onClick.AddListener(OnClickMenu);
            if (victoryBtnQuit) victoryBtnQuit.onClick.AddListener(OnClickQuit);
            if (defeatBtnMenu) defeatBtnMenu.onClick.AddListener(OnClickMenu);
            if (defeatBtnQuit) defeatBtnQuit.onClick.AddListener(OnClickQuit);
        }

        public void EvaluateGameOver()
        {
            // Vérifier les conditions de victoire
            if (score && score.Value >= 100 || IsChestOpened())
            {
                ShowVictory();
            }
            else
            {
                ShowDefeat();
            }
        }   
        private bool IsChestOpened()
        {
            // Vérifier si le coffre est ouvert
            return chest && chest.IsOpened;
        }
        private void OnClickMenu()
        {
            Debug.Log("Retour Menu (à remplacer par chargement de scène Menu).");
            // Exemple futur : UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }

        private void OnClickQuit()
        {
            Debug.Log("Quitter jeu.");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void ShowVictory()
        {
            StopFootsteps(); // Arrêter les bruits de pas
            if (panelDefeat) panelDefeat.SetActive(false);

            if (panelVictory)
            {
                panelVictory.SetActive(true);
                UpdateVictoryScore();
            }
        }

        public void ShowDefeat()
        {
            Debug.Log("ShowDefeat appelé.");
            StopFootsteps(); // Arrêter les bruits de pas
            if (panelVictory) panelVictory.SetActive(false);

            if (panelDefeat)
            {
                panelDefeat.SetActive(true);
                UpdateDefeatScore();
            }
        }

        private void UpdateVictoryScore()
{
    if (GameManager.Instance != null && victoryScore)
    {
        victoryScore.text = "Score: " + GameManager.Instance.Score; // Score en cours
    }
}

        private void UpdateDefeatScore()
{
    if (GameManager.Instance != null && defeatScore)
    {
        defeatScore.text = "Score: " + GameManager.Instance.Score; // Score en cours
    }
}

        private void StopFootsteps()
        {
            // Stopper l'AudioSource des bruits de pas
            var player = GameObject.FindWithTag("Player"); // Assurez-vous que le tag est "Player"
            if (player)
            {
                var audioSource = player.GetComponent<AudioSource>();
                if (audioSource)
                {
                    audioSource.Stop(); // Arrête le son des pas
                    audioSource.enabled = false; // Désactive l'AudioSource
                }
            }
        }

        
    }
}