using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BUT
{
    public class EndGameUI : MonoBehaviour
    {
        [Header("Score Data")]
        [SerializeField] private Score score;   // ScriptableObject Score_Game

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

        private void Awake()
        {
            // S’assurer que panneaux cachés au démarrage
            if (panelVictory) panelVictory.SetActive(false);
            if (panelDefeat) panelDefeat.SetActive(false);
        }

        private void Start()
        {
            // Brancher les boutons (actions temporaires – on affinera après)
            if (victoryBtnMenu) victoryBtnMenu.onClick.AddListener(OnClickMenu);
            if (victoryBtnQuit) victoryBtnQuit.onClick.AddListener(OnClickQuit);
            if (defeatBtnMenu) defeatBtnMenu.onClick.AddListener(OnClickMenu);
            if (defeatBtnQuit) defeatBtnQuit.onClick.AddListener(OnClickQuit);
        }

        private void OnClickMenu()
        {
            Debug.Log("Retour Menu (à remplacer par chargement de scène Menu).");
            // Exemple futur: UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
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
            if (panelDefeat) panelDefeat.SetActive(false);
            if (panelVictory)
            {
                panelVictory.SetActive(true);
                UpdateVictoryScore();
            }
        }

        public void ShowDefeat()
        {
            if (panelVictory) panelVictory.SetActive(false);
            if (panelDefeat)
            {
                panelDefeat.SetActive(true);
                UpdateDefeatScore();
            }
        }

        private void UpdateVictoryScore()
        {
            if (score && victoryScore)
                victoryScore.text = "Score: " + score.Value;
        }

        private void UpdateDefeatScore()
        {
            if (score && defeatScore)
                defeatScore.text = "Score: " + score.Value;
        }
    }
}