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
            if (GameStateData.dungeonClock >= 1.0f || !gameController)
            {
                SceneManager.Instance.LoadRestaurantScene(true);
            }
            else
            {
                SceneManager.Instance.LoadDungeonScene();
            }
        }
    }
}
