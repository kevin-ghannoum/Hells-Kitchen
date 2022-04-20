using System;
using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using Photon.Pun;
using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dungeon_Generation
{
    public class ExitDungeon : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;

        private void Interact(InputAction.CallbackContext context)
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

        private void OnDestroy()
        {
            _input.reference.actions["Interact"].performed -= Interact;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    _input.reference.actions["Interact"].performed += Interact;
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    _input.reference.actions["Interact"].performed -= Interact;
                }
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

            GameStateData.playerCurrentHitPoints = GameStateData.playerMaxHitPoints;
            SceneManager.Instance.LoadRestaurantScene();
        }
    }
}
