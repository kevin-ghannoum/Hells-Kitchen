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
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float playerHeightPosition = 0f;
        [SerializeField] private WeaponInstance defaultWeapon = WeaponInstance.Scimitar;
        [SerializeField] private ClockUI clock;
        [SerializeField] public Transform mazeStart;

        private MazeConstructor _generator;

        void Start() {
            _generator = GetComponent<MazeConstructor>();
            StartNewGame();
        }

        public void StartNewGame()
        {
            StartNewMaze();
            SetUpPlayerWeapon();
            MovePlayerToStart();
            SetDungeonClock();
        }
        
        public void StartNewMaze()
        {
            _generator.GenerateNewMaze();

            float x = _generator.StartCol * _generator.hallwayWidth - (_generator.hallwayWidth / 2);
            float y = playerHeightPosition;
            float z = _generator.StartRow * _generator.hallwayWidth - (_generator.hallwayWidth / 2);
            playerController.transform.position = new Vector3(x, y, z);

            playerController.enabled = true;

            GameObject.FindWithTag(Tags.Pathfinding).GetComponent<Pathfinding>().Bake(true);
        }

        private void SetUpPlayerWeapon()
        {
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

        private void MovePlayerToStart()
        {
            var player = GameObject.FindWithTag(Tags.Player);
            var characterController = player.GetComponent<CharacterController>();
            characterController.enabled = false;
            var playerTransform = player.transform;
            playerTransform.transform.localPosition = mazeStart.position;
            characterController.enabled = true;
        }

        private void SetDungeonClock()
        {
            if (!GameStateData.dungeonTimeHasElapsed) return;
            
            // if true we are coming from the restaurant, so reset timer
            clock.ResetClock();
            GameStateData.dungeonTimeHasElapsed = false;
        }
    }
}

