using Common.Enums;
using Photon.Pun;
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

            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(Instance.gameObject);
        }

        public void LoadMainMenu()
        {
            PhotonNetwork.LoadLevel(Scenes.MainMenu);
        }


        public void LoadGameOverScene()
        {
            PhotonNetwork.LoadLevel(Scenes.GameOver);
        }

        public void LoadRestaurantScene()
        {
            PhotonNetwork.LoadLevel(Scenes.Restaurant);
        }

        public void LoadDungeonScene()
        {
            PhotonNetwork.LoadLevel(Scenes.Dungeon);
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