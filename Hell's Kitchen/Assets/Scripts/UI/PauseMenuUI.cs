﻿using System;
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
            _input.reference.actions["OpenPauseMenu"].performed -= OpenPauseMenu;
        }

        public void OpenPauseMenu(InputAction.CallbackContext callbackContext)
        {
            isUIActive = !isUIActive;
            content.SetActive(isUIActive);
            Time.timeScale = isUIActive ? 0f : 1f;
        }

        public void ResumeGame()
        {
            isUIActive = !isUIActive;
            content.SetActive(isUIActive);
            Time.timeScale = 1f;
        }
    }
}