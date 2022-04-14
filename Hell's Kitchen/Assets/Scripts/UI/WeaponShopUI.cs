using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GridLayoutGroup gridLayout;

        private List<GameObject> _weapons;
        private void Awake()
        {
            _weapons = GameObject.FindGameObjectsWithTag(Tags.Weapon).ToList();
            foreach(var weapon in _weapons)
                weapon.SetActive(false);
            
            CreateButtons();
        }

        public void BuyWeapon(GameObject weapon)
        {
            if(!GameStateManager.Instance.PurchasedWeapons.Contains(weapon))
                GameStateManager.Instance.PurchasedWeapons.Add(weapon);

            var weaponComponent = weapon.GetComponent<IWeapon>();
            if (weaponComponent == null)
                return;
            
            if (GameStateManager.Instance.cashMoney >= weaponComponent.Price)
            {
                weapon.SetActive(true);
                GameStateManager.Instance.cashMoney -= weaponComponent.Price;
            }
            // TODO some sort of message saying that you dont have enough cash money
        }

        private void CreateButtons()
        {
            foreach (var weapon  in _weapons)
            {
                var button = Instantiate(buttonPrefab, gridLayout.transform);
                var buttonComponent = button.GetComponent<Button>();
                buttonComponent.onClick.AddListener(delegate { BuyWeapon(weapon); });
            }
        }
    }
}
