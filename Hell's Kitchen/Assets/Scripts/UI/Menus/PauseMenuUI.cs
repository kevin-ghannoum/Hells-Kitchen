using Input;
using UnityEngine;

namespace UI.Menus
{
    public class PauseMenuUI : MenuUI
    {
        [SerializeField] private GameObject content;
        
        private bool _isUIActive = false;
        
        private void OnDestroy()
        {
            // return to game before leaving to resume timescale and return to defaults
            ResumeGame();
        }

        private void OpenPauseMenu()
        {
            _isUIActive = !_isUIActive;
            content.SetActive(_isUIActive);
        }

        public void ResumeGame()
        {
            _isUIActive = false;
            content.SetActive(_isUIActive);
        }

        private void Update()
        {
            if (InputManager.Actions.OpenPauseMenu.triggered)
            {
                OpenPauseMenu();
            }
        }
    }
}
