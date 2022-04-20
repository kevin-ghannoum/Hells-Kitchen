using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Dungeon_Generation;
using UI;
using UnityEngine;

namespace Shops
{
    public class WeaponShopkeeper : Interactable
    {
        [SerializeField] private WeaponShopUI shopUI;

        private List<GameObject> _weapons;

        private void Start()
        {
            shopUI.Close();
            _weapons = GameObject.FindGameObjectsWithTag(Tags.Weapon).ToList();
            foreach (var weapon in _weapons)
            {
                if (!GameStateData.purchasedWeapons.Contains(weapon.name))
                    weapon.SetActive(false);
            }
        }

        protected override void Interact()
        {
            shopUI.Initialize(_weapons);
        }
    }
}
