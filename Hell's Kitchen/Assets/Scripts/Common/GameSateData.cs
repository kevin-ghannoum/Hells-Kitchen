using System.Collections.Generic;
using Common.Enums;
using PlayerInventory;
using UnityEngine;

namespace Common
{
    public static class GameStateData
    {
        public static float playerMaxHitPoints = 100f;
        public static float playerCurrentHitPoints;// = 100f;
        public static float playerMaxStamina = 5f;
        public static float playerCurrentStamina = 5f;
        
        public static WeaponInstance carriedWeapon = WeaponInstance.None;
        public static bool IsCarryingWeapon => (carriedWeapon != WeaponInstance.None);
        public static List<string> purchasedWeapons =  new List<string>();

        public static float cashMoney = 0f;
        public static float dungeonClock = 0;

        public static Inventory inventory = new Inventory();

        public static GameObject player = null;

        public static SousChefType sousChefType = SousChefType.Healer;

        public static int hiddenLevel = 0;
    }
}
