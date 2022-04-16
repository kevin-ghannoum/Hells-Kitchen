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
                if(!GameStateManager.Instance.dungeonTimeHasElapsed)
                    SceneManager.Instance.LoadDungeonScene();
                else
                {
                    var playerController = other.gameObject.GetComponent<PlayerController>();
                    if (playerController)
                    {
                        var heldWeapon = playerController.GetComponentInChildren<IPickup>();
                        if (heldWeapon != null)
                        {
                            GameStateManager.Instance.carriedWeapon = WeaponInstance.None;
                            heldWeapon.RemoveFromPlayer();
                        }
                    }
                    SceneManager.Instance.LoadRestaurantScene();
                }
                   
            }
        }
    }
}
