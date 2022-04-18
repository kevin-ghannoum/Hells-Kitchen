using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using Player;
using UnityEngine;

namespace Dungeon_Generation
{
    public class ExitDungeon : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag(Tags.Player) && _input.interact)
            {
                var gameController = GameObject.FindObjectOfType<GameController>();
                if (GameStateManager.Instance.dungeonTimeHasElapsed || !gameController)
                    ReturnToRestaurant(other.gameObject);
                else
                    gameController.StartNewGame();
            }
        }

        private void ReturnToRestaurant(GameObject player)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController)
            {
                var heldWeapon = playerController.GetComponentInChildren<IPickup>();
                if (heldWeapon != null)
                {
                    GameStateManager.Instance.carriedWeapon = WeaponInstance.None;
                    heldWeapon.RemoveFromPlayer();
                }
            }

            GameStateManager.Instance.playerCurrentHitPoints = GameStateManager.Instance.playerMaxHitPoints;
            SceneManager.Instance.LoadRestaurantScene();
        }
    }
}
