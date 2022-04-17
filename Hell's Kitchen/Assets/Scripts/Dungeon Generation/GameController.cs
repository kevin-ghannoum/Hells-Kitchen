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

        private MazeConstructor generator;
        
        private bool goalReached;
        
        void Start() {
            generator = GetComponent<MazeConstructor>();
            StartNewGame();
        }

        private void StartNewGame()
        {
            StartNewMaze();
            SetUpPlayerWeapon();
            MovePlayerToStart();
            SetDungeonClock();
        }
        
        private void StartNewMaze()
        {
            generator.GenerateNewMaze(mazeStart);

            float x = generator.StartCol * generator.hallwayWidth - (generator.hallwayWidth / 2);
            float y = playerHeightPosition;
            float z = generator.StartRow * generator.hallwayWidth - (generator.hallwayWidth / 2);
            playerController.transform.position = new Vector3(x, y, z);

            goalReached = false;
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
            playerController.gameObject.transform.parent = mazeStart.parent;
            var playerPosition = playerController.gameObject.transform;
            var globalPosition = mazeStart.localPosition; //mazeStart.parent.TransformDirection(mazeStart.localPosition);
            Debug.Log($"Global position {globalPosition}");
            globalPosition.y += 0.2f;
            playerPosition.transform.localPosition = globalPosition;
            Debug.Log($"Player position {playerPosition.transform.localPosition}");
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

