using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenuUI : MenuUI
    {
        [SerializeField] private GameObject content;
        
        private InputManager _input => InputManager.Instance;
        private bool isUIActive = false;
        
        private void Awake()
        {
            _input.reference.actions["OpenPauseMenu"].performed += OpenPauseMenu;
        }

        private void OnDestroy()
        {
            // return to game before leaving to resume timescale and return to defaults
            ResumeGame();
            if (_input == null)
                return;
            _input.reference.actions["OpenPauseMenu"].performed -= OpenPauseMenu;
        }

        public void OpenPauseMenu(InputAction.CallbackContext callbackContext)
        {
            isUIActive = !isUIActive;
            content.SetActive(isUIActive);
        }

        public void ResumeGame()
        {
            isUIActive = !isUIActive;
            content.SetActive(isUIActive);
        }
    }
}
