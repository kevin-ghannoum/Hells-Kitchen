// Adapted from: https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity
using Common;
using Common.Enums;
using Common.Interfaces;
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
        [SerializeField] private Transform mazeStart;

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
            MoveSousChefToStart();
            SetDungeonClock();
        }
        
        public void StartNewMaze()
        {
            _generator.GenerateNewMaze(mazeStart);

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
            if (GameStateManager.Instance.carriedWeapon == WeaponInstance.None)
                GameStateManager.Instance.carriedWeapon = defaultWeapon;
            
            // set weapon in players hand
            var weapon = Weapons.Models.Weapons.GetItem(GameStateManager.Instance.carriedWeapon);
            var weaponInstance = Instantiate(weapon.WeaponModel.Prefab);
            weaponInstance.GetComponent<IPickup>()?.PickUp();
        }

        private void MovePlayerToStart()
        {
            Vector3 playerPosition = new Vector3(mazeStart.position.x - 1, mazeStart.position.y, mazeStart.position.z);
            var player = GameObject.FindWithTag(Tags.Player);
            var characterController = player.GetComponent<CharacterController>();
            characterController.enabled = false;
            var playerTransform = player.transform;
            playerTransform.transform.localPosition = playerPosition;
            characterController.enabled = true;
        }
        private void MoveSousChefToStart() {
            Vector3 sousChefPosition = new Vector3(mazeStart.position.x + 1, mazeStart.position.y, mazeStart.position.z);
            var sousChefs = GameObject.FindGameObjectsWithTag("SousChef");
            foreach (var ss in sousChefs) {
                ss.transform.localPosition = sousChefPosition;
            }
        }

        private void SetDungeonClock()
        {
            if (!GameStateManager.Instance.dungeonTimeHasElapsed) return;
            
            // if true we are coming from the restaurant, so reset timer
            clock.ResetClock();
            GameStateManager.Instance.dungeonTimeHasElapsed = false;
        }
    }
}

