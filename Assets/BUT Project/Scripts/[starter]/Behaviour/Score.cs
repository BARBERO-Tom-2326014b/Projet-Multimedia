using UnityEngine;
using UnityEngine.Events;

namespace BUT
{
    [CreateAssetMenu(fileName = "Score", menuName = "BUT/Score", order = 1)]
    public class Score : ScriptableObject
    {
        [SerializeField] private int m_Value = 0;
        [SerializeField] private Sprite m_Icon;

        // Appelé à chaque changement de score (passe la nouvelle valeur)
        public UnityEvent<int> OnScoreChanged;

        public int Value => m_Value;
        public Sprite Icon => m_Icon;

        public void ResetValue()
        {
            SetValue(0);
        }

        public void Add(int amount)
        {
            SetValue(m_Value + amount);
        }

        public void SetValue(int value)
        {
            if (m_Value == value) return;
            m_Value = Mathf.Max(0, value);
            OnScoreChanged?.Invoke(m_Value);
        }
    }
}