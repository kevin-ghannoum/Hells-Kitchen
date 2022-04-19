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
        }
        
        public void LoadMainMenu()
        {
            PhotonNetwork.LoadLevel(Scenes.MainMenu);
        }
        
        public void LoadGameOverScene(PhotonView photonView)
        {
            photonView.RPC(nameof(LoadGameOverRPC), RpcTarget.MasterClient);
        }

        public void LoadRestaurantScene(PhotonView photonView)
        {
            photonView.RPC(nameof(LoadRestaurantRPC), RpcTarget.MasterClient);
        }
        
        public void LoadRestaurantScene()
        {
            PhotonNetwork.LoadLevel(Scenes.Restaurant);
        }

        public void LoadDungeonScene(PhotonView photonView)
        {
            photonView.RPC(nameof(LoadDungeonRPC), RpcTarget.MasterClient);
        }
        
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #region PUNCallabacks

        [PunRPC]
        private void LoadRestaurantRPC()
        {
            PhotonNetwork.LoadLevel(Scenes.Restaurant);
        }
        
        [PunRPC]
        private void LoadDungeonRPC()
        {
            PhotonNetwork.LoadLevel(Scenes.Dungeon);
        }

        [PunRPC]
        private void LoadGameOverRPC()
        {
            PhotonNetwork.LoadLevel(Scenes.GameOver);
        }

        #endregion
        
    }
}