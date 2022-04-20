using Common.Enums;
using Photon.Pun;
using UnityEngine;

namespace Common
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        [SerializeField]
        private PhotonView photonView;

        private void Reset()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        [PunRPC]
        private void DestroyPlayerRPC()
        {
            if (GameStateData.player != null)
            {
                Destroy(GameStateData.player);
                GameStateData.player = null;
            }
        }

        public void LoadMainMenu()
        {
            PhotonNetwork.LoadLevel(Scenes.MainMenu);
        }
        
        public void LoadGameOverScene()
        {
            photonView.RPC(nameof(LoadGameOverRPC), RpcTarget.MasterClient);
        }

        public void LoadRestaurantScene()
        {
            photonView.RPC(nameof(LoadRestaurantRPC), RpcTarget.MasterClient);
        }

        public void LoadDungeonScene()
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
            photonView.RPC(nameof(DestroyPlayerRPC), RpcTarget.All);
            PhotonNetwork.LoadLevel(Scenes.Restaurant);
        }
        
        [PunRPC]
        private void LoadDungeonRPC()
        {
            photonView.RPC(nameof(DestroyPlayerRPC), RpcTarget.All);
            PhotonNetwork.DestroyAll();
            PhotonNetwork.LoadLevel(Scenes.Dungeon);
        }

        [PunRPC]
        private void LoadGameOverRPC()
        {
            photonView.RPC(nameof(DestroyPlayerRPC), RpcTarget.All);
            PhotonNetwork.DestroyAll();
            PhotonNetwork.LoadLevel(Scenes.GameOver);
        }
        
        #endregion
        
    }
}
