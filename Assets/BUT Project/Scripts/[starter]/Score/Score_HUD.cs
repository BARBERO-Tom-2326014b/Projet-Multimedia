using UnityEngine;
using UnityEngine.UI;

namespace BUT
{
    public class ScoreHUD : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Score score;

        [Header("UI")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMPro.TMP_Text valueTextTMP;   // recommandé
        [SerializeField] private UnityEngine.UI.Text valueText; // fallback si pas de TMP

        [Header("Affichage")]
        [SerializeField] private string format = "{0}"; // ex: "x {0}" ou "{0} pts"

        private void OnEnable()
        {
            if (score != null)
            {
                // s'abonner aux changements
                score.OnScoreChanged.AddListener(OnScoreChanged);
            }
            // init affichage
            RefreshIcon();
            OnScoreChanged(score != null ? score.Value : 0);
        }

        private void OnDisable()
        {
            if (score != null)
            {
                score.OnScoreChanged.RemoveListener(OnScoreChanged);
            }
        }

        private void RefreshIcon()
        {
            if (iconImage != null && score != null)
            {
                iconImage.sprite = score.Icon;
                iconImage.enabled = (score.Icon != null);
            }
        }

        private void OnScoreChanged(int newValue)
        {
            string txt = string.Format(format, newValue);
            if (valueTextTMP != null) valueTextTMP.text = txt;
            if (valueText != null) valueText.text = txt;
        }
    }
}