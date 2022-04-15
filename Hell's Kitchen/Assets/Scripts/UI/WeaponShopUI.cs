using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Common.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private TextMeshProUGUI errorText;

        private List<GameObject> _weapons;
        private void Awake()
        {
            _weapons = GameObject.FindGameObjectsWithTag(Tags.Weapon).ToList();
            foreach (var weapon in _weapons)
            {
                if(!GameStateManager.Instance.PurchasedWeapons.Contains(weapon.name))
                    weapon.SetActive(false);
            }
            
            errorText.text = string.Empty;
            CreateButtons();
        }

        public void Initialize()
        {
            gameObject.SetActive(true);
            errorText.text = string.Empty;
        }

        private void BuyWeapon(GameObject weapon, Button button)
        {
            var weaponComponent = weapon.GetComponent<IWeapon>();
            if (weaponComponent == null)
                return;

            if (GameStateManager.Instance.cashMoney < weaponComponent.Price)
            {
                errorText.text = $"Insufficient funds to purchase {weapon.name}.";
                return;
            }

            weapon.SetActive(true);
            GameStateManager.Instance.cashMoney -= weaponComponent.Price;
            GameStateManager.Instance.PurchasedWeapons.Add(weapon.name);
            errorText.text = string.Empty;
            button.interactable = false;
        }

        private void CreateButtons()
        {
            foreach (var weapon  in _weapons)
            {
                var button = Instantiate(buttonPrefab, gridLayout.transform);
                var buttonComponent = button.GetComponent<Button>();
                buttonComponent.onClick.AddListener(delegate { BuyWeapon(weapon, buttonComponent); });

                if (GameStateManager.Instance.PurchasedWeapons.Contains(weapon.name))
                    buttonComponent.interactable = false;

                var text = button.GetComponentInChildren<TextMeshProUGUI>();
                var weaponComponent = weapon.GetComponent<IWeapon>();
                text.text = $"Buy {weapon.name} - {weaponComponent.Price}$";
            }
        }
    }
}
