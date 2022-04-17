using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Input;
using UI;
using UnityEngine;

namespace Shops
{
    public class WeaponShopkeeper : MonoBehaviour
    {
        [SerializeField] private WeaponShopUI shopUI;
        
        private InputManager _input => InputManager.Instance;
        private List<GameObject> _weapons;

        private void Start()
        {
            _weapons = GameObject.FindGameObjectsWithTag(Tags.Weapon).ToList();
            foreach (var weapon in _weapons)
            {
                if(!GameStateManager.Instance.PurchasedWeapons.Contains(weapon.name))
                    weapon.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
            {
                shopUI.Initialize(_weapons);
            }
        }
    }
}
