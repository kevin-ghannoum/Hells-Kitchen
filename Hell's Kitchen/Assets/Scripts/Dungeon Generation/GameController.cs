// Adapted from: https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity
using Common;
using Common.Enums;
using Common.Interfaces;
using Photon.Pun;
using UnityEngine;
using Player;
using UI;

namespace Dungeon_Generation
{
    [RequireComponent(typeof(MazeConstructor))]

    public class GameController : MonoBehaviour
    {
        [SerializeField] private WeaponInstance defaultWeapon = WeaponInstance.Scimitar;
        [SerializeField] public Transform mazeStart;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private PhotonView photonView;

        private MazeConstructor _generator;

        void Start() {
            _generator = GetComponent<MazeConstructor>();
            StartNewGame();
        }

        public void StartNewGame()
        {
            StartNewMaze();
            SpawnPlayers();
        }
        
        public void StartNewMaze()
        {
            _generator.GenerateNewMaze();
        }

        public void SpawnPlayers()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(SpawnLocalPlayer), RpcTarget.All);
            }
        }

        [PunRPC]
        public void SpawnLocalPlayer()
        {
            playerSpawner.SpawnPlayerInScene();
            if (PhotonNetwork.IsMasterClient)
            {
                playerSpawner.SpawnSousChefInScene();
            }
            SetUpPlayerWeapon();
        }

        private void SetUpPlayerWeapon()
        {
            var playerController = NetworkHelper.GetLocalPlayerController();
            if (!playerController)
                return;
            
            var heldWeapon = playerController.gameObject.GetComponentInChildren<IPickup>();
            if (heldWeapon != null)
            {
                heldWeapon.RemoveFromPlayer();
            }
                
            // Set default weapon in case none is equipped when entering the dungeon
            if (GameStateData.carriedWeapon == WeaponInstance.None)
                GameStateData.carriedWeapon = defaultWeapon;
            
            // set weapon in players hand
            var weapon = Weapons.Models.Weapons.GetItem(GameStateData.carriedWeapon);
            var weaponInstance = PhotonNetwork.Instantiate(weapon.WeaponModel.Prefab.name, Vector3.zero, Quaternion.identity);
            weaponInstance.GetComponent<IPickup>()?.PickUp();
        }

    }
}

