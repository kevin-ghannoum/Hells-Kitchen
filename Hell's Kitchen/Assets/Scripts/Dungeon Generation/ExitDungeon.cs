using System;
using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Dungeon_Generation
{
    public class ExitDungeon : Interactable
    {
        protected override void Interact()
        {
            var gameController = FindObjectOfType<GameController>();
            if (GameStateData.dungeonClock > 1.0f || !gameController)
            {
                ReturnToRestaurant();
            }
            else
            {
                SceneManager.Instance.LoadDungeonScene();
            }
        }

        private void ReturnToRestaurant()
        {
            var player = NetworkHelper.GetLocalPlayerObject();
            var playerController = player.GetComponent<PlayerController>();
            if (playerController)
            {
                var heldWeapon = playerController.GetComponentInChildren<IPickup>();
                if (heldWeapon != null)
                {
                    GameStateData.carriedWeapon = WeaponInstance.None;
                    heldWeapon.RemoveFromPlayer();
                }
            }
            
            SceneManager.Instance.LoadRestaurantScene(true);
        }
    }
}
