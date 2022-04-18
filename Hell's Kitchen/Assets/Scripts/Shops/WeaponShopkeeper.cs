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
        [SerializeField] private GameObject interactText;
        [SerializeField] private WeaponShopUI shopUI;
        
        private InputManager _input => InputManager.Instance;
        private List<GameObject> _weapons;

        private void Start()
        {
            interactText.SetActive(false);
            _weapons = GameObject.FindGameObjectsWithTag(Tags.Weapon).ToList();
            foreach (var weapon in _weapons)
            {
                if(!GameStateData.purchasedWeapons.Contains(weapon.name))
                    weapon.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
            {
                interactText.SetActive(false);
                shopUI.Initialize(_weapons);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                interactText.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                interactText.SetActive(false);
            }
        }
    }
}
