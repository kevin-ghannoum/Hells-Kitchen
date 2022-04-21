using Common.Enums;
using Common.Interfaces;
using Photon.Pun;
using Player;
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

        public void LoadRestaurantScene(bool shouldIncrementLevel = false)
        {
            photonView.RPC(nameof(RemoveWeapon), RpcTarget.All);
            photonView.RPC(nameof(LoadRestaurantRPC), RpcTarget.MasterClient, shouldIncrementLevel);
        }

        public void LoadDungeonScene(bool resetClock = false)
        {
            photonView.RPC(nameof(LoadDungeonRPC), RpcTarget.MasterClient, resetClock);
        }
        
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #region PUNCallbacks

        [PunRPC]
        private void LoadRestaurantRPC(bool shouldIncrementLevel)
        {
            if (shouldIncrementLevel)
                GameStateManager.SetHiddenLevel(GameStateData.hiddenLevel + 1);
            
            photonView.RPC(nameof(DestroyPlayerRPC), RpcTarget.All);
            PhotonNetwork.LoadLevel(Scenes.Restaurant);
        }
        
        [PunRPC]
        private void LoadDungeonRPC(bool resetClock)
        {
            if (resetClock)
                GameStateData.dungeonClock = 0.0f;
            
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

        [PunRPC]
        private void RemoveWeapon()
        {
            var player = NetworkHelper.GetLocalPlayerObject();
            if (!player) return;

            var playerController = player.GetComponent<PlayerController>();
            if (!playerController) return;
            
            var heldWeapon = playerController.GetComponentInChildren<IPickup>();
            if (heldWeapon != null)
            {
                GameStateData.carriedWeapon = WeaponInstance.None;
                heldWeapon.RemoveFromPlayer();
            }
        }
        #endregion
        
    }
}
