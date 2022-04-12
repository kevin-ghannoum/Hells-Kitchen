﻿using Common.Enums;
using UnityEngine;

namespace Common
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(Instance.gameObject);
        }

        public void LoadMainMenu()
        {
        }


        public void LoadGameOverScene()
        {
        }

        public void LoadRestaurantScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.Restaurant);
        }

        public void LoadDungeonScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.Dungeon);
        }
        
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}