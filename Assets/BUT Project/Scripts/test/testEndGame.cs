using UnityEngine;
using UnityEngine.InputSystem;   // IMPORTANT pour le nouveau Input System

namespace BUT
{
    public class TestEndGame : MonoBehaviour
    {
        [SerializeField] private EndGameUI endUI;

        private void Update()
        {
            if (endUI == null) return;

            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // V = Victoire
            if (keyboard.vKey.wasPressedThisFrame)
                endUI.ShowVictory();

            // D = Défaite
            if (keyboard.cKey.wasPressedThisFrame)
                endUI.ShowDefeat();
        }
    }
}