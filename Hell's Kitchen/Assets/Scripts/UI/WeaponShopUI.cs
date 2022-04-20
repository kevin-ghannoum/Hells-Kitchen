using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Enums;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject weaponSlotPrefab;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private TextMeshProUGUI errorText;

        private List<GameObject> _weapons;
        private InputManager _input => InputManager.Instance;

        public void Initialize(List<GameObject> weapons)
        {
            _weapons = weapons;
            gameObject.SetActive(true);
            errorText.text = string.Empty;
            _input.Deactivate();
            CreateWeaponSlots();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            ClearGrid();
            _input.Activate();
        }

        private void BuyWeapon(GameObject weapon, Button button)
        {
            var weaponComponent = weapon.GetComponent<IWeapon>();
            if (weaponComponent == null)
                return;

            var weaponCost = weaponComponent.Price;
            if (GameStateManager.Instance.cashMoney < weaponCost)
            {
                errorText.text = $"Insufficient funds to purchase {weapon.name}.";
                return;
            }

            PerformTransaction(weapon.name,weaponCost);
            weapon.SetActive(true);
            errorText.text = string.Empty;
            button.interactable = false;
        }

        private void CreateWeaponSlots()
        {
            foreach (GameObject weapon  in _weapons)
            {
                GameObject weaponSlot = Instantiate(weaponSlotPrefab, gridLayout.transform);
                weaponSlot.GetComponentInChildren<Image>().sprite = WeaponSprites.GetSprite(weapon.name); // sprite
                
                Button buttonComponent = weaponSlot.GetComponentInChildren<Button>(); // button
                buttonComponent.onClick.AddListener(delegate { BuyWeapon(weapon, buttonComponent); });

                if (GameStateManager.Instance.purchasedWeapons.Contains(weapon.name))
                    buttonComponent.interactable = false;

                TextMeshProUGUI text = weaponSlot.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
                IWeapon weaponComponent = weapon.GetComponent<IWeapon>();
                text.text = $"Buy {weapon.name} - {weaponComponent.Price}$";
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < gridLayout.transform.childCount; i++)
                Destroy(gridLayout.transform.GetChild(i).gameObject);
        }

        private void PerformTransaction(string weaponName, float cost)
        {
            GameStateManager.Instance.cashMoney -= cost;
            GameStateManager.Instance.purchasedWeapons.Add(weaponName);
        }
    }
}
